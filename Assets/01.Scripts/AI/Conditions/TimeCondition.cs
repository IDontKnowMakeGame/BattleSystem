using UnityEngine;

namespace AI.Conditions
{
    public class TimeCondition : AiCondition
    {
        private float _time;
        
        public override bool IsSatisfied()
        {
            _time += Time.deltaTime;
            return _time >= floatParam ;
        }
        
        public void SetTimeToWait(float timeToWait)
        {
             floatParam = timeToWait;
        }
        
        public override void Reset()
        {
            _time = 0;
        }
    }
}