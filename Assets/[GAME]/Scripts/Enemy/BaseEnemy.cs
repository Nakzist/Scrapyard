using System.Collections;
using _GAME_.Scripts.Enums;
using _GAME_.Scripts.Interfaces;
using _GAME_.Scripts.Managers;
using _GAME_.Scripts.Scriptable_Objects.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace _GAME_.Scripts.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BaseEnemy : MonoBehaviour, IDamageable
    {
        #region Private Variables

        private protected NavMeshAgent Agent;

        [Header("Enemy Stats")]
        private float _currentHealth;
        private float _maxHealth;
        private protected float AttackRange;
        private protected float Damage;
        private protected float RotationSpeed;

        private protected Transform PlayerTransform;
        
        private Coroutine _behaviorCoroutine;

        #endregion

        #region Monobehaviour Methods
        
        protected virtual void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            
            StartCoroutine(GetPlayer());
        }

        protected virtual void Update()
        {
            if (SelectorNode())
            {
                Node1();
            }
            else
            {
                Node2();
            }
        }

        #endregion

        #region Public Methods

        public void TakeDamage(float incomingDamage)
        {
            _currentHealth -= incomingDamage;
            
            if(_currentHealth <= 0)
                Destroy(gameObject);
        }

        #endregion
        
        #region Virtual Methods

        protected virtual void Node1()
        {
            // Node 1
        }

        protected virtual void Node2()
        {
            // Node 2
        }

        protected virtual bool SelectorNode()
        {
            // If true execute node1 else node2
            return (Random.value > 0.5f);
        }
        
        #endregion

        #region Protected Methods

        private protected void MoveToDestination(Vector3 dest)
        {
            Agent.destination = dest;
        }
        
        private protected void RotateTowardsTarget(Transform target, float rotationSpeed)
        {
            var dir = target.position - transform.position;
            var targetRot = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        private protected void GetBaseVariables(BaseEnemyScriptableObject data)
        {
            Agent.speed = data.Speed;
            Agent.angularSpeed = data.AngularSpeed;
            Agent.acceleration = data.Acceleration;

            _maxHealth = data.MaxHealth;
            _currentHealth = _maxHealth;
            
            AttackRange = data.AttackRange;
            Damage = data.Damage;

            RotationSpeed = data.RotationSpeed;
        }

        #endregion
        
        #region Private Methods
        
        private IEnumerator GetPlayer()
        {
            while (true)
            {
                if (GameManager.Instance.currentPlayer != null)
                {
                    PlayerTransform = GameManager.Instance.currentPlayer.transform;
                    break;
                }

                yield return null;
            }
        }

        #endregion
    }
}