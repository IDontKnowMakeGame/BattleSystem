using Units.Base.Unit;
using Units.Behaviours.Base;

namespace Units.Behaviours.Unit
{
    public class UnitBehaviour : Behaviour
    {
        public new UnitBase ThisBase => base.ThisBase as UnitBase;
    }
}