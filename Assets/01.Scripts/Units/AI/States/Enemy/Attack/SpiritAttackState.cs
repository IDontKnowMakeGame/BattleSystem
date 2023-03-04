using System.Collections;
using Core;
using Managements.Managers;
using Units.Base.Enemy;
using Units.Behaviours.Unit;
using UnityEngine;

namespace Units.AI.States.Enemy.Attack
{
    public class SpiritAttackState : AttackState
    {
        protected override IEnumerator AttackCoroutine()
        {
            var move = ThisBase.GetBehaviour<EnemyMove>();
            var weaponStat = ThisBase.GetBehaviour<UnitEquiq>().CurrentWeapon.WeaponStat;
            yield return new WaitUntil(() => !move.IsMoving());
            var nextDir = Quaternion.Euler(0, -angle, 0) * Vector3.forward;
            nextDir.Normalize();
            nextDir.x = Mathf.Round(nextDir.x);
            nextDir.z = Mathf.Round(nextDir.z);
            var nextPos = ThisBase.Position - nextDir * 3;
            var dis = 3;
            var map = Define.GetManager<MapManager>();
            bool checkBlock = map.GetBlock(nextPos) == null;
            if(checkBlock == false)
                if (map.GetBlock(nextPos).canBossEnter == false)
                    checkBlock = true;
            while (checkBlock)
            {
                dis--;
                nextPos = ThisBase.Position - nextDir * dis;
                if (dis <= 0)
                    break;
            }
            move.Translate(-nextDir * dis, 1);
            yield return new WaitUntil(() => !move.IsMoving());
            yield return new WaitForSeconds(weaponStat.Ats);
            BeamAttack();
            yield return new WaitForSeconds(5f);
            attackCheck.SetBool(false);
            yield break;
        }
    }
}