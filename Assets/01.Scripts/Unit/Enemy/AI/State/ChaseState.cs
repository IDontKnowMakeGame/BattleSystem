using Unit.Enemy.AI.Conditions;
using UnityEngine;

namespace Unit.Enemy.AI.State
{
    public class ChaseState : AIState
    {
        public ChaseState()
        {
            Name = "Chase";
        }
        public override void Awake()
        {
            AITransition toIdle = new AITransition();
            toIdle.SetConditionState(true, false);
            toIdle.SetTarget(new IdleState());
            var timeCondition = new TimeCheckCondition();
            timeCondition.SetTime(5);
            toIdle.AddCondition(timeCondition.CheckCondition, true);
            AddTransition(toIdle);
        }

        protected override void OnEnter()
        {
            Debug.Log("Chase");
        }
    }
}