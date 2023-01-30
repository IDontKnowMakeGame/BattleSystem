using System.Collections.Generic;
using Unit.Base.AI;
using Unit.Enemy.AI.Conditions;
using UnityEngine;

namespace Units.AI.States.Enemy.Boss.CrazyGhost
{
    public class RandomState : AIState
    {
        private List<CommonCondition> _commonConditions = new();
        private int random = -1;
        public override void Awake()
        {
            var toAttack = new AITransition();
            toAttack.SetLogicCondition(true);
            var common = new CommonCondition();
            common.SetResult(true);
            _commonConditions.Add(common);
            toAttack.AddCondition(common);
            toAttack.SetTarget(new AttackState());
            AddTransition(toAttack);
            
            var toBackAttack = new AITransition();
            toBackAttack.SetLogicCondition(true);
            var common2 = new CommonCondition();
            common2.SetResult(true);
            _commonConditions.Add(common2);
            toBackAttack.AddCondition(common2);
            toBackAttack.SetTarget(new BackAttackState());
            AddTransition(toBackAttack);
            
            var toTripleAttack = new AITransition();
            toTripleAttack.SetLogicCondition(true);
            var common3 = new CommonCondition();
            common3.SetResult(true);
            _commonConditions.Add(common3);
            toTripleAttack.AddCondition(common3);
            toTripleAttack.SetTarget(new TripleAttackState());
            AddTransition(toTripleAttack);
        }

        protected override void OnEnter()
        {
            random = Random.Range(0, _commonConditions.Count);
            _commonConditions[random].SetBool(true);
        }

        protected override void OnExit()
        {
            _commonConditions[random].SetBool(false);
            random = -1;
        }
    }
}