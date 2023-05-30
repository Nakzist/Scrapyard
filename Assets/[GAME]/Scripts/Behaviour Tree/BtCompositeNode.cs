using System.Collections.Generic;

namespace _GAME_.Scripts.Behaviour_Tree
{
    public abstract class BtCompositeNode : BtNode
    {
        #region Protected Variables

        protected List<BtNode> ChildNodes = new List<BtNode>();

        #endregion

        #region Public Methods

        public void AddChild(BtNode node)
        {
            ChildNodes.Add(node);
        }
        
        public void RemoveChild(BtNode node)
        {
            ChildNodes.Remove(node);
        }

        #endregion
    }
}