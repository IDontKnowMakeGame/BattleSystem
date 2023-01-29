using System.Collections;
using Core;
using Unit.Base.AI;
using Unit.Enemy.AI.Conditions;
using Units.Base.Enemy;
using Units.Behaviours.Unit;
using UnityEngine;

namespace Units.AI.States.Enemy.Boss.CrazyGhost
{
    public class TripleAttackState : AttackState
    {
        protected override IEnumerator AttackCoroutine()
        {
            var move = ThisBase.GetBehaviour<EnemyMove>();
            var weaponStat = ThisBase.GetBehaviour<UnitEquiq>().CurrentWeapon.WeaponStat;
            yield return new WaitUntil(() => !move.IsMoving());
            yield return new WaitForSeconds(weaponStat.Ats);
            ForwardAttack();
            yield return new WaitForSeconds(weaponStat.Afs);
            var dir = Quaternion.Euler(0, -angle, 0) * Vector3.forward;
            if (ThisBase.Position + dir != InGame.PlayerBase.Position)
            {
                move.Translate(dir);
                yield return new WaitUntil(() => !move.IsMoving());
            }
            yield return new WaitForSeconds(weaponStat.Ats);
            ForwardAttack();
            yield return new WaitForSeconds(weaponStat.Afs);
            if (ThisBase.Position + dir != InGame.PlayerBase.Position)
            {
                move.Translate(dir);
                yield return new WaitUntil(() => !move.IsMoving());
            }
            yield return new WaitForSeconds(weaponStat.Ats);
            SwingAttack();
            yield return new WaitForSeconds(weaponStat.Afs);
            attackCheck.SetBool(false);
            yield break;
        }
    }
}