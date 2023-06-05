using System;
using System.Collections;
using System.Collections.Generic;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Interfaces;
using _GAME_.Scripts.Scriptable_Objects.Player;
using UnityEngine;

namespace _GAME_.Scripts.Player
{
    [RequireComponent(typeof(PlayerMovementController))]
    public class PlayerWeaponController : MonoBehaviour
    {
        #region Serialized Variables
        
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private bool drawAttackRange;
        [SerializeField] private Vector3 attackRange;

        #endregion

        #region Private Variables
        
        private float _delayBetweenShots;
        private float _weaponDamage;
        private float _meleeDamage;
        private float _weaponRange;
        private float _bulletSpeed;
        private GameObject _bulletPrefab;
        private float _maxAmmo;
        private LayerMask _hittableMask;
        
        private float _lastTimeShot;
    
        private bool _canMeleeAttack = true;

        private PlayerWeaponDataScriptableObject _playerWeaponData;
        
        private Camera _mainCamera;
        
        private PlayerInputHandler _playerInputHandler;
        
        private Transform AttackRangeTransform => transform.GetChild(2);

        #endregion

        #region Monobehaviour Methods

        private void Start()
        {
            GetDataFromScriptable();
            
            _mainCamera = Camera.main;
            _playerInputHandler = GetComponent<PlayerInputHandler>();
        }

        private void Update()
        {
            HandleShooting();
            HandleMeleeAttack();
        }

        private void OnDrawGizmos()
        {
            if (drawAttackRange)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(AttackRangeTransform.position, attackRange);
            }
        }

        #endregion

        #region Private Methods

        private void GetDataFromScriptable()
        {
            _playerWeaponData = Resources.Load<PlayerWeaponDataScriptableObject>(FolderPaths.WEAPON_DATA_PATH);
            
            _delayBetweenShots = _playerWeaponData.DelayBetweenShoots;
            _weaponDamage = _playerWeaponData.WeaponDamage;
            _meleeDamage = _playerWeaponData.MeleeDamage;
            _weaponRange = _playerWeaponData.WeaponRange;
            _bulletSpeed = _playerWeaponData.BulletSpeed;
            _bulletPrefab = _playerWeaponData.BulletPrefab;
            _maxAmmo = _playerWeaponData.MaxAmmo;
            _hittableMask = _playerWeaponData.HittableLayerMask;
        }
        
        private void HandleShooting()
        {
            if (_playerInputHandler.IsFiring)
            {
                if (_lastTimeShot + _delayBetweenShots < Time.time)
                {
                    HandleShoot();
                }
            }
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private void HandleShoot()
        {
            var currentBullet = Instantiate(this._bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            _lastTimeShot = Time.time;
            
            // Calculate the shooting direction
            var shootingDirection = _mainCamera.transform.forward;
            
            if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out var hit,
                    _weaponRange, _hittableMask))
            {
                if (hit.transform.TryGetComponent(out IDamageable damageable))
                {
                    Debug.Log(hit.transform.name);
                    damageable.TakeDamage(_weaponDamage);
                }
                
                // Update the shooting direction based on the hit point
                shootingDirection = (hit.point - bulletSpawnPoint.position).normalized;
            }
            
            // Apply velocity to the bullet in the shooting direction
            currentBullet.GetComponent<Rigidbody>().velocity = shootingDirection * _bulletSpeed;
        }
                
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
            Debug.Log("Melee Attacking");
            _canMeleeAttack = false;
            // ReSharper disable once Unity.PreferNonAllocApi
            var colliders = Physics.OverlapBox(AttackRangeTransform.position, attackRange, Quaternion.identity, _hittableMask,
                QueryTriggerInteraction.Collide);

            foreach (var enemy in colliders)
            {
                if (enemy.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(_meleeDamage);
                }
            }
            yield return new WaitForSeconds(4f);
            _canMeleeAttack = true;
        }

        #endregion
    }
}