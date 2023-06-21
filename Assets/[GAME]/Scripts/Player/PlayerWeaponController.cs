using System.Collections;
using _GAME_.Scripts.Enums;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Interfaces;
using _GAME_.Scripts.Models;
using _GAME_.Scripts.Observer;
using _GAME_.Scripts.Scriptable_Objects.Player.Weapon.Close_Combat_Weapons;
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

        [SerializeField] private Transform leftHandSocket;
        [SerializeField] private Transform rightHandSocket;
        
        [SerializeField] private bool addRangeWeapon;
        [SerializeField] private BaseRangedWeaponScriptableObject rangeWeaponToAdd;
        
        [SerializeField] private bool addCloseRangeWeapon;
        [SerializeField] private BaseCloseCombatWeaponScriptableObject closeRangeWeaponToAdd;

        [SerializeField] private AudioClip shootSoundClip;

        #endregion

        #region Private Variables

        #region Ranged Weapon Variables

        private GameObject _rangedWeaponGameObject;
        private float _currentAmmo;
        private float _currentBulletCount;
        private RangedWeapon _currentRangedWeapon;
        private Transform _bulletSpawnPoint;

        private float _lastTimeShot;
        private bool _isReloading;

        private AudioSource _audioSource;

        #endregion

        #region Melee Variables

        private CloseRangeWeaponData _currentCloseRangeWeapon;
        private GameObject _closeRangeWeaponGameObject;
        private bool _meleeDamageBoost = false;
        private bool _canMeleeAttack = true;
        private float _lastMeleeAttackTime;
        private int _closeRangeEnemyKillCount;

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
            
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (_currentRangedWeapon != null)
            {
                HandleShooting();
                HandleReload();
            }

            if (_currentCloseRangeWeapon != null)
            {
                HandleMeleeAttack();
            }

            if (addRangeWeapon)
            {
                addRangeWeapon = false;
                ChangeRangedWeapon(rangeWeaponToAdd);
            }
            
            if(addCloseRangeWeapon)
            {
                addCloseRangeWeapon = false;
                ChangeCloseCombatWeapon(closeRangeWeaponToAdd);
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

        private void OnEnable()
        {
            Register(CustomEvents.OnEnemyDeathClose, OnEnemyDeathWithCloseCombat);
        }

        private void OnDisable()
        {
            Unregister(CustomEvents.OnEnemyDeathClose, OnEnemyDeathWithCloseCombat);
        }

        #endregion

        #region Private Methods

        // ReSharper disable Unity.PerformanceAnalysis
        private void ChangeRangedWeapon(BaseRangedWeaponScriptableObject weaponScriptableObject)
        {
            _rangedWeaponGameObject = Instantiate(weaponScriptableObject.Weapon.weaponPrefab, rightHandSocket);

            _bulletSpawnPoint = _rangedWeaponGameObject.transform.GetChild(0);
            
            _currentRangedWeapon = weaponScriptableObject.Weapon;
            _rangedWeaponGameObject.transform.localPosition = _currentRangedWeapon.weaponPositionOffset;
            _rangedWeaponGameObject.transform.localRotation = Quaternion.Euler(_currentRangedWeapon.weaponRotationOffset);
            _rangedWeaponGameObject.transform.localScale = _currentRangedWeapon.weaponScale;
            
            _animatorOverrideController["RightShootEmpty"] = _currentRangedWeapon.shootAnimationClip;
            _animatorOverrideController["RightReloadEmpty"] = _currentRangedWeapon.reloadAnimationClip;

            var shootLength = _currentRangedWeapon.shootAnimationClip.length;
            var desiredShootLength = _currentRangedWeapon.delayBetweenShoots;
            var shootSpeedMultiplier = shootLength / desiredShootLength;
            
            var reloadLength = _currentRangedWeapon.reloadAnimationClip.length;
            var desiredReloadLength = _currentRangedWeapon.reloadTime;
            var reloadSpeedMultiplier = reloadLength / desiredReloadLength;
            
            _rangedWeaponGameObject.GetComponent<Animation>()["reload"].speed = reloadSpeedMultiplier;

            _animator.SetFloat(ShootSpeedMultiplier, shootSpeedMultiplier);
            _animator.SetFloat(ReloadSpeedMultiplier, reloadSpeedMultiplier);

            _currentAmmo = _currentRangedWeapon.magSize;
            _currentBulletCount = _currentRangedWeapon.bulletCount;
            
            Push(CustomEvents.OnWeaponChanged);
        }

        private void ChangeCloseCombatWeapon(BaseCloseCombatWeaponScriptableObject weaponScriptableObject)
        {
            _closeRangeWeaponGameObject = Instantiate(weaponScriptableObject.Weapon.weaponPrefab, leftHandSocket);
            
            _currentCloseRangeWeapon = weaponScriptableObject.Weapon;
            _closeRangeWeaponGameObject.transform.localPosition = _currentCloseRangeWeapon.weaponPositionOffset;
            _closeRangeWeaponGameObject.transform.localRotation = Quaternion.Euler(_currentCloseRangeWeapon.weaponRotationOffset);
            _closeRangeWeaponGameObject.transform.localScale = _currentCloseRangeWeapon.weaponScale;
            
            _animatorOverrideController["LeftAttackEmpty"] = _currentCloseRangeWeapon.attackAnimationClip;
        }

        #region Shooting

        private void HandleShooting()
        {
            if (_playerInputHandler.IsFiring && !_isReloading && _currentAmmo > 0)
            {
                if (_lastTimeShot + _currentRangedWeapon.delayBetweenShoots < Time.time)
                {
                    HandleShoot();
                }
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void HandleShoot()
        {
            //var currentBullet = Instantiate(_currentRangedWeapon.bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
            _lastTimeShot = Time.time;
            _animator.SetTrigger(ShootingTrigger);
            _currentAmmo--;
            Push(CustomEvents.OnBulletChange);
            
            _audioSource.PlayOneShot(shootSoundClip);
            
            if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out var hit,
                    Mathf.Infinity, _currentRangedWeapon.hittableLayerMask))
            {
                if (_currentRangedWeapon.weapon == RangedWeaponsEnum.GrenadeLauncher)
                {
                    // ReSharper disable once Unity.PreferNonAllocApi
                    var colliders = Physics.OverlapSphere(hit.transform.position, 10f,
                        _currentRangedWeapon.hittableLayerMask);
                    
                    foreach (var col in colliders)
                    {
                        if (col.transform.TryGetComponent(out IDamageable damageable))
                        {
                            damageable.TakeDamage(_currentRangedWeapon.weaponDamage, DamageType.Ranged, DamageCauser.Player);
                        }
                    }
                }
                else
                {

                    if (hit.transform.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.TakeDamage(_currentRangedWeapon.weaponDamage, DamageType.Ranged, DamageCauser.Player);
                    }
                }

                
                //shootingDirection = (hit.point - _bulletSpawnPoint.position).normalized;
            }
            
            //currentBullet.GetComponent<Rigidbody>().velocity = shootingDirection * _currentRangedWeapon.bulletSpeed;
        }

        #endregion

        #region Reloading

        private void HandleReload()
        {
            if (_playerInputHandler.IsReloading)
            {
                if (_currentBulletCount <= 0) return;
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_currentAmmo == _currentRangedWeapon.magSize || _isReloading) return;
                
                StartCoroutine(ReloadWeapon());
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator ReloadWeapon()
        {
            _rangedWeaponGameObject.GetComponent<Animation>().Play();
            _animator.SetTrigger(ReloadTrigger);
            _isReloading = true;
            yield return new WaitForSeconds(_currentRangedWeapon.reloadTime);
            var bulletToAdd = _currentBulletCount - _currentRangedWeapon.magSize;
            var magAmmoCount = _currentAmmo;
            bulletToAdd = bulletToAdd < 0 ? _currentBulletCount : _currentRangedWeapon.magSize;
            _currentAmmo = bulletToAdd;
            _currentBulletCount -= (bulletToAdd - magAmmoCount);
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

            switch (_currentCloseRangeWeapon.weaponSkill)
            {
                case CloseRangeWeaponSkill.Cleaver:
                    CloseRangeWeaponSkills.CleaverSkillHandler();
                    break;
                case CloseRangeWeaponSkill.Kanabo:
                    CloseRangeWeaponSkills.KanaboSkillHandler();
                    break;
                case CloseRangeWeaponSkill.Katana:
                    CloseRangeWeaponSkills.KatanaSkillHandler();
                    break;
                case CloseRangeWeaponSkill.Spear:
                    CloseRangeWeaponSkills.SpearSkillHandler();
                    break;
                case CloseRangeWeaponSkill.None:
                    break;
                default:
                    break;
            }
            
            _lastMeleeAttackTime += Time.deltaTime;
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator MeleeAttack()
        {
            _canMeleeAttack = false;
            _lastMeleeAttackTime = 0;
            _animator.SetTrigger(AttackTrigger);
            // ReSharper disable once Unity.PreferNonAllocApi
            var colliders = Physics.OverlapBox(AttackRangeTransform.position, attackRange, AttackRangeTransform.rotation, _currentCloseRangeWeapon.hittableLayerMask,
                QueryTriggerInteraction.Collide);

            foreach (var enemy in colliders)
            {
                if (enemy.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(_currentCloseRangeWeapon.weaponDamage, DamageType.Melee, DamageCauser.Player);
                }
            }
            yield return new WaitForSeconds(4f);
            _canMeleeAttack = true;
        }

        private void OnEnemyDeathWithCloseCombat()
        {
            _closeRangeEnemyKillCount++;
        }

        #endregion

        #endregion

        #region Public Methods

        public string GetCurrentAmmoText()
        {
            return $"{_currentAmmo} / {_currentBulletCount}";
        }

        public void GiveMeleeDamageBoost()
        {
            _meleeDamageBoost = true;
        }

        public bool IsMeleeAttackBoosted()
        {
            return _meleeDamageBoost;
        }

        public float GetLastMeleeAttackTime()
        {
            return _lastMeleeAttackTime;
        }
        
        public int GetCloseRangeKillCount()
        {
            return _closeRangeEnemyKillCount;
        }
        
        public void ResetCloseRangeKillCount()
        {
            _closeRangeEnemyKillCount = 0;
        }

        public void IncreaseBullet(float value)
        {
            _currentBulletCount += value;
            Push(CustomEvents.OnBulletChange);
        }

        public void AddWeapon(BaseRangedWeaponScriptableObject ranged, BaseCloseCombatWeaponScriptableObject close)
        {
            ChangeRangedWeapon(ranged);
            ChangeCloseCombatWeapon(close);
        }
        
        #endregion
    }
}