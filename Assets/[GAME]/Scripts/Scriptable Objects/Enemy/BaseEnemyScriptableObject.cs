﻿using UnityEngine;

namespace _GAME_.Scripts.Scriptable_Objects.Enemy
{
    public abstract class BaseEnemyScriptableObject : ScriptableObject
    {
        #region Serialized Variables

        [Header("Movement Settings")] 
        [SerializeField] private float speed = 4f;
        [SerializeField] private float angularSpeed = 720f;
        [SerializeField] private float acceleration = 8f;
        [SerializeField] private float rotationSpeed = 10f;

        [Header("Combat Settings")] 
        [SerializeField] private float maxHealth = 10f;
        [SerializeField] private float attackRange = 10f;
        [SerializeField] private float damage = 5f;

        #endregion

        #region Public Variables

        public float Speed => speed;
        public float AngularSpeed => angularSpeed;
        public float Acceleration => acceleration;
        public float RotationSpeed => rotationSpeed;
        
        public float MaxHealth => maxHealth;
        public float AttackRange => attackRange;
        public float Damage => damage;

        #endregion
    }
}