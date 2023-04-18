using Actors.Bases;
using UnityEngine;

namespace AI.Conditions
{
    public class LineCondition : AiCondition
    {
        public Actor TargetActor;
        public int Distance = 0;

        public override bool IsSatisfied()
        {
            var result = _thisActor.Position.IsLine(TargetActor.Position);
            if (Distance < 0)
                Distance = int.MaxValue;
            result = result && Vector3.Distance(_thisActor.Position, TargetActor.Position) <= Distance;
            return result;
        }
    }
}