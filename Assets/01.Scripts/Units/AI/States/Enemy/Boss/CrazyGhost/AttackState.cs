using System.Collections;
using System.Collections.Generic;
using Core;
using Managements.Managers;
using Unit.Base.AI;
using Unit.Enemy.AI.Conditions;
using Units.Base.Unit;
using Units.Behaviours.Unit;
using UnityEngine;

namespace Units.AI.States.Enemy.Boss.CrazyGhost
{
    public class AttackState : AIState
    {
        protected int angle = 0;
        protected CommonCondition attackCheck;
        public AIState NextState;
        public override void Awake()
        {
            var toChase = new AITransition();
            attackCheck = new CommonCondition();
            attackCheck.SetResult(false);
            attackCheck.SetBool(true);
            toChase.AddCondition(attackCheck);
            toChase.SetTarget(NextState);
            AddTransition(toChase);
        }

        protected override void OnEnter()
        {
            attackCheck.SetBool(true);
            var dir = InGame.PlayerBase.Position - ThisBase.Position;
            angle = (int)(Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg) - 90;
            ThisBase.StartCoroutine(AttackCoroutine());
        }

        protected virtual IEnumerator AttackCoroutine()
        {
            attackCheck.SetBool(true);
            var stat = ThisBase.GetBehaviour<UnitEquiq>().CurrentWeapon.WeaponStat;
            yield return new WaitForSeconds(stat.Ats);
            ForwardAttack();
            yield return new WaitForSeconds(1.5f);
            attackCheck.SetBool(false);
            yield break;
        }

        protected void AreaAttack(int range)
        {
            var map = Define.GetManager<MapManager>();
            var damage = ThisBase.GetBehaviour<UnitEquiq>().CurrentWeapon.WeaponStat.Atk;
            for (var i = -range; i <= range; i++)
            {
                for (var j = -range; j <= range; j++)
                {
                    var dir = Quaternion.Euler(0, -angle, 0) * new Vector3(i, 0, j);
                    map.Damage(ThisBase.Position + dir, damage, 0.5f, Color.red, ThisBase as UnitBase);
                }
            }
        }
        
        protected void ForwardAttack()
        {
            var map = Define.GetManager<MapManager>();
            var damage = ThisBase.GetBehaviour<UnitEquiq>().CurrentWeapon.WeaponStat.Atk;
            for (var i = -1; i <= 1; i++)
            {
                var dir = Quaternion.Euler(0, -angle, 0) * new Vector3(i, 0, 1);
                map.Damage(ThisBase.Position + dir, damage, 0.5f, Color.red, ThisBase as UnitBase);
            }
        }

        protected void SwingAttack()
        {
            var map = Define.GetManager<MapManager>();
            var damage = ThisBase.GetBehaviour<UnitEquiq>().CurrentWeapon.WeaponStat.Atk;
            Vector3 dir;
            for (var i = -1; i <= 1; i++)
            {
                dir = Quaternion.Euler(0, -angle, 0) * new Vector3(i, 0, 1);
                map.Damage(ThisBase.Position + dir, damage, 0.5f, Color.red, ThisBase as UnitBase);
            }
            dir = Quaternion.Euler(0, -angle, 0) * new Vector3(1, 0, 0);
            map.Damage(ThisBase.Position + dir, damage, 0.5f, Color.red, ThisBase as UnitBase);
            
            dir = Quaternion.Euler(0, -angle, 0) * new Vector3(-1, 0, 0);
            map.Damage(ThisBase.Position + dir, damage, 0.5f, Color.red, ThisBase as UnitBase);
        }
        
        protected void BeamAttack()
        {
            var map = Define.GetManager<MapManager>();
            var damage = ThisBase.GetBehaviour<UnitEquiq>().CurrentWeapon.WeaponStat.Atk;
            var dir = Quaternion.Euler(0, -angle, 0) * new Vector3(0, 0, 1);
            var count = 0;
            var lastBlock = map.GetBlock(ThisBase.Position + dir);
            while (lastBlock != null)
            {
                count++;
                lastBlock = map.GetBlock(ThisBase.Position + dir * count);    
            }

            for (var i = 0; i < count; i++)
            {
                for(var j = -1; j <= 1; j++)
                {
                    var dir2 = Quaternion.Euler(0, -angle, 0) * new Vector3(j, 0, -1);
                    dir2.x = Mathf.Round(dir2.x);
                    dir2.z = Mathf.Round(dir2.z);
                    map.Damage((ThisBase.Position + dir * i) + dir2, damage * 2, 0.5f, Color.red, ThisBase as UnitBase);
                }
            }
        }
        

        protected override void OnExit()
        {
            attackCheck.SetBool(true);
        }
    }
}