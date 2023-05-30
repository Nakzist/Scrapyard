using _GAME_.Scripts.Enums;

namespace _GAME_.Scripts.Behaviour_Tree
{
    public class BtSequenceNode : BtCompositeNode
    {
        #region Public Methods

        public override BtNodeState Execute()
        {
            foreach (var node in ChildNodes)
            {
                var result = node.Execute();
                if (result != BtNodeState.Success)
                {
                    return result;
                }
            }
            return BtNodeState.Success;
        }

        #endregion
    }
}