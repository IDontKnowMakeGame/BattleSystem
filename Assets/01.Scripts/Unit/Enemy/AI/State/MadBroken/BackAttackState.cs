using System.Collections;
using DG.Tweening;
using Manager;
using Unit.Enemy.AI.Conditions;
using Unit.Enemy.Base;
using UnityEngine;

namespace Unit.Enemy.AI.MadBroken.State
{
    public class BackAttackState  : AIState
    {
        private Vector3 _attackDireciton;
        private BaseStat _unitStat;
        private bool isStateOver = false;
        public BackAttackState()
        {
            Name = "Back";
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
            unit.StartCoroutine(AttackCoroutine());
        }


        private IEnumerator AttackCoroutine()
        {
            var attack = unit.GetBehaviour<EnemyAttack>();
            attack.SlideAttack(_attackDireciton, _unitStat.atk, _unitStat.ats);
            yield return new WaitForSeconds(_unitStat.ats + _unitStat.afs);
            var move = unit.GetBehaviour<EnemyMove>();
            move.Translate(move.position - _attackDireciton);
            var areaCheck = new AreaCheckCondition();
            var playerPos = Core.Define.PlayerBase.GetBehaviour<UnitMove>().position;
            playerPos.y = 0;
            areaCheck.SetPos(move.position, playerPos);
            areaCheck.SetRange(3); 
            yield return new WaitUntil(() => !move.IsMoving());
            if (areaCheck.CheckCondition())
            {
                unit.transform.DOMove(playerPos, 1).OnComplete(() =>
                {
                    attack.KnockBackAttack(playerPos, _unitStat.atk, _attackDireciton);
                    move.Move(playerPos);
                });
                yield return new WaitForSeconds(_unitStat.afs + 1);
            }
            isStateOver = true;
            yield break;
        }


        public void SetAttackDirection(Vector3 direction)
        {
            _attackDireciton = direction;
        }
    }
}