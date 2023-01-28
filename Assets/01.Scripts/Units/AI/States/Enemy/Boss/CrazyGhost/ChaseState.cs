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
            var toRandom = new AITransition();
            toRandom.SetLogicCondition(true);
            var crossDetect = new CrossDetectCondition();
            crossDetect.SetUnits(InGame.PlayerBase, InGame.BossBase);
            crossDetect.SetDistance(1);
            crossDetect.SetResult(true);
            toRandom.AddCondition(crossDetect);
            toRandom.SetTarget(new RandomState());
            AddTransition(toRandom);
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
            var move = ThisBase.GetBehaviour<EnemyMove>();
            var stat = ThisBase.GetBehaviour<UnitStat>().NowStats;
            var path = astar.GetNextPath();
            if (path != null)
            {
                move.MoveTo(path.Position, stat.Agi);
            }
            yield return new WaitUntil(() => !move.IsMoving());
            isChasing = false;
            yield break;
        }
    }
}