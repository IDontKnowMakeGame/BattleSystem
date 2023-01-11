using Unit.Enemy.AI.Conditions;
using UnityEngine;

namespace Unit.Enemy.AI.ElderBroken.State
{
    public class IdleState : AIState
    {

        public IdleState()
        {
            Name = "Idle";
        }
        public override void Awake()
        {
            AITransition toChase = new AITransition();
            toChase.SetConditionState(true, false);
            toChase.SetTarget(new ChaseState());
            var timeCondition = new TimeCheckCondition();
            timeCondition.SetTime(5);
            toChase.AddCondition(timeCondition.CheckCondition, true);
            AddTransition(toChase);
        }

        protected override void OnEnter()
        {
            Debug.Log("Idle");
        }
    }
}