using System.Collections;
using Core;
using Managements.Managers;
using Units.Base.Enemy;
using Units.Behaviours.Unit;
using UnityEngine;

namespace Units.AI.States.Enemy.Boss.CrazyGhost
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
            while (map.GetBlock(nextPos) == null)
            {
                dis--;
                nextPos = ThisBase.Position - nextDir * dis;
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