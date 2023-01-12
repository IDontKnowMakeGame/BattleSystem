using System.Collections;
using Manager;
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
            unit.StartCoroutine(TripleCoroutine());
        }

        private IEnumerator TripleCoroutine()
        {
            var attack = unit.GetBehaviour<EnemyAttack>();
            var move = unit.GetBehaviour<EnemyMove>();
            var map = GameManagement.Instance.GetManager<MapManager>();
            attack.SlideAttack(_attackDireciton, _unitStat.atk, _unitStat.ats);
            yield return new WaitForSeconds(_unitStat.ats + _unitStat.afs);
            var targetBlock = map.GetBlock(move.position + _attackDireciton);
            if (targetBlock.GetUnit() == null)
            {
                move.Translate(targetBlock.transform.position, 1);
            }

            yield return new WaitUntil(() => !move.IsMoving());
            attack.SlideAttack(_attackDireciton, _unitStat.atk, _unitStat.ats);
            yield return new WaitForSeconds(_unitStat.ats + _unitStat.afs);
            
            
            targetBlock = map.GetBlock(move.position + _attackDireciton);
            if (targetBlock.GetUnit() == null)
            {
                move.Translate(targetBlock.transform.position, 1);
            }
            yield return new WaitUntil(() => !move.IsMoving());
            attack.HalfAttack(_attackDireciton, _unitStat.atk, _unitStat.ats);
            yield return new WaitForSeconds(_unitStat.ats + _unitStat.afs);
            
            
            isStateOver = true;
            yield return null;
        }
        
        public void SetAttackDirection(Vector3 direction)
        {
            _attackDireciton = direction;
        }
    }
}