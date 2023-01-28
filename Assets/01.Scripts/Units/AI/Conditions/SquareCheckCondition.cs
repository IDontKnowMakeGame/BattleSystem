using Unit.Base.AI;
using UnityEngine;

namespace Unit.Enemy.AI.Conditions
{
    public class SquareCheckCondition : DetectCondition
    {
        protected override bool CheckConditionInternal()
        {
            for (var i = -_distance; i <= _distance; i++)
            {
                for (var j = -_distance; j <= _distance; j++)
                {
                    if (_owner.Position + new Vector3(i, 0, j) == _target.Position)
                        return true;
                }
            }

            return false;
        }
    }
}