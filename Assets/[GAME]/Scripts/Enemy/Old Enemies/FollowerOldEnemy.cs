using System.Collections;
using _GAME_.Scripts.Enums;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Managers;
using _GAME_.Scripts.Player;
using UnityEngine;

namespace _GAME_.Scripts.Enemy.Old_Enemies
{
    [RequireComponent(typeof(Rigidbody))]
    public class FollowerOldEnemy : BaseOldEnemy
    {
        #region Private Variables

        private Vector3 _velocity;
        public bool isQueen;

        #endregion

        #region Monobehaviour Methods

        private void Update()
        {
            _velocity = Vector3.zero;
            
            if(GameManager.Instance == null || GameManager.Instance.currentPlayer == null) return;
            
            if (Vector3.Distance(GameManager.Instance.currentPlayer.transform.position, transform.position) <= range && LastAttackTime + delayBetweenAttacks <= Time.time)
            {
                StartCoroutine(Attack());
            }
            else
            {
                if (CanMove)
                {
                    _velocity = (GameManager.Instance.currentPlayer.transform.position - transform.position).normalized *
                                moveSpeed;
                    _velocity.y = 0;
                }
            }
            Rb.velocity = _velocity + new Vector3(0,Rb.velocity.y,0);
        }

        #endregion

        #region Private Methods

        private IEnumerator Attack()
        {
            CanMove = false;
            LastAttackTime = Time.time;
            GameManager.Instance.currentPlayer.GetComponent<PlayerHealthManager>().TakeDamage(damage, DamageType.Melee, DamageCauser.Enemy);
            yield return new WaitForSeconds(1f);
            CanMove = true;
        }
        
        private protected override void EnemyDeath(DamageCauser damageCauser)
        {
            GameManager.Instance.aliveEnemies.Remove(gameObject);
            
            if (isQueen)
            {
                Push(CustomEvents.OnQueenDeath);
            }
            if(damageCauser == DamageCauser.Player)
                Push(CustomEvents.OnEnemyDeath);
            
            Destroy(gameObject);
        }

        #endregion
    }
}
