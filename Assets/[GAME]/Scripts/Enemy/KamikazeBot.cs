using System.Collections;
using _GAME_.Scripts.Enums;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Interfaces;
using _GAME_.Scripts.Managers;
using _GAME_.Scripts.Scriptable_Objects.Enemy;
using UnityEngine;

namespace _GAME_.Scripts.Enemy
{
    public class KamikazeBot : BaseEnemy
    {
        #region Private Variables

        private float _explosionRadius;
        private float _explosionDamage;
        private float _explosionDelay;
        
        private bool _isExploding = false;

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
            if (_isExploding)
                return;
            
            StartCoroutine(ExplodeDelay());
        }

        protected override void Node2()
        {
            if (PlayerTransform == null || _isExploding)
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

        #endregion

        #region Private Methods
        
        private void GetDataFromScriptable()
        {
            var kamikazeBotData = Resources.Load<KamikazeBotScriptableObject>(FolderPaths.KAMIKAZE_BOT_DATA_PATH);
            
            GetBaseVariables(kamikazeBotData);
            
            _explosionRadius = kamikazeBotData.ExplosionRadius;
            _explosionDamage = kamikazeBotData.ExplosionDamage;
            _explosionDelay = kamikazeBotData.ExplosionDelay;
        }
        
        
        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator ExplodeDelay()
        {
            _isExploding = true;
            
            Agent.speed *= 1.3f;
            
            yield return new WaitForSeconds(_explosionDelay);
            
            // ReSharper disable once Unity.PreferNonAllocApi
            var colliders = Physics.OverlapSphere(transform.position, _explosionRadius);
            foreach (var col in colliders)
            {
                var damageable = col.GetComponent<IDamageable>();
                damageable?.TakeDamage(_explosionDamage, DamageType.Melee, DamageCauser.Enemy);
            }
            
            GameManager.Instance.aliveEnemies.Remove(gameObject);
            Destroy(gameObject);
        }

        #endregion
    }
}