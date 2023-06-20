using UnityEngine;
// ReSharper disable MemberCanBeMadeStatic.Global

namespace _GAME_.Scripts.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        #region Public Variables
        
        public float HorizontalMovement => Input.GetAxisRaw("Horizontal");
        public float VerticalMovement => Input.GetAxisRaw("Vertical");
        public float HorizontalLookInput => Input.GetAxisRaw("Mouse X") * sensitivity;
        public float VerticalLookInput => Input.GetAxisRaw("Mouse Y") * sensitivity;
        public bool IsSprinting => Input.GetButton("Sprint");
        public bool IsJumping => Input.GetButtonDown("Jump");
        public bool IsReloading => Input.GetButtonDown("Reload");
        public bool IsFiring => Input.GetButton("Fire1");
        public bool IsMeleeAttacking => Input.GetButton("Fire2");

        #endregion

        #region Serialized Variables

        [SerializeField] private float sensitivity = 2f;

        #endregion
    }
}
