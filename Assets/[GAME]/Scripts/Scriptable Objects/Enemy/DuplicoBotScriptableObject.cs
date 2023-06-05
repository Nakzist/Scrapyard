using UnityEngine;

namespace _GAME_.Scripts.Scriptable_Objects.Enemy
{
    [CreateAssetMenu(fileName = "Duplico Bot Data", menuName = "Scrapyard/Data/Enemy/Duplico Bot Data")]
    public class DuplicoBotScriptableObject : BaseEnemyScriptableObject
    {
        #region Serialized Variables

        [Header("Duplico Bot Combat Settings")] 
        [SerializeField] private float attackCooldown = 1f;
        [SerializeField] private float attackAngle = 60f;
        [SerializeField] private LayerMask playerLayerMask;
        
        [Header("Duplico Bot Death Settings")]
        [SerializeField] private float enemyToSpawn = 2f;
        [SerializeField] private GameObject enemyPrefab;

        #endregion

        #region Public Variables
        
        public float AttackCooldown => attackCooldown;
        public float AttackAngle => attackAngle;
        public LayerMask PlayerLayerMask => playerLayerMask;

        public float EnemyToSpawn => enemyToSpawn;
        public GameObject EnemyPrefab => enemyPrefab;

        #endregion
    }
}