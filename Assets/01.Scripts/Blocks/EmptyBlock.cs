namespace Blocks
{
    public class EmptyBlock : Block
    {
        protected override void Update()
        {
            base.Update();
            if(ActorOnBlock == null) return;
            if (ActorOnBlock.Position == Position)
            {
                var stat = ActorOnBlock.GetAct<CharacterStatAct>();
                stat?.Damage(int.MaxValue, this);
            }
        }
    }
}