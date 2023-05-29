using System.Collections;
using _GAME_.Scripts.Managers;
using _GAME_.Scripts.Player;
using UnityEngine;

namespace _GAME_.Scripts.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class FollowerOldEnemy : BaseOldEnemy
    {
        private Vector3 _velocity;
        public bool isQueen;
        
        private void Update()
        {
            _velocity = Vector3.zero;
            if (Vector3.Distance(PlayerInputHandler.Instance.gameObject.transform.position, transform.position) <= range && LastAttackTime + delayBetweenAttacks <= Time.time)
            {
                StartCoroutine(Attack());
            }
            else
            {
                if (CanMove)
                {
                    _velocity = (PlayerInputHandler.Instance.gameObject.transform.position - transform.position).normalized *
                                moveSpeed;
                    _velocity.y = 0;
                }
            }
            Rb.velocity = _velocity + new Vector3(0,Rb.velocity.y,0);
        }

        private IEnumerator Attack()
        {
            CanMove = false;
            LastAttackTime = Time.time;
            PlayerHealthManager.Instance.TakeDamage(damage);
            yield return new WaitForSeconds(1f);
            CanMove = true;
        }
        
        private protected override void EnemyDeath()
        {
            GameManager.AliveEnemies.Remove(gameObject);
            if (isQueen)
            {
                GameManager.OnQueenDied?.Invoke();
            }
            GameManager.OnEnemyDied?.Invoke();
            Debug.Log("die");
            Destroy(gameObject);
        }
    }
}
