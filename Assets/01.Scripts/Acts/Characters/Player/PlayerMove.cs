using Acts.Characters;
using Managements.Managers;

namespace Acts.Characters.Player
{
    public class PlayerMove : CharacterMove
    {
        public override void Awake()
        {
            
            base.Awake();
            InputManager.OnMovePress += Translate;
        }
    }
}