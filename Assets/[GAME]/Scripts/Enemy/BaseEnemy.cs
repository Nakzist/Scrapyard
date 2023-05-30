using _GAME_.Scripts.Behaviour_Tree;
using _GAME_.Scripts.Enums;
using UnityEngine;

namespace _GAME_.Scripts.Enemy
{
    public class BaseEnemy : MonoBehaviour
    {
        #region Private Variables

        private BtNode _behaviourTreeRoot;

        #endregion

        #region Virtual Methods

        protected virtual void Start()
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

            _behaviourTreeRoot = rootSelectorNode;
        }

        protected virtual void Update()
        {
            _behaviourTreeRoot.Execute();
        }

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
    }
}