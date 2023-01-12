using Unit.Enemy.AI.Conditions;
using Unit.Enemy.Base;
using UnityEngine;

namespace Unit.Enemy.AI.MadBroken.State
{
    public class NormalState : AIState
    {
        private Vector3 _attackDireciton;
        private BaseStat _unitStat;
        public NormalState()
        {
            Name = "Normal";
        }

        public override void Awake()
        {
            _unitStat = unit.GetBehaviour<UnitStat>().GetCurrentStat();
            AITransition toChase = new AITransition();
            toChase.SetConditionState(true);
            toChase.SetTarget(new ChaseState());
            var timeCheck = new TimeCheckCondition();
            timeCheck.SetTime(_unitStat.ats + _unitStat.afs);
            toChase.AddCondition(timeCheck.CheckCondition, true);
            AddTransition(toChase);
        }

        protected override void OnEnter()
        {
            Debug.Log(Name);
            unit.GetBehaviour<EnemyAttack>().SlideAttack(_attackDireciton, _unitStat.atk, _unitStat.ats);
        }
        
        public void SetAttackDirection(Vector3 direction)
        {
            _attackDireciton = direction;
        }
    }
}