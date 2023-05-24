using UnityEngine;
using UnityEngine.Serialization;

namespace _GAME_.Scripts.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        #region Public Variables

        public static PlayerInputHandler Instance { get; private set; }
        public float HorizontalMovement => Input.GetAxisRaw("Horizontal");
        public float VerticalMovement => Input.GetAxisRaw("Vertical");
        public float HorizontalLookInput => Input.GetAxisRaw("Mouse X") * horizontalSensitivity;
        public float VerticalLookInput => Input.GetAxisRaw("Mouse Y") * verticalSensitivity;
        public bool IsSprinting => Input.GetButton("Sprint");
        public bool IsJumping => Input.GetButtonDown("Jump");
        public bool IsFiring => Input.GetButton("Fire1");
        public bool IsMeleeAttacking => Input.GetButton("Fire2");

        #endregion

        #region Private Variables

        [FormerlySerializedAs("_horizontalsensitivity")] [SerializeField] private float horizontalSensitivity;
        [FormerlySerializedAs("_verticalsensitivity")] [SerializeField] private float verticalSensitivity;

        #endregion
        
        #region Monobehavious Methods

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        #endregion
    }
}
