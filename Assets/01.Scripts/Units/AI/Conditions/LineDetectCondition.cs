using System.Collections.Generic;
using System.Linq;
using Core;
using Unit.Base.AI;
using Units.Base.Unit;
using UnityEngine;

namespace Unit.Enemy.AI.Conditions
{
    public class LineDetectCondition : DetectCondition
    {
        private List<Vector3> _crossPoints = new () { Vector3.forward , Vector3.back, Vector3.left, Vector3.right};
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