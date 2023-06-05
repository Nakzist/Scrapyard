using UnityEngine;

namespace _GAME_.Scripts.Scriptable_Objects.Enemy
{
    [CreateAssetMenu(fileName = "Spawner Bot Data", menuName = "Scrapyard/Data/Enemy/Spawner Bot Data")]
    public class SpawnerBotScriptableObject : BaseEnemyScriptableObject
    {
        #region Serialized Variables
        
        [Header("Spawner Bot Movement Settings")]
        [SerializeField] private float roamRadius = 5f;

        [Header("Spawner Bot Combat Settings")]
        [SerializeField] private float spawnCooldown = 10f;
        [SerializeField] private float spawnAmount = 1f;
        [SerializeField] private GameObject spawnPrefab;

        #endregion

        #region Public Variables
        
        public float RoamRadius => roamRadius;

        public float SpawnCooldown => spawnCooldown;
        public float SpawnAmount => spawnAmount;
        public GameObject SpawnPrefab => spawnPrefab;

        #endregion
    }
}