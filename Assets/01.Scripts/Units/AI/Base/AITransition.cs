using System;
using System.Collections.Generic;
using System.Linq;

namespace Unit.Base.AI
{
    public class AITransition
    {
        private AIState targetState;
        private List<AICondition> conditions = new();
        
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
            var result = true;
            var count = 0;
            foreach (var condition in conditions)
            {
                if (count == 0)
                {
                    result = condition.CheckCondition();
                }
                else if (condition._logicCondition)
                {
                    result &= condition.CheckCondition();
                }
                else
                {
                    result |= condition.CheckCondition();
                }

                count = 1;
            }
            
            return result;
        }
        
        public AIState NextState()
        {
            return targetState;
        }
    }
}