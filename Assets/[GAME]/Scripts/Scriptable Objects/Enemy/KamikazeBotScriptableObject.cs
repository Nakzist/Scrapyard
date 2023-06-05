using UnityEngine;

namespace _GAME_.Scripts.Scriptable_Objects.Enemy
{
    [CreateAssetMenu(fileName = "Kamikaze Bot Data", menuName = "Scrapyard/Data/Enemy/Kamikaze Bot Data")]
    public class KamikazeBotScriptableObject : BaseEnemyScriptableObject
    {
        #region Serialized Variables
        
        [Header("Sniper Bot Combat Settings")] 
        [SerializeField] private float explosionRadius = 5f;
        [SerializeField] private float explosionDamage = 100f;
        [SerializeField] private float explosionDelay = 1f;

        #endregion

        #region Public Variables
        
        public float ExplosionRadius => explosionRadius;
        public float ExplosionDamage => explosionDamage;
        public float ExplosionDelay => explosionDelay;

        #endregion
    }
}