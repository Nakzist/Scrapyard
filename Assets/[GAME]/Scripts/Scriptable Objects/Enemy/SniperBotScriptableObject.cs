using UnityEngine;

namespace _GAME_.Scripts.Scriptable_Objects.Enemy
{
    [CreateAssetMenu(fileName = "Sniper Bot Data", menuName = "Scrapyard/Data/Enemy/Sniper Bot Data")]
    public class SniperBotScriptableObject : BaseEnemyScriptableObject
    {
        #region Serialized Variables

        [Header("Sniper Bot Movement Settings")] 
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float moveRange = 10f;

        [Header("Sniper Bot Combat Settings")] 
        [SerializeField] private float delayBetweenShoots = 1f;
        [SerializeField] private float dodgeSpeedThreshold = 7.5f;

        #endregion

        #region Public Variables

        public float RotationSpeed => rotationSpeed;
        public float MoveRange => moveRange;
        public float DelayBetweenShoots => delayBetweenShoots;
        public float DodgeSpeedThreshold => dodgeSpeedThreshold;

        #endregion
    }
}