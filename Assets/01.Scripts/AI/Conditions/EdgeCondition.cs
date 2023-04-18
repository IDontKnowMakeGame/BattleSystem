﻿using System.Linq;
using Actors.Bases;
using UnityEngine;

namespace AI.Conditions
{
    public class EdgeCondition : AiCondition
    {
        [SerializeField] private Actor targetActor;
        [SerializeField] private int distance = 0;
        public override bool IsSatisfied()
        {
            var edgePoses = new Vector3[] { new(distance, 0, distance), new (distance, 0, -distance), new (-distance, 0, distance), new (-distance, 0, -distance) };
            return edgePoses.Any(edgePos => _thisActor.Position + edgePos == targetActor.Position);
        }
    }
}