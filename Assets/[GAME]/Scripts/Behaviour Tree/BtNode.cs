using _GAME_.Scripts.Enums;

namespace _GAME_.Scripts.Behaviour_Tree
{
    public abstract class BtNode
    {
        public BtNodeState CurrentState { get; protected set; }
        
        public abstract BtNodeState Execute();

        public virtual void Reset()
        {
            CurrentState = BtNodeState.Running;
        }
    }
}