using _GAME_.Scripts.Enums;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Interfaces;
using _GAME_.Scripts.Managers;
using UnityEngine;

namespace _GAME_.Scripts.Enemy.Old_Enemies
{
    public class ExplodingOldEnemy : BaseOldEnemy
    {
        #region Serialized Variables

        [SerializeField] private GameObject areaPrefab;

        #endregion

        #region Monobehaviour Methods

        private void Update()
        {
            if (!CanMove) return;
            
            if(GameManager.Instance == null || GameManager.Instance.currentPlayer) return;
            
            if (Vector3.Distance(GameManager.Instance.currentPlayer.transform.position, transform.position) <= range && LastAttackTime + delayBetweenAttacks <= Time.time)
            {
                Attack();
            }
            else
            {
                var velocity = (GameManager.Instance.currentPlayer.transform.position - transform.position).normalized * moveSpeed;
                velocity.y = 0;
                transform.LookAt(velocity);
                Rb.velocity = velocity + new Vector3(0,Rb.velocity.y,0);
            }
        }

        #endregion

        #region Private Variables

        private void Attack()
        {
            EnemyDeath(DamageCauser.Enemy);
        }
        
        private protected override void EnemyDeath(DamageCauser damageCauser)
        {
            IsDead = true;
            GameManager.Instance.aliveEnemies.Remove(gameObject);
            
            if(damageCauser == DamageCauser.Player)
                Push(CustomEvents.OnEnemyDeath);
            
            var sphere = Instantiate(areaPrefab, transform.position, Quaternion.identity);
            Destroy(sphere, .3f);
            // ReSharper disable once Unity.PreferNonAllocApi
            foreach (var col in Physics.OverlapSphere(transform.position, 3f))
            {
                if (col.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(damage, DamageType.Melee, DamageCauser.Enemy);
                }
            }
            Destroy(gameObject);
        }

        #endregion
    }
}
