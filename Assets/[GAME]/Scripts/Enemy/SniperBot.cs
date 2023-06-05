using System.Collections;
using _GAME_.Scripts.Enums;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Interfaces;
using _GAME_.Scripts.Scriptable_Objects.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace _GAME_.Scripts.Enemy
{
    public class SniperBot : BaseEnemy
    {
        #region Private Variables
        
        private float _moveRange;
        private float _rotationSpeed;
        private float _delayBetweenShoots;
        private bool _canShoot = true;
        
        private LineRenderer _lineRenderer;
        private float _dodgeSpeedThreshold;
        private bool _isShooting = false;
        private bool _playerDodged = false;

        #endregion

        #region Monobehaviour Methods

        protected override void Start()
        {
            base.Start();

            GetDataFromScriptable();
            LineRendererSettings();
        }

        #endregion

        #region Override Methods

        protected override BtNodeState Node1()
        {
            if (!_canShoot)
                return BtNodeState.Failure;

            if (_isShooting)
                return BtNodeState.Failure;

            if (PlayerDodged())
            {
                _playerDodged = true;
                return BtNodeState.Failure;
            }
            
            StartCoroutine(ShootDelay());
            
            StartCoroutine(ShootLaser());

            return BtNodeState.Success;
        }

        protected override BtNodeState Node2()
        {
            var dest = FindLocationWithClearLineOfSight();
            MoveToDestination(dest);
            
            RotateTowardsTarget(PlayerTransform, _rotationSpeed);

            return BtNodeState.Success;
        }

        protected override BtNodeState SelectorNode()
        {
            return HasClearLineOfSight() ? BtNodeState.Success : BtNodeState.Failure;
        }

        #endregion

        #region Private Methods

        private void GetDataFromScriptable()
        {
            var sniperBotData = Resources.Load<SniperBotScriptableObject>(FolderPaths.SNIPER_BOT_DATA_PATH);
            
            Agent.speed = sniperBotData.Speed;
            Agent.angularSpeed = sniperBotData.AngularSpeed;
            Agent.acceleration = sniperBotData.Acceleration;

            MaxHealth = sniperBotData.MaxHealth;
            CurrentHealth = MaxHealth;
            _moveRange = sniperBotData.MoveRange;
            _rotationSpeed = sniperBotData.RotationSpeed;
            Damage = sniperBotData.Damage;
            AttackRange = sniperBotData.AttackRange;
            _delayBetweenShoots = sniperBotData.DelayBetweenShoots;
            _dodgeSpeedThreshold = sniperBotData.DodgeSpeedThreshold;
        }

        private bool HasClearLineOfSight()
        {
            if (PlayerTransform == null)
                return false;

            
            if (Physics.Raycast(transform.position, PlayerTransform.transform.position - transform.position, out var hit,
                    AttackRange))
            {
                if(hit.collider.CompareTag("Player"))
                    return true;
            }
            
            return false;
        }

        private Vector3 FindLocationWithClearLineOfSight()
        {
            const int maxIterations = 100; // Maximum number of iterations
            var iterations = 0; // Counter for iterations

            while (iterations < maxIterations)
            {
                var randomDirection = Random.insideUnitSphere * _moveRange;
                randomDirection += transform.position;

                NavMesh.SamplePosition(randomDirection, out var navHit, _moveRange, NavMesh.AllAreas);

                var destination = navHit.position;
                var direction = (PlayerTransform.position - destination).normalized; // Normalize the direction

                if (Physics.Raycast(destination, direction, out var hit, _moveRange))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        return destination;
                    }
                }

                iterations++; // Increment the iteration counter
            }

            // If no suitable location found, return the current position as a fallback
            return transform.position;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void ApplyDamageToPlayer()
        {
            if (PlayerTransform != null)
            {
                var direction = PlayerTransform.position - transform.position;
                if (Physics.Raycast(transform.position, direction, out var hit, AttackRange))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        var playerDamageable = hit.collider.GetComponent<IDamageable>();
                        if (playerDamageable != null)
                        {
                            playerDamageable.TakeDamage(Damage);
                        }
                    }
                }
            }
        }
        
        private IEnumerator ShootLaser()
        {
            _isShooting = true;
            _lineRenderer.enabled = true;
            
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, PlayerTransform.position);

            yield return new WaitForSeconds(0.1f);

            _lineRenderer.enabled = false;
            _isShooting = false;

            if (!_playerDodged)
            {
                ApplyDamageToPlayer();
            }
            else
            {
                _playerDodged = false;
            }
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private bool PlayerDodged()
        {
            if (PlayerTransform != null)
            {
                var direction = PlayerTransform.position - transform.position;
                if (Physics.Raycast(transform.position, direction, out var hit, AttackRange))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        var playerRigidbody = PlayerTransform.GetComponent<Rigidbody>();
                        if (playerRigidbody != null && playerRigidbody.velocity.magnitude > _dodgeSpeedThreshold)
                            return true;
                    }
                }
            }
            return false;
        }

        private IEnumerator ShootDelay()
        {
            _canShoot = false;
            yield return new WaitForSeconds(_delayBetweenShoots);
            _canShoot = true;
        }
        
        private void LineRendererSettings()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.enabled = false;

            _lineRenderer.startWidth = 0.1f;
            _lineRenderer.endWidth = 0.1f;
        }

        #endregion
    }
}