using UnityEngine;

namespace AI.Conditions
{
    public class TimeCondition : AiCondition
    {

        public float TargetTime;
        private float _time;
        
        public override bool IsSatisfied()
        {
            _time += Time.deltaTime;
            return _time >= TargetTime ;
        }
        
        public void SetTimeToWait(float timeToWait)
        {
             TargetTime = timeToWait;
        }
        
        public override void Reset()
        {
            _time = 0;
        }
    }
}