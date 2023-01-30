using Unit.Base.AI;
using UnityEngine;

namespace Unit.Enemy.AI.Conditions
{
    public class TimeCheckCondition : AICondition
    {
        private float goalTime;
        private float time;

        protected override bool CheckConditionInternal()
        {
            time += Time.deltaTime;
            if (!(time >= goalTime)) return false;
            return true;
        }

        public void SetTime(float value)
        {
            goalTime = value;
        }
        
        public void ResetTime()
        {
            time = 0;
        }
    }
}