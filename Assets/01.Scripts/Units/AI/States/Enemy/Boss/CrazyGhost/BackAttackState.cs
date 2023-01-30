using System.Collections;
using Core;
using Unit.Base.AI;
using Unit.Enemy.AI.Conditions;
using Units.Base.Enemy;
using Units.Behaviours.Unit;
using UnityEngine;

namespace Units.AI.States.Enemy.Boss.CrazyGhost
{
    public class BackAttackState : AttackState
    {
        protected override IEnumerator AttackCoroutine()
        {
            var move = ThisBase.GetBehaviour<EnemyMove>();
            yield return new WaitUntil(() => !move.IsMoving());
            attackCheck.SetBool(true);
            var stat = ThisBase.GetBehaviour<UnitStat>().NowStats;
            var weaponStat = ThisBase.GetBehaviour<UnitEquiq>().CurrentWeapon.WeaponStat;
            var dir = Quaternion.Euler(0, -angle, 0) * Vector3.forward;
            yield return new WaitForSeconds(weaponStat.Ats);
            ForwardAttack();
            var nextPos = ThisBase.Position - dir;
            move.MoveTo(nextPos, stat.Agi);
            yield return new WaitUntil(() => !move.IsMoving());
            yield return new WaitForSeconds(weaponStat.Afs);
            var squareCheck = new SquareCheckCondition();
            squareCheck.SetUnits(InGame.PlayerBase, InGame.BossBase);
            squareCheck.SetDistance(3);
            squareCheck.SetResult(true);
            if (squareCheck.CheckCondition() == true)
            {
                move.JumpTo(InGame.PlayerBase.Position - dir,1, stat.Agi);
                yield return new WaitUntil(() => !move.IsMoving());
                AreaAttack(1);
                yield return new WaitForSeconds(3f);
            }
            attackCheck.SetBool(false);
        }
    }
}