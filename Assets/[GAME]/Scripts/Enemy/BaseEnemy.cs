using System.Collections;
using _GAME_.Scripts.Enums;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Interfaces;
using _GAME_.Scripts.Managers;
using _GAME_.Scripts.Observer;
using _GAME_.Scripts.Scriptable_Objects.Enemy;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _GAME_.Scripts.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BaseEnemy : ObserverBase, IDamageable
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
        private bool _deathByCloseCombat;

        #endregion

        #region Serialized Variables

        [Header("Interaction Settings")]
        [SerializeField] private bool drawGizmos = false;
        [SerializeField] private float gizmosRadius = 1f;

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

        private void OnDrawGizmos()
        {
            if(!drawGizmos)
                return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, gizmosRadius);
        }

        #endregion

        #region Public Methods

        public virtual void TakeDamage(float incomingDamage, DamageType damageType, DamageCauser damageCauser)
        {
            _currentHealth -= incomingDamage;

            if (_currentHealth <= 0)
            {
                EnemyDeath(damageCauser);
                if(damageType == DamageType.Melee)
                    _deathByCloseCombat = true;
            }
        }

        public IEnumerator StunEnemy(float duration)
        {
            var speed  = Agent.speed;
            Agent.speed = 0;
            yield return new WaitForSeconds(duration);
            // ReSharper disable once Unity.InefficientPropertyAccess
            Agent.speed = speed;
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
            var euler = targetRot.eulerAngles;
            var currentEuler = transform.rotation.eulerAngles;
            var finalVector = new Vector3(currentEuler.x, euler.y, currentEuler.z);
            targetRot = Quaternion.Euler(finalVector);
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

        private protected virtual void EnemyDeath(DamageCauser damageCauser)
        {
            GameManager.Instance.aliveEnemies.Remove(gameObject);
            
            if(damageCauser == DamageCauser.Player)
                Push(CustomEvents.OnEnemyDeath);
            
            if(_deathByCloseCombat)
                Push(CustomEvents.OnEnemyDeathClose);
            
            Destroy(gameObject);
        }

        #endregion
    }
}