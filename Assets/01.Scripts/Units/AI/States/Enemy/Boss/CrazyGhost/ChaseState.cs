using System.Collections;
using Core;
using Unit.Base.AI;
using Unit.Enemy.AI.Conditions;
using Units.Base.Enemy;
using Units.Behaviours.Unit;
using UnityEngine;

namespace Units.AI.States.Enemy.Boss.CrazyGhost
{
    public class ChaseState : AIState
    {
        private Astar astar = new ();
        private bool isChasing = false;

        public override void Awake()
        {
            var toIdle = new AITransition();
            toIdle.SetLogicCondition(true);
            var crossDetect = new CrossDetectCondition();
            crossDetect.SetDistance(1);
            crossDetect.SetResult(true);
            toIdle.AddCondition(crossDetect);
            toIdle.SetTarget(new IdleState());
            AddTransition(toIdle);
        }

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
            var move = ThisBase.GetBehaviour<EnemyMove>();
            move.MoveTo(astar.GetNextPath().Position);
            yield return new WaitUntil(() => !move.IsMoving());
            isChasing = false;
            yield break;
        }
    }
}