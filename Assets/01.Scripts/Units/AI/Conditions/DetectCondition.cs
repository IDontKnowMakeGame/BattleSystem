using Unit.Base.AI;
using Units.Base.Unit;

namespace Unit.Enemy.AI.Conditions
{
    public class DetectCondition : AICondition
    {
        protected UnitBase _target, _owner;
        protected int _distance = 1;

        public void SetUnits(UnitBase target, UnitBase owner)
        {
            _target = target;
            _owner = owner;
        }
        
        public void SetDistance(int value)
        {
            _distance = value;
        }
    }
}