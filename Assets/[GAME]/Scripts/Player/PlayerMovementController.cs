using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Scriptable_Objects.Player;
using UnityEngine;

namespace _GAME_.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovementController : MonoBehaviour
    {
        #region Private Variables

        [Header("Movement")]
        private float _moveSpeed;
        private float _sprintMoveSpeed;
        private float _jumpForce;
        private LayerMask _groundMask;
        private Transform _groundChecker;
        
        private bool _isGrounded = true;
    
        private float _cameraVerticalAngle;

        private Camera _mainCamera;
        private Rigidbody _rb;
        
        private PlayerMovementDataScriptableObject _playerMovementData;

        #endregion

        #region Monobehavious Methods

        private void Start()
        {
            GetDataFromScriptable();
            _groundChecker = transform.GetChild(0);
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _rb = GetComponent<Rigidbody>();
            _mainCamera = Camera.main;
            if (_mainCamera != null) _mainCamera.transform.rotation = transform.rotation;
        }

        private void Update()
        {
            MovePlayer();
            HandleJump();
        }

        private void FixedUpdate()
        {
            transform.Rotate(
                new Vector3(0f, PlayerInputHandler.Instance.HorizontalLookInput,
                    0f), Space.Self);
        }

        private void LateUpdate()
        {
            _cameraVerticalAngle -= PlayerInputHandler.Instance.VerticalLookInput;
            _cameraVerticalAngle = Mathf.Clamp(_cameraVerticalAngle, -89f, 89f);
            var cameraTransform = _mainCamera.transform;
            cameraTransform.eulerAngles = new Vector3(_cameraVerticalAngle, cameraTransform.eulerAngles.y, 0);
        }
        
        #endregion

        #region Private Methods

        private void GetDataFromScriptable()
        {
            _playerMovementData = Resources.Load<PlayerMovementDataScriptableObject>(FolderPaths.MOVEMENT_DATA_PATH);


            _moveSpeed = _playerMovementData.MoveSpeed;
            _sprintMoveSpeed = _playerMovementData.SprintMoveSpeed;
            _jumpForce = _playerMovementData.JumpForce;
            _groundMask = _playerMovementData.GroundLayerMask;

        }
        
        private void MovePlayer()
        {
            var t = transform;
            var playerMovement = t.forward * PlayerInputHandler.Instance.VerticalMovement +
                                 t.right * PlayerInputHandler.Instance.HorizontalMovement;
            playerMovement *= PlayerInputHandler.Instance.IsSprinting ? _sprintMoveSpeed : _moveSpeed;
            playerMovement.y += _rb.velocity.y;
            _rb.velocity = playerMovement;
        }
        
        private void HandleJump()
        {
            // ReSharper disable once Unity.PreferNonAllocApi
            _isGrounded = Physics.OverlapSphere(transform.position, .1f, _groundMask).Length != 0;
            if (_isGrounded && PlayerInputHandler.Instance.IsJumping)
            {
                Debug.Log("jump");
                _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
            }
        }

        #endregion
    }
}