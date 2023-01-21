using System;
using System.Collections.Generic;
using System.Linq;

namespace Unit.Base.AI
{
    public class AITransition
    {
        private AIState targetState;
        private List<AICondition> conditions = new();
        
        private bool isAllConditions = false;

        public void SetLogicCondition(bool logic)
        {
            isAllConditions = logic;
        }
        
        public void SetTarget(AIState target)
        {
            targetState = target;
        }
        
        public void AddCondition(AICondition condition)
        {
            conditions.Add(condition);
        }

        public bool CheckCondition()
        {
            var result = isAllConditions;

            foreach (var condition in conditions)
            {
                if (isAllConditions)
                {
                    result &= condition.CheckCondition();
                }
                else
                {
                    result |= condition.CheckCondition();
                }
            }
            
            return result;
        }
        
        public AIState NextState()
        {
            return targetState;
        }
    }
}