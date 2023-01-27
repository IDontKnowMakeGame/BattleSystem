using Unit.Base.AI;
using UnityEngine;

namespace Unit.Enemy.AI.Conditions
{
    public class RandomNumCondition : AICondition
    {
        private int num = 0;
        private int answer = 0;
        protected override bool CheckConditionInternal()
        {
            return num == answer;
        }
        
        public void SetNum(int num)
        {
            this.num = num;
        }
        
        public void SetAnswer(int answer)
        {
            this.answer = answer;
        }
    }
}