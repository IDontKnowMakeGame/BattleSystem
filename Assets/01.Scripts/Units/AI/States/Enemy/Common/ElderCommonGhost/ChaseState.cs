using System.Collections;
using Core;
using Unit.Base.AI;
using Unit.Enemy.AI.Conditions;
using Units.AI.States.Enemy.Boss.CrazyGhost;
using Units.Base.Enemy;
using Units.Base.Unit;
using Units.Behaviours.Unit;
using UnityEngine;

namespace _01.Scripts.Units.AI.States.Enemy.Common.ElderCommonGhost
{
    public class ChaseState : AIState
    {
        private Astar astar = new ();
        private bool isChasing = false;
        public override void Awake()
        {
            var toAttack = new AITransition();
            var lineDetect = new LineDetectCondition();
            lineDetect.SetUnits(InGame.PlayerBase, ThisBase as UnitBase);
            lineDetect.SetDistance(1);
            lineDetect.SetResult(true);
            toAttack.AddCondition(lineDetect);
            var attack = new AttackState();
            attack.NextState = this;
            toAttack.SetTarget(attack);
            AddTransition(toAttack);
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