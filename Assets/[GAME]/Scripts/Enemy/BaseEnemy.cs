﻿using _GAME_.Scripts.Interfaces;
using UnityEngine;

namespace _GAME_.Scripts.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseEnemy : MonoBehaviour, IDamageable
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

        public virtual void TakeDamage(float incomingDamage)
        {
            if (IsDead) return;
            
            CurrentHealth -= incomingDamage;
            if(CurrentHealth <= 0)
                EnemyDeath();
        }
        
        private protected abstract void EnemyDeath();

        protected virtual void Start()
        {
            CurrentHealth = maxHealth;
            Rb = GetComponent<Rigidbody>();
        }
    }
}