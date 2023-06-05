using System.Collections;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Scriptable_Objects.Enemy;
using UnityEngine;

namespace _GAME_.Scripts.Enemy
{
    public class SpawnerBot : BaseEnemy
    {
        #region Private Variables

        private float _roamRadius;
        
        private float _spawnCooldown;
        private float _spawnAmount;
        private GameObject _spawnPrefab;
        
        private bool _canSpawn = true;
        private bool _walkingToLocation = false;

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
            StartCoroutine(SpawnEnemy());
        }

        protected override void Node2()
        {
            if(PlayerTransform == null)
                return;

            StartCoroutine(WaitForDestination());
            
            RotateTowardsTarget(PlayerTransform, RotationSpeed);
        }

        protected override bool SelectorNode()
        {
            return _canSpawn;
        }

        #endregion

        #region Private Methods

        private void GetDataFromScriptable()
        {
            var spawnerBotData = Resources.Load<SpawnerBotScriptableObject>(FolderPaths.SPAWNER_BOT_DATA_PATH);
            
            _roamRadius = spawnerBotData.RoamRadius;
            
            _spawnCooldown = spawnerBotData.SpawnCooldown;
            _spawnAmount = spawnerBotData.SpawnAmount;
            _spawnPrefab = spawnerBotData.SpawnPrefab;
            
            GetBaseVariables(spawnerBotData);
        }
        
        private IEnumerator WaitForDestination()
        {
            if (_walkingToLocation) yield break;
            
            _walkingToLocation = true;
            
            var dest = FindLocationInRadius();
            
            MoveToDestination(dest);

            while (Agent.pathPending || Agent.remainingDistance > Agent.stoppingDistance)
            {
                yield return null;
            }

            _walkingToLocation = false;

        }

        private Vector3 FindLocationInRadius()
        {
            var randomPoint = Random.insideUnitCircle * _roamRadius;
            var targetPosition = transform.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
            return targetPosition;
        }

        private IEnumerator SpawnEnemy()
        {
            if (!_canSpawn)
                yield break;
            
            _canSpawn = false;
            for(var i = 0; i < _spawnAmount; i++)
                Instantiate(_spawnPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(_spawnCooldown);
            _canSpawn = true;
        }

        #endregion
    }
}