using UnityEngine;

namespace Unit.Enemy.AI.Conditions
{
    public class LineCheckCondition : AICondition
    {
        private int length;
        private Transform TargetPos;
        private Transform MyPos;
        private Vector3 direction;
        
        public override bool CheckCondition()
        {
            if(TargetPos.position.x == MyPos.position.x || TargetPos.position.z == MyPos.position.z)
            {
                if (Vector3.Distance(TargetPos.position, MyPos.position) <= length)
                {
                    direction = TargetPos.position - MyPos.position;
                    direction.Normalize();
                    return true;
                }
            }

            return false;
        }
        
        public void SetLength(int length)
        {
            this.length = length;
        }
        
        public void SetPos(Transform my, Transform target)
        {
            MyPos = my;
            TargetPos = target;
        }
    }
}