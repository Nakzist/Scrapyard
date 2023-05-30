using System;
using _GAME_.Scripts.Enums;

namespace _GAME_.Scripts.Behaviour_Tree
{
    public class BtLeafNode : BtNode
    {
        #region Private Variables

        private Func<BtNodeState> _action;

        #endregion

        #region Public Methods

        public BtLeafNode(Func<BtNodeState> action)
        {
            this._action = action;
        }

        public override BtNodeState Execute()
        {
            return _action.Invoke();
        }

        #endregion
    }
}