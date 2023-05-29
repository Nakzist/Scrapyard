using _GAME_.Scripts.Enums;

namespace _GAME_.Scripts.Behaviour_Tree
{
    public class BtSelectorNode : BtCompositeNode
    {
        #region Public Methods

        public override BtNodeState Execute()
        {
            foreach (var node in ChildNodes)
            {
                var result = node.Execute();
                if (result is BtNodeState.Success or BtNodeState.Running)
                {
                    return result;
                }
            }
            return BtNodeState.Failure;
        }

        #endregion
    }
}