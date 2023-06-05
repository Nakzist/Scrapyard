using System.Collections;
using _GAME_.Scripts.Behaviour_Tree;
using _GAME_.Scripts.Enums;
using _GAME_.Scripts.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace _GAME_.Scripts.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BaseEnemy : MonoBehaviour
    {
        #region Private Variables

        private BtNode _behaviourTreeRoot;

        private protected NavMeshAgent Agent;

        [Header("Enemy Stats")]
        private protected float CurrentHealth;
        private protected float MaxHealth;
        private protected float AttackRange;
        private protected float Damage;

        private protected Transform PlayerTransform;

        #endregion

        #region Monobehaviour Methods
        
        protected virtual void Start()
        {
            _behaviourTreeRoot = CreateBehaviourTree();

            Agent = GetComponent<NavMeshAgent>();
            StartCoroutine(GetPlayer());
        }

        protected virtual void Update()
        {
            _behaviourTreeRoot?.Execute();
            
            if (_behaviourTreeRoot?.CurrentState is BtNodeState.Success or BtNodeState.Failure)
            {
                ResetBehaviourTree();
            }
        }

        #endregion

        #region Public Methods

        public void ModifyHealth(float amount)
        {
            CurrentHealth += amount;

            if (CurrentHealth <= 0)
            {
                // Enemy Death
            }
        }

        #endregion
        
        #region Virtual Methods

        protected virtual BtNodeState Node1()
        {
            // Node 1
            return BtNodeState.Success;
        }

        protected virtual BtNodeState Node2()
        {
            // Node 2
            return BtNodeState.Success;
        }

        protected virtual BtNodeState SelectorNode()
        {
            // If true execute node1 else node2
            var isTrue = (Random.value > 0.5f);
            return isTrue ? BtNodeState.Success : BtNodeState.Failure;
        }
        
        #endregion

        #region Private Methods

        private BtNode CreateBehaviourTree()
        {
            BtSelectorNode rootSelectorNode = new();

            BtSequenceNode detectPlayerSequence = new();
            detectPlayerSequence.AddChild(new BtLeafNode(SelectorNode));

            BtSequenceNode actionSequence = new();
            actionSequence.AddChild(new BtLeafNode(Node1));
            actionSequence.AddChild(new BtLeafNode(Node2));

            BtInverterNode inverterNode = new(actionSequence);

            detectPlayerSequence.AddChild(inverterNode);

            rootSelectorNode.AddChild(detectPlayerSequence);

            return rootSelectorNode;
        }

        private void ResetBehaviourTree()
        {
            _behaviourTreeRoot.Reset();
        }
        
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