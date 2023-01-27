using System.Collections.Generic;
using Core;
using Unit.Base.AI;
using UnityEngine;

namespace Unit.Enemy.AI.Conditions
{
    public class CrossDetectCondition : AICondition
    {
        private List<Vector3> _crossPoints = new () { Vector3.forward , Vector3.back, Vector3.left, Vector3.right};
        private int _crossDistance = 1;
        
        protected override bool CheckConditionInternal()
        {
            foreach (var crossPoint in _crossPoints)
            {
                if(InGame.BossBase.Position + crossPoint * _crossDistance == InGame.PlayerBase.Position)
                    return true;
            }
            return false;
        }

        public void SetDistance(int value)
        {
            _crossDistance = value;
        }
    }
}