using _GAME_.Scripts.Enums;
using UnityEngine;

namespace _GAME_.Scripts.Enemy
{
    public class MeleeEnemy : BaseEnemy
    {
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float movementSpeed = 2f; 
        
        private Transform _playerTransform; // Reference to the player's transform

        protected override void Start()
        {
            base.Start();
            // Get the player's transform reference
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        protected override BtNodeState Node1()
        {
            // Attack logic here
            // For example, play attack animation or deal damage to the player
            Debug.Log("Attacking!");
            return BtNodeState.Success;
        }

        protected override BtNodeState Node2()
        {
            // Move towards the player's location
            Vector3 direction = _playerTransform.position - transform.position;
            transform.Translate(direction.normalized * (Time.deltaTime * movementSpeed));
            Debug.Log("Moving towards player!");
            return BtNodeState.Success;
        }

        protected override BtNodeState SelectorNode()
        {
            // Check if the player is close enough to attack
            float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
            if (distanceToPlayer <= attackRange)
            {
                return BtNodeState.Success; // Player is close enough to attack
            }
            else
            {
                return BtNodeState.Failure; // Player is not close enough to attack
            }
        }
    }
}