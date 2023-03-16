using Acts.Characters.Player;
using UnityEngine;

namespace Actors.Characters.Player
{
    public class PlayerActor : CharacterActor
    {
        [SerializeField] private PlayerAnimation _plyerAnimation;
        protected override void Init()
        {
            base.Init();
            AddAct<PlayerMove>();
            AddAct(_plyerAnimation);
        }
    }
}