using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Scriptable_Objects.Player;
using UnityEngine;

namespace _GAME_.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovementController : MonoBehaviour
    {
        #region Private Variables
        
        private float _moveSpeed;
        private float _sprintMoveSpeed;
        private float _jumpForce;
        private LayerMask _groundMask;

        private bool _isGrounded = true;
    
        private float _cameraVerticalAngle;

        private Camera _mainCamera;
        private Rigidbody _rb;
        private Animator _animator;
        
        private PlayerMovementDataScriptableObject _playerMovementData;
        private PlayerInputHandler _playerInputHandler;
        
        private bool _justJumped = false;
        private float _bunnyHopTimer = 0.0f;
        private float _jumpDebounce;

        private float _bunnyHopWindow;
        private float _bunnyHopBoost;
        private float _jumpDebounceTime;
        private static readonly int Speed = Animator.StringToHash("Speed");

        #endregion

        #region Monobehavious Methods

        private void Start()
        {
            GetDataFromScriptable();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _playerInputHandler = GetComponent<PlayerInputHandler>();
            _mainCamera = Camera.main;
            if (_mainCamera != null) _mainCamera.transform.rotation = transform.rotation;
        }

        private void Update()
        {
            MovePlayer();
            
            // Reduce the jump debounce timer if it's greater than 0
            if (_jumpDebounce > 0)
            {
                _jumpDebounce -= Time.deltaTime;
            }
            
            HandleJump();
            
            // Decrease the bunny hop timer
            if (_justJumped)
            {
                _bunnyHopTimer -= Time.deltaTime;
                
                // If the bunny hop timer runs out, the player cannot bunny hop
                if(_bunnyHopTimer <= 0.0f)
                {
                    _justJumped = false;
                }
            }
        }

        private void FixedUpdate()
        {
            transform.Rotate(
                new Vector3(0f, _playerInputHandler.HorizontalLookInput,
                    0f), Space.Self);
        }

        private void LateUpdate()
        {
            _cameraVerticalAngle -= _playerInputHandler.VerticalLookInput;
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

            _bunnyHopWindow = _playerMovementData.BunnyHopWindow;
            _bunnyHopBoost = _playerMovementData.BunnyHopBoost;
            _jumpDebounceTime = _playerMovementData.JumpDebounceTime;
        }
        
        private void MovePlayer()
        {
            var t = transform;
            var playerMovement = t.forward * _playerInputHandler.VerticalMovement +
                                 t.right * _playerInputHandler.HorizontalMovement;
            playerMovement *= _playerInputHandler.IsSprinting ? _sprintMoveSpeed : _moveSpeed;
            playerMovement.y += _rb.velocity.y;
            _rb.velocity = playerMovement;
            
            var speed = playerMovement.magnitude;
            _animator.SetFloat(Speed, speed); // Use the speed to control the animator
        }
        
        private void HandleJump()
        {
            // ReSharper disable once Unity.PreferNonAllocApi
            _isGrounded = Physics.OverlapSphere(transform.position, .1f, _groundMask).Length != 0;
            if (_jumpDebounce <= 0 && (_playerInputHandler.IsJumping || Input.mouseScrollDelta.y < 0))
            {
                if (_isGrounded || _justJumped)
                {
                    _rb.AddForce(transform.up * (_justJumped ? _jumpForce * _bunnyHopBoost : _jumpForce), ForceMode.Impulse);
                    _justJumped = true;
                    _bunnyHopTimer = _bunnyHopWindow;
                }
                // Start the jump debounce timer if the player is on the ground
                if (_isGrounded)
                {
                    _jumpDebounce = _jumpDebounceTime;
                }
            }
            else if (_isGrounded)
            {
                _justJumped = false;
            }
        }

        #endregion
    }
}