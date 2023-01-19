using Units.Base.Unit;
using Units.Behaviours.Base;
using System;

namespace Units.Behaviours.Unit
{
    public class UnitBehaviour : Behaviour
    {
        public new UnitBase ThisBase => base.ThisBase as UnitBase;

        public Action onBehaviourEnd;
    }
}