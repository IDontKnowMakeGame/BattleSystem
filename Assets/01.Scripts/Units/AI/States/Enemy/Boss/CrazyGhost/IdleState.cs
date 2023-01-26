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
            toChase.SetLogicCondition(true);
            var timeCheck = new TimeCheckCondition();
            timeCheck.SetResult(true);
            timeCheck.SetTime(2);
            toChase.AddCondition(timeCheck);
            toChase.SetTarget(new ChaseState());
            var crossDetect = new CrossDetectCondition();
            crossDetect.SetDistance(1);
            crossDetect.SetResult(false);
            toChase.AddCondition(crossDetect);
            AddTransition(toChase);
        }

        protected override void OnEnter()
        {
            Debug.Log("IdleState");
        }
    }
}