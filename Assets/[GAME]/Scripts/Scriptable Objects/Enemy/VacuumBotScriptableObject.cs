using UnityEngine;

namespace _GAME_.Scripts.Scriptable_Objects.Enemy
{
    [CreateAssetMenu(fileName = "Vacuum Bot Data", menuName = "Scrapyard/Data/Enemy/Vacuum Bot Data")]
    public class VacuumBotScriptableObject : BaseEnemyScriptableObject   
    {
        #region Serialized Variables

        [Header("Pursuer Bot Combat Settings")]
        [SerializeField] private float attackCooldown = 1f;
        [SerializeField] private float attackAngle = 60f;
        [SerializeField] private LayerMask playerLayerMask;

        #endregion

        #region Public Variables

        public float AttackCooldown => attackCooldown;
        public float AttackAngle => attackAngle;
        public LayerMask PlayerLayerMask => playerLayerMask;

        #endregion
    }
}