using Manager;
using Unit.Enemy.AI.Conditions;
using UnityEngine;

namespace Unit.Enemy.AI.ElderBroken.State
{
    public class RoamingState : AIState
    {
        public RoamingState()
        {
            Name = "Roaming";
        }
        public override void Awake()
        {
            AITransition toChase = new AITransition();
            toChase.SetConditionState(true, false);
            toChase.SetTarget(new ChaseState());
            var rangeCondition = new RangeCheckCondition();
            rangeCondition.SetRange(1);
            rangeCondition.SetPos(GameObject.Find("Enemy").transform, GameObject.Find("Player").transform);
            toChase.AddCondition(rangeCondition.CheckCondition, true);
            AddTransition(toChase);
        }

        protected override void OnEnter()
        {
            Debug.Log(Name);
        }
        
        protected override void OnStay()
        {
            
        }
        
        protected override void OnExit()
        {
            
        }
    }
}