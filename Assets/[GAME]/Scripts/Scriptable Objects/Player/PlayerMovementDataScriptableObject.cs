using UnityEngine;
using UnityEngine.Serialization;

namespace _GAME_.Scripts.Scriptable_Objects.Player
{
    [CreateAssetMenu(fileName = "Player Movement Data", menuName = "Scrapyard/Data/Player/Player Movement Data")]
    public class PlayerMovementDataScriptableObject : ScriptableObject
    {
        #region Serialized Variables

        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float sprintMoveSpeed = 10f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private LayerMask groundLayerMask;
        
        [Header("Jump Settings")]
        [SerializeField] private float bunnyHopWindow = 0.1f;
        [SerializeField] private float bunnyHopBoost = 2.0f;
        [SerializeField] private float jumpDebounceTime = 0.2f;

        #endregion

        #region Public Variables

        public float MoveSpeed => moveSpeed;
        public float SprintMoveSpeed => sprintMoveSpeed;
        public float JumpForce => jumpForce;
        public LayerMask GroundLayerMask => groundLayerMask;
        public float BunnyHopWindow => bunnyHopWindow;
        public float BunnyHopBoost => bunnyHopBoost;
        public float JumpDebounceTime => jumpDebounceTime;

        #endregion
    }
}