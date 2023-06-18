using _GAME_.Scripts.Enums;
using _GAME_.Scripts.Interfaces;
using _GAME_.Scripts.Observer;
using UnityEngine;

namespace _GAME_.Scripts.Enemy.Old_Enemies
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseOldEnemy : ObserverBase, IDamageable
    {
        [SerializeField] private protected float range = 1f;
        [SerializeField] private protected float damage = 5f;
        [SerializeField] private protected float moveSpeed = 5f;
        [SerializeField] private protected float delayBetweenAttacks = 5f;
        [SerializeField] private protected float maxHealth = 5f;

        private protected float CurrentHealth;
        private protected float LastAttackTime;
        private protected bool CanMove;
        private protected bool IsDead;
        private protected Rigidbody Rb;

        public void TakeDamage(float incomingDamage, DamageType damageType, DamageCauser damageCauser)
        {
            if (IsDead) return;
            
            CurrentHealth -= incomingDamage;
            if(CurrentHealth <= 0)
                EnemyDeath(damageCauser);
        }

        private protected abstract void EnemyDeath(DamageCauser damageCauser);

        protected virtual void Start()
        {
            CurrentHealth = maxHealth;
            Rb = GetComponent<Rigidbody>();
        }
    }
}