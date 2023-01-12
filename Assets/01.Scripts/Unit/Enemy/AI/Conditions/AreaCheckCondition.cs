using UnityEngine;

namespace Unit.Enemy.AI.Conditions
{
    public class AreaCheckCondition : AICondition
    {
        private Vector3 TargetPos;
        private Vector3 MyPos;
        private int range = 0;
        public override bool CheckCondition()
        {
            for (var i = -range; i <= range; i++)
            {
                for (var j = -range; j <= range; j++)
                {
                    if(TargetPos.x == MyPos.x + i && TargetPos.z == MyPos.z + j)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void SetRange(int value)
        {
            range = value;
        }
        
        public void SetPos(Vector3 my, Vector3 target)
        {
            MyPos = my;
            TargetPos = target;
        }
    }
}