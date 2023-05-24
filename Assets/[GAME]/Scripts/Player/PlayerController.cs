using System.Collections;
using System.Collections.Generic;
using _GAME_.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace _GAME_.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        #region Serialized Variables

        [FormerlySerializedAs("_moveSpeed")] [SerializeField] private float moveSpeed;
        [FormerlySerializedAs("_sprintMoveSpeed")] [SerializeField] private float sprintMoveSpeed;
        [FormerlySerializedAs("_jumpForce")] [SerializeField] private float jumpForce;
        [FormerlySerializedAs("DelayBetweenShots")] [SerializeField] private float delayBetweenShots;
        [SerializeField] private float weaponDamage;
        [FormerlySerializedAs("_meleeDamage")] [SerializeField] private float meleeDamage;
        [SerializeField] private float range;
        [SerializeField] private float bulletSpeed;
        [FormerlySerializedAs("_groundChecker")] [SerializeField] private Transform groundChecker;
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private GameObject bullet;
        [FormerlySerializedAs("_groundMask")] [SerializeField] private LayerMask groundMask;
        [FormerlySerializedAs("hittables")] [SerializeField] private LayerMask hittableMask;
        

        #endregion

        #region Private Variables

        private float _maxAmmo;
        private float _lastTimeShot;
    
        private bool _canMeleeAttack = true;
        private bool _isGrounded = true;
    
        private float _cameraVerticalAngle;

        private Camera _mainCamera;
        private Rigidbody _rb;

        #endregion

        #region Monobehavious Methods

        private void Start()
        {
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
            HandleShooting();
            HandleMeleeAttack();
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

        private void MovePlayer()
        {
            var t = transform;
            var playerMovement = t.forward * PlayerInputHandler.Instance.VerticalMovement +
                                 t.right * PlayerInputHandler.Instance.HorizontalMovement;
            playerMovement *= PlayerInputHandler.Instance.IsSprinting ? sprintMoveSpeed : moveSpeed;
            playerMovement.y += _rb.velocity.y;
            _rb.velocity = playerMovement;
        }
        
        private void HandleJump()
        {
            // ReSharper disable once Unity.PreferNonAllocApi
            _isGrounded = Physics.OverlapSphere(transform.position, .1f, groundMask).Length != 0;
            if (_isGrounded && PlayerInputHandler.Instance.IsJumping)
            {
                Debug.Log("jump");
                _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }
        }
        
        private void HandleShooting()
        {
            if (PlayerInputHandler.Instance.IsFiring)
            {
                if (_lastTimeShot + delayBetweenShots < Time.time)
                {
                    HandleShoot();
                }
            }
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private void HandleShoot()
        {
            var currentBullet = Instantiate(this.bullet, bulletSpawnPoint.position, Quaternion.identity);
            _lastTimeShot = Time.time;
            if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out var hit, range, hittableMask))
            {
                if (hit.transform.TryGetComponent(out IDamageable damageable))
                {
                    Debug.Log(hit.transform.name);
                    damageable.TakeDamage(weaponDamage);
                }

                currentBullet.GetComponent<Rigidbody>().velocity = (hit.point - transform.position).normalized * bulletSpeed;
            }
        }
        
        private void HandleMeleeAttack()
        {
            if (PlayerInputHandler.Instance.IsMeleeAttacking && _canMeleeAttack)
            {
                StartCoroutine(MeleeAttack());
            }
        }
        
        private IEnumerator MeleeAttack()
        {
            Debug.Log("Melee Attacking");
            _canMeleeAttack = false;
            foreach (var enemy in InAttackRange.EnemiesInAttackRange)
            {
                if (enemy.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(meleeDamage);
                }
            }
            InAttackRange.EnemiesInAttackRange = new List<GameObject>();
            yield return new WaitForSeconds(4f);
            _canMeleeAttack = true;
        }

        #endregion
    }
}