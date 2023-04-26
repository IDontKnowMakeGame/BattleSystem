using Core;
using Managements.Managers;
using UnityEngine;

namespace AI.Conditions
{
    public class MovableCondition : AiCondition
    {
        public Vector3 nextDir;

        public override bool IsSatisfied()
        {
            return Define.GetManager<MapManager>().IsStayable(_thisActor.Position + nextDir);
        }
    }
}