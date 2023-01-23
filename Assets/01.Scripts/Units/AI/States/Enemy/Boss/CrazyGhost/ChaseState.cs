using Unit.Base.AI;
using UnityEngine;

namespace Units.AI.States.Enemy.Boss.CrazyGhost
{
    public class ChaseState : AIState
    {
        protected override void OnEnter()
        {
            Debug.Log("ChaseState");
        }
    }
}