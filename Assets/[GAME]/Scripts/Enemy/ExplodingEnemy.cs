using _GAME_.Scripts.Interfaces;
using _GAME_.Scripts.Managers;
using _GAME_.Scripts.Player;
using UnityEngine;

namespace _GAME_.Scripts.Enemy
{
    public class ExplodingEnemy : BaseEnemy
    {
        [SerializeField] private GameObject areaPrefab;

        private void Update()
        {
            if (!CanMove) return;
            
            if (Vector3.Distance(PlayerInputHandler.Instance.transform.position, transform.position) <= range && LastAttackTime + delayBetweenAttacks <= Time.time)
            {
                Attack();
            }
            else
            {
                var velocity = (PlayerInputHandler.Instance.transform.position - transform.position).normalized * moveSpeed;
                velocity.y = 0;
                transform.LookAt(velocity);
                Rb.velocity = velocity + new Vector3(0,Rb.velocity.y,0);
            }
        }

        private void Attack()
        {
            EnemyDeath();
        }
        
        private protected override void EnemyDeath()
        {
            IsDead = true;
            GameManager.AliveEnemies.Remove(gameObject);
            GameManager.OnEnemyDied?.Invoke();
            var sphere = Instantiate(areaPrefab, transform.position, Quaternion.identity);
            Destroy(sphere, .3f);
            foreach (var col in Physics.OverlapSphere(transform.position, 3f))
            {
                if (col.TryGetComponent(out IDamageable damagable))
                {
                    damagable.TakeDamage(damage);
                }
            }
            Destroy(gameObject);
        }
    }
}
