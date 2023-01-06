using System;
using System.Collections.Generic;
using System.Linq;

namespace Unit.Enemy.AI
{
    public class AITransition
    {
        private AIState targetState;
        private List<Func<bool>> positiveCondition = new List<Func<bool>>();
        private List<Func<bool>> negativeCondition = new List<Func<bool>>();
        
        private bool isAllPositiveConditions;
        private bool isAllNegativeConditions;

        public void SetConditionState(bool positive = false, bool negative = false)
        {
            isAllPositiveConditions = positive;
            isAllNegativeConditions = negative;
        }
        
        public void SetTarget(AIState target)
        {
            targetState = target;
        }
        
        public void AddCondition(Func<bool> condition, bool positive)
        {
            if (positive)
            {
                positiveCondition.Add(condition);
            }
            else
            {
                negativeCondition.Add(condition);
            }
        }

        public bool CheckCondition()
        {
            var result = false;
            var positveResult = isAllPositiveConditions;
            positveResult = isAllPositiveConditions ? positiveCondition.Aggregate(positveResult, (current, condition) => current & condition()) : positiveCondition.Aggregate(positveResult, (current, condition) => current | condition());

            var negativeResult = isAllNegativeConditions;
            negativeResult = isAllNegativeConditions ? negativeCondition.Aggregate(negativeResult, (current, condition) => current & condition()) : negativeCondition.Aggregate(negativeResult, (current, condition) => current | condition());
            
            result = positveResult & !negativeResult;
            
            return result;
        }
        
        public AIState NextState()
        {
            return targetState;
        }
    }
}