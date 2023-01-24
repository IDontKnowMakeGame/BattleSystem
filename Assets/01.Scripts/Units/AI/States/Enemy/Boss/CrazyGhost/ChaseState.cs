using System.Collections;
using Core;
using Unit.Base.AI;
using Units.Behaviours.Unit;
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
            astar.SetPath(ThisBase.Position, InGame.PlayerBase.Position);
            ThisBase.StartCoroutine(astar.FindPath());
            yield return new WaitUntil(astar.IsFinished);
            Debug.Log(ThisBase.Position);
            var move = ThisBase.GetBehaviour<UnitMove>();
            move.MoveTo(astar.GetNextPath().Position);
            yield return new WaitForSeconds(5);
            isChasing = false;
            yield break;
        }
    }
}