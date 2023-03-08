using Units.Behaviours.Unit;
using Units.Base.Default;
using Units.Behaviours.Character;
using UnityEngine;

namespace Units.Base.Character
{
    public class CharacterBase : Unit
    {
        [SerializeField]
        private PlayerAnimation playerAnimation;

        protected override void Init()
        {
            AddBehaviour<UnitRender>();
            AddBehaviour<CharacterState>();
            AddBehaviour<CharacterMove>();
            playerAnimation = AddBehaviour(playerAnimation);
        }
    }
}