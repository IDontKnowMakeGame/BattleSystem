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
            for (var i = -length; i <= length; i++)
            {
                if (TargetPos.position.x == MyPos.position.x + i && TargetPos.position.z == MyPos.position.z)
                {
                    direction = TargetPos.position -  MyPos.position;
                    direction.y = 0;
                    Debug.Log(direction);
                    return true;
                }
                
                if (TargetPos.position.z == MyPos.position.z + i && TargetPos.position.x == MyPos.position.x )
                {
                    direction = TargetPos.position - MyPos.position;
                    direction.y = 0;
                    Debug.Log(direction);
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

        public Vector3 GetDirection()
        {
            return direction;
        }
    }
}