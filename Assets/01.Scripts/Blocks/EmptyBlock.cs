namespace Blocks
{
    public class EmptyBlock : Block
    {
        protected bool isParent = false;
        protected override void Update()
        {
            base.Update();
            if (isParent)
                return;
            if(ActorOnBlock == null) return;
            if (ActorOnBlock.Position == Position)
            {
                var stat = ActorOnBlock.GetAct<CharacterStatAct>();
                stat?.Damage(int.MaxValue, this);
            }
        }
    }
}