using UnityEngine;

namespace _GAME_.Scripts.Scriptable_Objects.Enemy
{
    [CreateAssetMenu(fileName = "Boss Data", menuName = "Scrapyard/Data/Enemy/Boss Data")]
    public class BossScriptableObject : BaseEnemyScriptableObject
    {
        #region Serialized Variables

        [SerializeField] private float chargeDuration = 2f;

        #endregion

        #region Public Variables

        public float ChargeDuration => chargeDuration;

        #endregion
    }
}