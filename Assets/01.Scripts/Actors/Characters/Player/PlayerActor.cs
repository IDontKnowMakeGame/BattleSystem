using Acts.Characters.Player;

namespace Actors.Characters.Player
{
    public class PlayerActor : CharacterActor
    {
        protected override void Init()
        {
            base.Init();
            AddAct<PlayerMove>();
        }
    }
}