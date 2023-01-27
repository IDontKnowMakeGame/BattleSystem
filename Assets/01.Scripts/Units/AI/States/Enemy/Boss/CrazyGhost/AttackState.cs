using System.Collections;
using Core;
using Managements.Managers;
using Unit.Base.AI;
using Unit.Enemy.AI.Conditions;
using Units.Behaviours.Unit;
using UnityEngine;

namespace Units.AI.States.Enemy.Boss.CrazyGhost
{
    public class AttackState : AIState
    {
        protected float angle = 0;
        CommonCondition attackCheck;
        public override void Awake()
        {
            var toChase = new AITransition();
            toChase.SetLogicCondition(true);
            attackCheck = new CommonCondition();
            attackCheck.SetResult(false);
            toChase.AddCondition(attackCheck);
            toChase.SetTarget(new ChaseState());
            AddTransition(toChase);
        }

        protected override void OnEnter()
        {
            Debug.Log("AttackState");
            var dir = InGame.PlayerBase.Position - InGame.BossBase.Position;
            angle = (Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg) - 90;
            Debug.Log(angle);
            ThisBase.StartCoroutine(AttackCoroutine());
        }

        protected virtual IEnumerator AttackCoroutine()
        {
            attackCheck.SetBool(true);
            var stat = ThisBase.GetBehaviour<UnitEquiq>().CurrentWeapon.WeaponStat;
            yield return new WaitForSeconds(stat.Ats);
            SwingAttack();
            yield return new WaitForSeconds(stat.Afs);
            attackCheck.SetBool(false);
            yield break;
        }
        
        protected void SwingAttack()
        {
            var map = Define.GetManager<MapManager>();
            var damage = InGame.BossBase.GetBehaviour<UnitStat>().NowStats.Atk;
            for (var i = -1; i <= 1; i++)
            {
                var dir = Quaternion.Euler(0, -angle, 0) * new Vector3(i, 0, 1);
                map.Damage(InGame.BossBase.Position + dir, damage, 0.5f, Color.red);
            }
        }
    }
}