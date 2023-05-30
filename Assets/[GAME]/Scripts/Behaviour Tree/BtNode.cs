using _GAME_.Scripts.Enums;

namespace _GAME_.Scripts.Behaviour_Tree
{
    public abstract class BtNode
    {
        public abstract BtNodeState Execute();
    }
}