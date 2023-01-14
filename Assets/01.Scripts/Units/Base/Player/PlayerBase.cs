using Units.Base.Unit;

namespace Units.Base.Player
{
    public class PlayerBase : UnitBase
    {
        protected override void Init()
        {
            AddBehaviour(thisStat);
        }
    }
}