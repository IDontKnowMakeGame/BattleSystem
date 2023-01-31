using Unit.Base.AI;
using Units.Behaviours.Unit;

namespace Unit.Enemy.AI.Conditions
{
    public class LifeCheckCondition : AICondition
    {
        private UnitStat _targetStat;
        private float _targetLife;
        public void SetTarget(UnitStat target, float life)
        {
            _targetStat = target;
            _targetLife = life;
        }

        protected override bool CheckConditionInternal()
        {
            return _targetStat.NowStats.Hp <= _targetLife;
        }
    }
}