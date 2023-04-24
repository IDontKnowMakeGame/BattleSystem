using Actors.Bases;
using UnityEngine;

namespace AI.Conditions
{
    public class CircleCondition : TargetCondition
    {
        public int Radius = 0;
        public override bool IsSatisfied()
        {
            if (Vector3.Distance(TargetActor.Position, _thisActor.Position) <= Radius)
            {
                return true;
            }

            return false;
        }
    }
} 