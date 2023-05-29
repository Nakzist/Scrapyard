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

        #endregion

        #region Private Variables

        [Header("Weapon")]
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

        #endregion

        #region Monobehaviour Methods

        private void Start()
        {
            GetDataFromScriptable();
            
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            HandleShooting();
            HandleMeleeAttack();
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
            if (PlayerInputHandler.Instance.IsFiring)
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
            if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out var hit,
                    _weaponRange, _hittableMask))
            {
                if (hit.transform.TryGetComponent(out IDamageable damageable))
                {
                    Debug.Log(hit.transform.name);
                    damageable.TakeDamage(_weaponDamage);
                }

                currentBullet.GetComponent<Rigidbody>().velocity =
                    (hit.point - transform.position).normalized * _bulletSpeed;
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
                    damageable.TakeDamage(_meleeDamage);
                }
            }
            InAttackRange.EnemiesInAttackRange = new List<GameObject>();
            yield return new WaitForSeconds(4f);
            _canMeleeAttack = true;
        }

        #endregion
    }
}