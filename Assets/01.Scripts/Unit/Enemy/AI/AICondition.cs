using UnityEngine;

namespace Unit.Enemy.AI
{
    public class AICondition
    {
        public virtual bool CheckCondition()
        {
            return true;
        }

        public virtual void DebugCondition()
        {
            
        }
    }
}