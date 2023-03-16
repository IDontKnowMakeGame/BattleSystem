using Acts.Characters.Player;
using UnityEngine;

namespace Actors.Characters.Player
{
    public class PlayerActor : CharacterActor
    {
        [SerializeField] private PlayerAnimation _plyerAnimation;
        [SerializeField] private PlayerAttack _playerAttack;

        private bool isPlaying = false;
        public bool IsPlaying
        {
            get => isPlaying;
            set => isPlaying = value;
        }

        protected override void Init()
        {
            base.Init();
            AddAct<PlayerMove>();
            AddAct(_playerAttack);
            AddAct(_plyerAnimation);
        }
    }
}