using UnityEngine;

namespace _GAME_.Scripts.Scriptable_Objects.Player
{
    [CreateAssetMenu(fileName = "PlayerMovementData", menuName = "Scrapyard/Data/Player Movement Data")]
    public class PlayerMovementDataScriptableObject : ScriptableObject
    {
        #region Serialized Variables

        [Header("Movement Data")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float sprintMoveSpeed = 10f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private LayerMask groundLayerMask;

        #endregion

        #region Public Variables

        public float MoveSpeed => moveSpeed;
        public float SprintMoveSpeed => sprintMoveSpeed;
        public float JumpForce => jumpForce;
        public LayerMask GroundLayerMask => groundLayerMask;

        #endregion
    }
}