using Unit.Base.AI;
using UnityEngine;

namespace Unit.Enemy.AI.Conditions
{
    public class CommonCondition : AICondition
    {
        private bool _temp = false;
        protected override bool CheckConditionInternal()
        {
            return _temp;
        }

        public void SetBool(bool temp)
        {
            _temp = temp;
        }
    }
}