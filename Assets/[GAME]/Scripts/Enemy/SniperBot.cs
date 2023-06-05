using System.Collections;
using _GAME_.Scripts.Enums;
using _GAME_.Scripts.GlobalVariables;
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
        private bool _canShoot;

        #endregion

        #region Monobehaviour Methods

        protected override void Start()
        {
            base.Start();

            GetDataFromScriptable();
        }

        #endregion

        #region Override Methods

        protected override BtNodeState Node1()
        {
            if (!_canShoot)
                return BtNodeState.Failure;
            
            Debug.Log("Shooting");
            return BtNodeState.Success;
        }

        protected override BtNodeState Node2()
        {
            var dest = FindLocationWithClearLineOfSight();
            MoveToDestination(dest);
            
            RotateTowardsPlayer();

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
        }

        private bool HasClearLineOfSight()
        {
            if (PlayerTransform == null)
            {
                Debug.Log("Player is null");
                return false;
            }

            
            if (Physics.Raycast(transform.position, PlayerTransform.transform.position - transform.position, out var hit,
                    AttackRange))
            {
                if(hit.collider.CompareTag("Player"))
                    return true;
                else
                {
                    Debug.Log("Something else than player hit");
                }
            }

            Debug.Log("Player not in range");
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

        private void MoveToDestination(Vector3 dest)
        {
            Agent.destination = dest;
        }

        private void RotateTowardsPlayer()
        {
            var dir = PlayerTransform.position - transform.position;
            var targetRot = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _rotationSpeed * Time.deltaTime);
        }
        
        private IEnumerator ShootDelay()
        {
            _canShoot = false;
            yield return new WaitForSeconds(_delayBetweenShoots);
            _canShoot = true;
        }

        #endregion
    }
}