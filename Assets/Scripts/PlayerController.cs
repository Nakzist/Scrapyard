using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _sprintMoveSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float DelayBetweenShots;
    [SerializeField] private float weaponDamage;
    [SerializeField] private float _meleeDamage;
    [SerializeField] private float range;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask hittables;
    private float m_MaxAmmo;
    private float m_LastTimeShot;
    
    private bool _canMeleeAttack = true;
    private bool _isGrounded = true;
    
    private float m_CameraVerticalAngle;

    private void Awake()
    {
        TryGetComponent(out _rigidbody);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Camera.main.transform.rotation = transform.rotation;
    }

    void Update()
    {
        Vector3 movement = transform.forward * PlayerInputHandler.instance.GetVerticalMovement() +
                           transform.right * PlayerInputHandler.instance.GetHorizontalMovement();
        movement *= PlayerInputHandler.instance.GetSprintInputHeld() ? _sprintMoveSpeed : _moveSpeed;
        movement.y += _rigidbody.velocity.y;
        _rigidbody.velocity = movement;
        _isGrounded = Physics.OverlapSphere(transform.position, .1f, _groundMask). Length != 0;
        if (_isGrounded && PlayerInputHandler.instance.GetJumpInputDown())
        {
            Debug.Log("jump");
            _rigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
        }

        float horizontalLookInput = (PlayerInputHandler.instance.GetHorizontalLookInput() *
                                     PlayerInputHandler.instance._horizontalsensitivity);

        transform.Rotate(
            new Vector3(0f, horizontalLookInput,
                0f), Space.Self);

        if (PlayerInputHandler.instance.GetFireInput())
        {
            TryShoot();
        }

        if (PlayerInputHandler.instance.GetMeleeAttackInput() && _canMeleeAttack)
        {
            StartCoroutine(Attack());
        }
    }

    private void LateUpdate()
    {
        m_CameraVerticalAngle -= PlayerInputHandler.instance.GetVerticalLookInput() *
                                 PlayerInputHandler.instance._verticalsensitivity;
        m_CameraVerticalAngle = Mathf.Clamp(m_CameraVerticalAngle, -89f, 89f);
        Camera.main.transform.eulerAngles = new Vector3(m_CameraVerticalAngle, Camera.main.transform.eulerAngles.y, 0);
    }
    
    void TryShoot()
    {
        if ( m_LastTimeShot + DelayBetweenShots < Time.time)
        {
            HandleShoot();
        }
    }

    void HandleShoot()
    {
        GameObject bullet = Instantiate(this.bullet, bulletSpawnPoint.position, Quaternion.identity);
        m_LastTimeShot = Time.time;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, range, hittables))
        {
            if (hit.transform.TryGetComponent(out IDamagable damagable))
            {
                Debug.Log(hit.transform.name);
                damagable.TakeDamage(weaponDamage);
            }

            bullet.GetComponent<Rigidbody>().velocity = (hit.point - transform.position).normalized * bulletSpeed;
        }
    }

    IEnumerator Attack()
    {
        Debug.Log("attacking");
        _canMeleeAttack = false;
        foreach (var Enemy in InAttackRange.EnemiesInAttackRange)
        {
            if (Enemy.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(_meleeDamage);
            }
        }
        InAttackRange.EnemiesInAttackRange = new List<GameObject>();
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(3f);
        _canMeleeAttack = true;
    }
}