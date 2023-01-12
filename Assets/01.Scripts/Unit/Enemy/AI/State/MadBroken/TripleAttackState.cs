using System.Collections;
using Unit.Enemy.AI.Conditions;
using Unit.Enemy.Base;
using UnityEngine;

namespace Unit.Enemy.AI.MadBroken.State
{
    public class TripleAttackState : AIState
    {
        private Vector3 _attackDireciton;
        private BaseStat _unitStat;
        private bool isStateOver = false;
        public TripleAttackState()
        {
            Name = "Triple";
        }

        public override void Awake()
        {
            _unitStat = unit.GetBehaviour<UnitStat>().GetCurrentStat();
            AITransition toChase = new AITransition();
            toChase.SetConditionState(true);
            toChase.SetTarget(new ChaseState());
            toChase.AddCondition(() => isStateOver, true);
            AddTransition(toChase);
        }

        protected override void OnEnter()
        {
            Debug.Log(Name);
            isStateOver = false;
        }

        private IEnumerator TripleCoroutine()
        {
            
            
            isStateOver = true;
            yield return null;
        }
        
        public void SetAttackDirection(Vector3 direction)
        {
            _attackDireciton = direction;
        }
    }
}