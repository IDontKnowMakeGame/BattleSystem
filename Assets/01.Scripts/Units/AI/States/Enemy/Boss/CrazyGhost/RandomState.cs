using System.Collections.Generic;
using Unit.Base.AI;
using Unit.Enemy.AI.Conditions;
using Units.AI.States.Enemy.Attack;
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
            var common = new CommonCondition();
            common.SetResult(true);
            _commonConditions.Add(common);
            toAttack.AddCondition(common);
            var attack = new AttackState();
            attack.NextState = new ChaseState();
            toAttack.SetTarget(attack);
            AddTransition(toAttack);
            
            var toBackAttack = new AITransition();
            var common2 = new CommonCondition();
            common2.SetResult(true);
            _commonConditions.Add(common2);
            toBackAttack.AddCondition(common2);
            attack = new BackAttackState();
            attack.NextState = new ChaseState();
            toBackAttack.SetTarget(attack);
            AddTransition(toBackAttack);
            
            var toTripleAttack = new AITransition();
            var common3 = new CommonCondition();
            common3.SetResult(true);
            _commonConditions.Add(common3);
            toTripleAttack.AddCondition(common3);
            attack = new TripleAttackState();
            attack.NextState = new ChaseState();
            toTripleAttack.SetTarget(attack);
            AddTransition(toTripleAttack);
        }

        protected override void OnEnter()
        {
            random = Random.Range(0, _commonConditions.Count);
            //random = 1;
            _commonConditions[random].SetBool(true);
        }

        protected override void OnExit()
        {
            _commonConditions[random].SetBool(false);
            random = -1;
        }
    }
}