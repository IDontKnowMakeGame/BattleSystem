using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unit.Enemy.AI.Conditions
{
    public class CrossDetectCondition : DetectCondition
    {
        private List<Vector3> _crossPoints = new () { new Vector3(1, 0, 1), new Vector3(-1, 0, -1), new Vector3(1, 0, -1), new Vector3(-1, 0, 1)};
        protected override bool CheckConditionInternal()
        {
            for(var i = 1; i <= _distance; i++)
                if (_crossPoints.Any(crossPoint => _owner.Position + crossPoint * i == _target.Position))
                {
                    return true;
                }
            return false;
        }
    }
}