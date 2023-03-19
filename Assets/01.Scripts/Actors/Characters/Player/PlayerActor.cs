using Acts.Characters.Player;
using UnityEngine;

namespace Actors.Characters.Player
{
    public class PlayerActor : CharacterActor
    {
        [SerializeField] private PlayerEquipment _playerEquipment;
        [SerializeField] private PlayerAnimation _plyerAnimation;
        [SerializeField] private PlayerAttack _playerAttack;
        [SerializeField] private PlayerUseAbleItem _playerUseAbleItem;

        protected override void Init()
        {
			AddAct(_playerEquipment);
            base.Init();
            AddAct<PlayerMove>();
            AddAct(_playerAttack);
            AddAct(_plyerAnimation);
            AddAct(_playerUseAbleItem);
		}
    }
}