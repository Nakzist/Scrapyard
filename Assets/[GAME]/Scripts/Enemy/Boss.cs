namespace _GAME_.Scripts.Enemy
{
    public class Boss : BaseEnemy
    {
        #region Override Methods

        protected override void Node1()
        {
            //attack
        }

        protected override void Node2()
        {
            //rest
        }

        protected override bool SelectorNode()
        {
            //check if on rest false for rest
            return false;
        }

        #endregion
    }
}