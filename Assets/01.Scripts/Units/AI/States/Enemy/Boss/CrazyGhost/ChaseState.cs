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
            var lineDetect = new LineDetectCondition();
            lineDetect.SetUnits(InGame.PlayerBase, InGame.BossBase);
            lineDetect.SetDistance(1);
            lineDetect.SetResult(true);
            toRandom.AddCondition(lineDetect);
            toRandom.SetTarget(new RandomState());
            AddTransition(toRandom);
            
            var toSpiritAttack = new AITransition();
            var lineDetect2 = new LineDetectCondition();
            lineDetect2.SetUnits(InGame.PlayerBase, InGame.BossBase);
            lineDetect2.SetDistance(4);
            lineDetect2.SetResult(true);
            lineDetect._logicCondition = false;
            var lineDetect3 = new LineDetectCondition();
            lineDetect3.SetUnits(InGame.PlayerBase, InGame.BossBase);
            lineDetect3.SetDistance(1); 
            lineDetect3.SetResult(false);
            lineDetect3._logicCondition = true;
            var squareCheck = new SquareCheckCondition();
            squareCheck.SetUnits(InGame.PlayerBase, InGame.BossBase);
            squareCheck.SetDistance(1);
            squareCheck.SetResult(true);
            squareCheck._logicCondition = false;
            var lifeCheck = new LifeCheckCondition();
            var stat = InGame.BossBase.GetBehaviour<UnitStat>();
            lifeCheck.SetTarget(stat, stat.OriginStats.Hp * 2 / 10);
            lifeCheck.SetResult(true);
            lifeCheck._logicCondition = true;
            toSpiritAttack.AddCondition(lineDetect2);
            toSpiritAttack.AddCondition(lineDetect3);
            toSpiritAttack.AddCondition(squareCheck);
            toSpiritAttack.SetTarget(new SpiritAttackState());
            AddTransition(toSpiritAttack);
        }

        protected override void OnEnter()
        {
            //Debug.Log("ChaseState");
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