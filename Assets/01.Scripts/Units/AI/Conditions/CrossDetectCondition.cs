using System.Collections.Generic;
using Core;
using Unit.Base.AI;
using Units.Base.Unit;
using UnityEngine;

namespace Unit.Enemy.AI.Conditions
{
    public class CrossDetectCondition : DetectCondition
    {
        private List<Vector3> _crossPoints = new () { Vector3.forward , Vector3.back, Vector3.left, Vector3.right};
        protected override bool CheckConditionInternal()
        {
            foreach (var crossPoint in _crossPoints)
            {
                if(_owner.Position + crossPoint * _distance == _target.Position)
                    return true;
            }
            return false;
        }
    }
}