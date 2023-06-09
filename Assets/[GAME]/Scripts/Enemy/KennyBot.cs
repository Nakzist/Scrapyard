﻿using System.Collections;
using _GAME_.Scripts.Enums;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Interfaces;
using _GAME_.Scripts.Scriptable_Objects.Enemy;
using UnityEngine;

namespace _GAME_.Scripts.Enemy
{
    public class KennyBot : BaseEnemy
    {
        #region Private Variables

        private bool _canAttack = true;
        private float _attackCooldown;
        private float _attackAngle;
        private LayerMask _playerLayerMask;

        #endregion

        #region Monobehaviour Methods

        protected override void Start()
        {
            base.Start();
            
            GetDataFromScriptable();
        }

        #endregion

        #region Override Methods

        protected override void Node1()
        {
            if(!_canAttack)
                return;

            StartCoroutine(MeleeAttack());
        }

        protected override void Node2()
        {
            if (PlayerTransform == null)
                return;
            
            // Move towards the player
            RotateTowardsTarget(PlayerTransform, RotationSpeed);
            MoveToDestination(PlayerTransform.position);
        }

        protected override bool SelectorNode()
        {
            if (PlayerTransform == null)
                return false;
            
            return Vector3.Distance(transform.position, PlayerTransform.position) <= AttackRange;
        }

        public override void TakeDamage(float incomingDamage, DamageType damageType, DamageCauser damageCauser)
        {
            if(damageType == DamageType.Ranged)
                return;
            
            base.TakeDamage(incomingDamage, damageType, DamageCauser.Enemy);
        }

        #endregion

        #region Private Methods

        private void GetDataFromScriptable()
        {
            var kennyBotData = Resources.Load<KennyBotScriptableObject>(FolderPaths.KENNY_BOT_DATA_PATH);

            _attackCooldown = kennyBotData.AttackCooldown;
            _attackAngle = kennyBotData.AttackAngle;
            _playerLayerMask = kennyBotData.PlayerLayerMask;
            
            GetBaseVariables(kennyBotData);
        }

        private IEnumerator MeleeAttack()
        {
            ApplyDamage();
            _canAttack = false;
            yield return new WaitForSeconds(_attackCooldown);
            _canAttack = true;
        }

        private void ApplyDamage()
        {
            // ReSharper disable once Unity.PreferNonAllocApi
            var hitColliders = Physics.OverlapSphere(transform.position, AttackRange, _playerLayerMask);
            
            foreach(var hitCollider in hitColliders)
            {
                var t = transform;
                var directionToTarget = hitCollider.transform.position - t.position;
                directionToTarget.y = 0f;
                
                var angleToTarget = Vector3.Angle(t.forward, directionToTarget);
                
                if (angleToTarget <= _attackAngle * 0.5f)
                {
                    if (hitCollider.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.TakeDamage(Damage, DamageType.Melee, DamageCauser.Enemy);
                    }
                }
            }
        }

        #endregion
    }
}