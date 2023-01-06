using UnityEngine;

namespace Unit.Enemy.AI.Conditions
{
    public class TimeCheckCondition : AICondition
    {
        public float GoalTime { get; set; }
        private float time;

        public override bool CheckCondition()
        {
            time += Time.deltaTime;
            if (time >= GoalTime)
            {
                time = 0;
                return true;
            }
            return false;
        }

        public void SetTime(float time)
        {
            GoalTime = time;
        }
    }
}