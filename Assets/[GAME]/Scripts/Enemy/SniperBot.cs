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
        private bool _walkingToLocation = false;

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

        protected override void Node1()
        {
            Debug.Log("Node1");
            if (!_canShoot)
                return;

            if (_isShooting)
                return;

            if (PlayerDodged())
            {
                _playerDodged = true;
                return;
            }
            
            StartCoroutine(ShootDelay());
            StartCoroutine(ShootLaser());
        }

        protected override void Node2()
        {
            Debug.Log("Node2");
            if(PlayerTransform == null)
                return;

            StartCoroutine(WaitForDestination());
            
            RotateTowardsTarget(PlayerTransform, _rotationSpeed);
        }

        protected override bool SelectorNode()
        {
            return HasClearLineOfSight();
        }

        #endregion

        #region Private Methods
        
        
        private IEnumerator WaitForDestination()
        {
            if (_walkingToLocation) yield break;
            
            _walkingToLocation = true;
            
            var dest = FindLocationWithClearLineOfSight();
            
            MoveToDestination(dest);

            while (Agent.pathPending || Agent.remainingDistance > Agent.stoppingDistance)
            {
                yield return null;
            }

            _walkingToLocation = false;

        }

        private void GetDataFromScriptable()
        {
            var sniperBotData = Resources.Load<SniperBotScriptableObject>(FolderPaths.SNIPER_BOT_DATA_PATH);
            
            GetBaseVariables(sniperBotData);
            
            _moveRange = sniperBotData.MoveRange;
            _rotationSpeed = sniperBotData.RotationSpeed;
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