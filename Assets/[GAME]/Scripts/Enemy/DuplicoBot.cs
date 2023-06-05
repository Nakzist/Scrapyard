using System.Collections;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Interfaces;
using _GAME_.Scripts.Scriptable_Objects.Enemy;
using UnityEngine;

namespace _GAME_.Scripts.Enemy
{
    public class DuplicoBot : BaseEnemy
    {
        #region Private Variables
        
        private bool _canAttack = true;
        private float _attackCooldown;
        private float _attackAngle;
        private LayerMask _playerLayerMask;

        private float _enemyToSpawn;
        private GameObject _enemyPrefab;

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

        private protected override void EnemyDeath()
        {
            SpawnEnemy();
            base.EnemyDeath();
        }

        #endregion

        #region Private Methods

        private void GetDataFromScriptable()
        {
            var duplicoBotData = Resources.Load<DuplicoBotScriptableObject>(FolderPaths.DUPLICO_BOT_DATA_PATH);
            
            _attackCooldown = duplicoBotData.AttackCooldown;
            _attackAngle = duplicoBotData.AttackAngle;
            _playerLayerMask = duplicoBotData.PlayerLayerMask;
            
            _enemyToSpawn = duplicoBotData.EnemyToSpawn;
            _enemyPrefab = duplicoBotData.EnemyPrefab;
            
            GetBaseVariables(duplicoBotData);
        }

        private void SpawnEnemy()
        {
            for (var i = 0; i < _enemyToSpawn; i++)
            {
                var t = transform;
                Instantiate(_enemyPrefab, t.position, t.rotation);
            }
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
                        damageable.TakeDamage(Damage);
                    }
                }
            }
        }

        #endregion
    }
}