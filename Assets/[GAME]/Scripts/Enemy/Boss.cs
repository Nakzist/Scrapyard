using _GAME_.Scripts.Enums;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Managers;
using _GAME_.Scripts.Scriptable_Objects.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace _GAME_.Scripts.Enemy
{
    public class Boss : BaseEnemy
    {
        #region Private Variables

        private bool _canReceiveDamage = false;
        private bool _isAttacked;
        private float _chargeTimer = 0f;
        private float _chargeDuration;
        private NavMeshAgent _agent;
        private bool _afterAttack;
        private Animator _anim;
        private static readonly int Rest = Animator.StringToHash("Rest");
        private static readonly int Charge = Animator.StringToHash("Charge");

        #endregion

        #region Override Methods

        protected override void Start()
        {
            base.Start();
            _agent = GetComponent<NavMeshAgent>();
            _anim = GetComponent<Animator>();
            
            GetDataFromScriptable();
        }

        
        protected override void Node1()
        {
            // Charge towards the player
            var playerPosition = GameManager.Instance.currentPlayer.transform.position;
            var path = new NavMeshPath();
            if (!_agent.CalculatePath(playerPosition, path))
            {
                var nearestPoint = GetNearestReachablePoint(playerPosition);
                MoveToDestination(nearestPoint);
            }
            else
            {
                MoveToDestination(playerPosition);
            }
            RotateTowardsTarget(GameManager.Instance.currentPlayer.transform, 10f);
            _isAttacked = true;
            _canReceiveDamage = false;
            _afterAttack = true;
            _chargeTimer = _chargeDuration;
            _anim.SetBool(Charge, true);
        }

        protected override void Node2()
        {
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance + 0.1f)
            {
                if (_afterAttack)
                {
                    _afterAttack = false;
                    if(Vector3.Distance(transform.position, GameManager.Instance.currentPlayer.transform.position) <= AttackRange)
                        GameManager.Instance.currentPlayer.PlayerHealthManager.TakeDamage(Damage, DamageType.Melee, DamageCauser.Enemy);
                    
                    _anim.SetBool(Charge, false);
                    _anim.SetBool(Rest, true);
                }
                
                _canReceiveDamage = true;
                
                // Create a timer for charging
                if (_chargeTimer > 0f)
                {
                    _chargeTimer -= Time.deltaTime;
                    if (_chargeTimer <= 0f)
                    {
                        _canReceiveDamage = false;
                        _isAttacked = false;
                        _anim.SetBool(Rest, false);
                    }
                }
            }
            else
            {
                RotateTowardsTarget(GameManager.Instance.currentPlayer.transform, 10f);
            }
        }

        protected override bool SelectorNode()
        {
            if(!_isAttacked)
                if(Vector3.Distance(transform.position, GameManager.Instance.currentPlayer.transform.position) <= AttackRange)
                    GameManager.Instance.currentPlayer.PlayerHealthManager.TakeDamage(Damage, DamageType.Melee, DamageCauser.Enemy);
            
            return !_isAttacked;
        }

        public override void TakeDamage(float incomingDamage, DamageType damageType, DamageCauser damageCauser)
        {
            if (!_canReceiveDamage) return;
            
            base.TakeDamage(incomingDamage, damageType, damageCauser);
        }

        #endregion

        #region Private Methods

        private void GetDataFromScriptable()
        {
            var bossData = Resources.Load<BossScriptableObject>(FolderPaths.BOSS_DATA_PATH);

            _chargeDuration = bossData.ChargeDuration;
            
            GetBaseVariables(bossData);
        }
        
        private Vector3 GetNearestReachablePoint(Vector3 targetPosition)
        {
            if (NavMesh.FindClosestEdge(targetPosition, out var hit, NavMesh.AllAreas))
            {
                // Found the nearest edge, return its position
                return hit.position;
            }

            // Failed to find a reachable position, return the target position as fallback
            return targetPosition;
        }

        #endregion
    }
}