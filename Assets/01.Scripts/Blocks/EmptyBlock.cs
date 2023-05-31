using Actors.Characters;

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
                if (ActorOnBlock is CharacterActor)
                {
                    var characterOnBlock = ActorOnBlock as CharacterActor;
                    if (characterOnBlock.HasState(CharacterState.Die))
                        return;
                }
                var stat = ActorOnBlock.GetAct<CharacterStatAct>();
                stat?.Damage(int.MaxValue, this);
            }
        }
    }
}