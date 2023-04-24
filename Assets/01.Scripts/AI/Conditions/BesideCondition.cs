using System.Collections;
using System.Collections.Generic;
using Actors.Bases;
using AI;
using UnityEngine;

namespace AI.Conditions
{
    public class BesideCondition : TargetCondition
    {
        public override bool IsSatisfied()
        {
            var dirs = new Vector3[] { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
            foreach (var dir in dirs)
            {
                if(_thisActor.transform.position.SetY(0) + dir == TargetActor.Position)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

