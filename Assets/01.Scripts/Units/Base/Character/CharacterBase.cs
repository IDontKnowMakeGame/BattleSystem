using Units.Behaviours.Unit;
using Units.Base.Default;
using Units.Behaviours.Character;

namespace Units.Base.Character
{
    public class CharacterBase : Unit
    {
        protected override void Init()
        {
            AddBehaviour<UnitRender>();
            AddBehaviour<CharacterState>();
            AddBehaviour<CharacterMove>();
        }
    }
}