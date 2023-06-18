using System.Collections;
using _GAME_.Scripts.Enums;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Interfaces;
using _GAME_.Scripts.Models;
using _GAME_.Scripts.Observer;
using _GAME_.Scripts.Scriptable_Objects.Player.Weapon.Ranged_Weapons;
using UnityEngine;

namespace _GAME_.Scripts.Player
{
    [RequireComponent(typeof(PlayerMovementController))]
    public class PlayerWeaponController : ObserverBase
    {
        #region Serialized Variables
        
        [SerializeField] private bool drawAttackRange;
        [SerializeField] private Vector3 attackRange;

        [SerializeField] private bool addWeapon;
        [SerializeField] private BaseRangedWeaponScriptableObject weaponToAdd;

        [SerializeField] private Transform leftHandSocket;
        [SerializeField] private Transform rightHandSocket;

        #endregion

        #region Private Variables

        #region Ranged Weapon Variables

        private GameObject _weaponGameObject;
        private float _currentAmmo;
        private RangedWeapon _currentWeapon;
        private Transform _bulletSpawnPoint;

        private float _lastTimeShot;
        private bool _isReloading;

        #endregion

        #region Melee Variables

        private float _meleeDamage;
        
        private bool _canMeleeAttack = true;

        #endregion

        #region Other Variables

        private Camera _mainCamera;
        
        private PlayerInputHandler _playerInputHandler;
        
        private Transform AttackRangeTransform => transform.GetChild(2);
        
        private Animator _animator;
        private AnimatorOverrideController _animatorOverrideController;

        #endregion

        #endregion

        #region Private Static Variables
        
        private static readonly int ShootSpeedMultiplier = Animator.StringToHash("ShootSpeedMultiplier");
        private static readonly int ShootingTrigger = Animator.StringToHash("ShootingTrigger");
        private static readonly int ReloadTrigger = Animator.StringToHash("ReloadTrigger");
        private static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
        private static readonly int ReloadSpeedMultiplier = Animator.StringToHash("ReloadSpeedMultiplier");

        #endregion

        #region Monobehaviour Methods

        private void Start()
        {
            _mainCamera = Camera.main;
            _playerInputHandler = GetComponent<PlayerInputHandler>();

            _animator = GetComponent<Animator>();
            _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            _animator.runtimeAnimatorController = _animatorOverrideController;
            
        }

        private void Update()
        {
            if (_currentWeapon != null)
            {
                HandleShooting();
                HandleReload();
            }
            HandleMeleeAttack();

            if (addWeapon)
            {
                addWeapon = false;
                ChangeWeapon(weaponToAdd);
            }
        }

        private void OnDrawGizmos()
        {
            if (drawAttackRange)
            {
                Gizmos.color = Color.green;
                Gizmos.matrix = Matrix4x4.TRS(AttackRangeTransform.position, AttackRangeTransform.rotation, Vector3.one);
                Gizmos.DrawWireCube(Vector3.zero, attackRange); // Draw at the origin because we have already included the position in the Gizmos.matrix
                Gizmos.matrix = Matrix4x4.identity; // Reset the Gizmos.matrix to not affect other Gizmos
            }
        }

        #endregion

        #region Private Methods

        private void ChangeWeapon(BaseRangedWeaponScriptableObject weaponScriptableObject)
        {
            _weaponGameObject = Instantiate(weaponScriptableObject.Weapon.weaponPrefab, rightHandSocket);

            _bulletSpawnPoint = _weaponGameObject.transform.GetChild(0);
            
            _currentWeapon = weaponScriptableObject.Weapon;
            _weaponGameObject.transform.localPosition = _currentWeapon.weaponPositionOffset;
            _weaponGameObject.transform.localRotation = Quaternion.Euler(_currentWeapon.weaponRotationOffset);
            _weaponGameObject.transform.localScale = _currentWeapon.weaponScale;
            
            _animatorOverrideController["RightShootEmpty"] = _currentWeapon.shootAnimationClip;
            _animatorOverrideController["RightReloadEmpty"] = _currentWeapon.reloadAnimationClip;

            var shootLength = _currentWeapon.shootAnimationClip.length;
            var desiredShootLength = _currentWeapon.delayBetweenShoots;
            var shootSpeedMultiplier = shootLength / desiredShootLength;
            
            var reloadLength = _currentWeapon.reloadAnimationClip.length;
            var desiredReloadLength = _currentWeapon.reloadTime;
            var reloadSpeedMultiplier = reloadLength / desiredReloadLength;

            _weaponGameObject.GetComponent<Animation>()["reload"].speed = reloadSpeedMultiplier;

            _animator.SetFloat(ShootSpeedMultiplier, shootSpeedMultiplier);
            _animator.SetFloat(ReloadSpeedMultiplier, reloadSpeedMultiplier);

            _currentAmmo = _currentWeapon.maxAmmo;
            
            Push(CustomEvents.OnWeaponChanged);
        }

        #region Shooting

        private void HandleShooting()
        {
            if (_playerInputHandler.IsFiring && !_isReloading && _currentAmmo > 0)
            {
                if (_lastTimeShot + _currentWeapon.delayBetweenShoots < Time.time)
                {
                    HandleShoot();
                }
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void HandleShoot()
        {
            var currentBullet = Instantiate(_currentWeapon.bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
            _lastTimeShot = Time.time;
            _animator.SetTrigger(ShootingTrigger);
            _currentAmmo--;
            Push(CustomEvents.OnBulletChange);

            var shootingDirection = _mainCamera.transform.forward;
            
            if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out var hit,
                    _currentWeapon.weaponRange, _currentWeapon.hittableLayerMask))
            {
                if (hit.transform.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(_currentWeapon.weaponDamage, DamageType.Ranged, DamageCauser.Player);
                }
                
                shootingDirection = (hit.point - _bulletSpawnPoint.position).normalized;
            }
            
            currentBullet.GetComponent<Rigidbody>().velocity = shootingDirection * _currentWeapon.bulletSpeed;
        }

        #endregion

        #region Reloading

        private void HandleReload()
        {
            if (_playerInputHandler.IsReloading)
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_currentAmmo == _currentWeapon.maxAmmo || _isReloading) return;
                
                StartCoroutine(ReloadWeapon());
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator ReloadWeapon()
        {
            _weaponGameObject.GetComponent<Animation>().Play();
            _animator.SetTrigger(ReloadTrigger);
            _isReloading = true;
            yield return new WaitForSeconds(_currentWeapon.reloadTime);
            _currentAmmo = _currentWeapon.maxAmmo;
            _isReloading = false;
            Push(CustomEvents.OnBulletChange);
        }

        #endregion

        #region Melee Attack

        private void HandleMeleeAttack()
        {
            if (_playerInputHandler.IsMeleeAttacking && _canMeleeAttack)
            {
                StartCoroutine(MeleeAttack());
            }
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator MeleeAttack()
        {
            _canMeleeAttack = false;
            // ReSharper disable once Unity.PreferNonAllocApi
            var colliders = Physics.OverlapBox(AttackRangeTransform.position, attackRange, AttackRangeTransform.rotation, _currentWeapon.hittableLayerMask,
                QueryTriggerInteraction.Collide);

            foreach (var enemy in colliders)
            {
                if (enemy.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(_meleeDamage, DamageType.Melee, DamageCauser.Player);
                }
            }
            yield return new WaitForSeconds(4f);
            _canMeleeAttack = true;
        }

        #endregion

        #endregion

        #region Public Methods

        public string GetCurrentAmmoText()
        {
            return $"{_currentAmmo} / {_currentWeapon.maxAmmo}";
        }

        #endregion
    }
}