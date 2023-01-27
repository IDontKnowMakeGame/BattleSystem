using Unit.Base.AI;
using Unit.Enemy.AI.Conditions;
using UnityEngine;

namespace Units.AI.States.Enemy.Boss.CrazyGhost
{
    public class RandomState : AIState
    {
        public override void Awake()
        {
            var toAttack = new AITransition();
            toAttack.SetLogicCondition(true);
            var randomNum = new RandomNumCondition();
            randomNum.SetNum(0);
            randomNum.SetAnswer(0);
            toAttack.AddCondition(randomNum);
            toAttack.SetTarget(new AttackState());
            AddTransition(toAttack);
        }

        protected override void OnEnter()
        {
            Debug.Log("RandomState");
        }
    }
}