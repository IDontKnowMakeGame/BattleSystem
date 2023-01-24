using System.Collections;
using Core;
using Unit.Base.AI;
using UnityEngine;

namespace Units.AI.States.Enemy.Boss.CrazyGhost
{
    public class ChaseState : AIState
    {
        private Astar astar = new ();
        private bool isChasing = false;
        protected override void OnEnter()
        {
            Debug.Log("ChaseState");
        }

        protected override void OnStay()
        {
            if (isChasing == false)
                ThisBase.StartCoroutine(ChaseCoroutine());
        }

        private IEnumerator ChaseCoroutine()
        {
            isChasing = true;
            astar.SetPath(InGame.PlayerBase.Position, ThisBase.Position);
            ThisBase.StartCoroutine(astar.FindPath());
            yield return new WaitUntil(astar.IsPathFining);
            Debug.Log(astar.GetNextPath().Position);
            yield return new WaitForSeconds(5);
            isChasing = false;
            yield break;
        }
    }
}