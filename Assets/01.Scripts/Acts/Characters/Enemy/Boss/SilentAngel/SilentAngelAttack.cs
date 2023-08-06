using AttackDecals;
using Core;
using UnityEngine;

namespace Acts.Characters.Enemy.Boss.SilentAngel
{
    public class SilentAngelAttack : EnemyAttack
    {
        public override void RoundAttack(int distance, bool isLast = true)
        {
            Attack();
            //ThisActor.GetAct<EnemyParticle>().PlayLandingParticle();
            InGame.Attack(CharacterActor.Position , 0, new Vector3(distance + 0.5f, 0, distance + 0.5f) * 2, DefaultStat.Atk * 2, 0.1f, CharacterActor, isLast, FillMethod.Radial);
        }
    }
}