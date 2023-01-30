using Core;
using Unit.Base.AI;
using Unit.Enemy.AI.Conditions;
using UnityEngine;

namespace Units.AI.States.Enemy.Boss.CrazyGhost
{
    public class IdleState : AIState
    {
        public override void Awake()
        {
            var toChase = new AITransition();
            var timeCheck = new TimeCheckCondition();
            timeCheck.SetResult(true);
            timeCheck.SetTime(2);
            toChase.AddCondition(timeCheck);
            toChase.SetTarget(new ChaseState());
            timeCheck._logicCondition = true;
            var crossDetect = new LineDetectCondition();
            crossDetect.SetUnits(InGame.PlayerBase, InGame.BossBase);
            crossDetect.SetDistance(1);
            crossDetect.SetResult(false);
            crossDetect._logicCondition = true;
            toChase.AddCondition(crossDetect);
            AddTransition(toChase);
        }

        protected override void OnEnter()
        {
            Debug.Log("IdleState");
        }
    }
}