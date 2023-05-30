using _GAME_.Scripts.Enums;

namespace _GAME_.Scripts.Behaviour_Tree
{
    public class BtInverterNode : BtNode
    {
        #region Private Variables

        private BtNode _childNode;

        #endregion

        #region Public Methods

        public BtInverterNode(BtNode childNode)
        {
            this._childNode = childNode;
        }
        
        public override BtNodeState Execute()
        {
            var result = _childNode.Execute();
            return result switch
            {
                BtNodeState.Success => BtNodeState.Failure,
                BtNodeState.Failure => BtNodeState.Success,
                _ => result
            };
        }

        #endregion
    }
}