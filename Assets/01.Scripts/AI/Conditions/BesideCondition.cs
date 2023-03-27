using System.Collections;
using System.Collections.Generic;
using Actors.Bases;
using AI;
using UnityEngine;

namespace AI.Conditions
{
    public class BesideCondition : AiCondition
    {
        public Actor TargetActor;
        public override bool IsSatisfied()
        {
            return _thisActor.Position.IsNeighbor(TargetActor.Position);
        }
    }
}

