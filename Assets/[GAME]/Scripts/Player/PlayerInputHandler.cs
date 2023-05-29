using UnityEngine;
using UnityEngine.Serialization;

namespace _GAME_.Scripts.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        #region Public Variables
        
        public float HorizontalMovement => Input.GetAxisRaw("Horizontal");
        public float VerticalMovement => Input.GetAxisRaw("Vertical");
        public float HorizontalLookInput => Input.GetAxisRaw("Mouse X") * horizontalSensitivity;
        public float VerticalLookInput => Input.GetAxisRaw("Mouse Y") * verticalSensitivity;
        public bool IsSprinting => Input.GetButton("Sprint");
        public bool IsJumping => Input.GetButtonDown("Jump");
        public bool IsFiring => Input.GetButton("Fire1");
        public bool IsMeleeAttacking => Input.GetButton("Fire2");

        #endregion

        #region Serialized Variables

        [SerializeField] private float horizontalSensitivity;
        [SerializeField] private float verticalSensitivity;

        #endregion
    }
}
