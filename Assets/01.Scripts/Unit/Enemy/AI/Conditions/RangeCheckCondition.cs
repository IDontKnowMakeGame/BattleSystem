using UnityEngine;

namespace Unit.Enemy.AI.Conditions
{
    public class RangeCheckCondition : AICondition
    {
        private int range;
        private Transform TargetPos;
        private Transform MyPos;
        public override bool CheckCondition()
        {
            var targetPos = TargetPos.position;
            var myPos = MyPos.position;

            targetPos.y = 0;
            myPos.y = 0;

            if (Mathf.FloorToInt(Vector3.Distance(targetPos, myPos)) <= range)
            {
                return true;
            }

            return false;
        }
        
        public void SetRange(int range)
        {
            this.range = range;
        }
        
        public void SetPos(Transform my, Transform target)
        {
            MyPos = my;
            TargetPos = target;
        }
    }
}