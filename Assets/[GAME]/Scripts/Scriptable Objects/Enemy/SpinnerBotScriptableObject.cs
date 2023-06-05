using UnityEngine;

namespace _GAME_.Scripts.Scriptable_Objects.Enemy
{
    [CreateAssetMenu(fileName = "Spinner Bot Data", menuName = "Scrapyard/Data/Enemy/Spinner Bot Data")]
    public class SpinnerBotScriptableObject : BaseEnemyScriptableObject
    {
        #region Serialized Variables

        [Header("Spinner Bot Combat Settings")]
        [SerializeField] private float attackCooldown = 3f;
        [SerializeField] private float attackAngle = 360f;
        [SerializeField] private LayerMask playerLayerMask;

        #endregion

        #region Public Variables

        public float AttackCooldown => attackCooldown;
        public float AttackAngle => attackAngle;
        public LayerMask PlayerLayerMask => playerLayerMask;

        #endregion
    }
}