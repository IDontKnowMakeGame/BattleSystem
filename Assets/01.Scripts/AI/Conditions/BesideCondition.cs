using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

namespace AI.Conditions
{
    public class BesideCondition : AiCondition
    {
        public override bool IsSatisfied()
        {
            return _thisActor.Position.IsNeighbor(actorParam.Position);
        }
    }
}

