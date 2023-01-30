using UnityEngine;

namespace Unit.Base.AI
{
    public class AICondition
    {
        protected bool _resultCondition = true;
        public bool _logicCondition = true;
        
        public void SetResult(bool result)
        {
            _resultCondition = result;
        }
        
        public virtual bool CheckCondition()
        {
            var result = (CheckConditionInternal() == _resultCondition);
            return result;
        }
        
        protected virtual bool CheckConditionInternal()
        {
            return true;
        }

        public virtual void DebugCondition()
        {
            
        }
    }
}