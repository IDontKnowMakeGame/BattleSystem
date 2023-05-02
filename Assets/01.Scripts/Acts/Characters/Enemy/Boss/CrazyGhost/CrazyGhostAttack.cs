using System.Collections;
using System.Collections.Generic;
using Actors.Characters.Enemy;
using Blocks;
using Blocks.Acts;
using Core;
using Managements.Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace Acts.Characters.Enemy.Boss.CrazyGhost
{
    public class CrazyGhostAttack : EnemyAttack
    {
        public void RoundAttack(int distance, bool isLast = true)
        {
            Attack();
            ThisActor.GetAct<EnemyParticle>().PlayLandingParticle();
            for (var i = -distance; i <= distance; i++)
            {
                for (var j = -distance; j <= distance; j++)
                {
                    var attackPos = new Vector3(i, 0, j);
                    Define.GetManager<MapManager>().AttackBlock(CharacterActor.Position + attackPos, DefaultStat.Atk, DefaultStat.Ats, CharacterActor, MovementType.Bounce, isLast);
                }
            }
        }
        public void AreaAttack(int distance, bool singleLayer, bool isLast = true)
        {
            Attack();
            ThisActor.GetAct<EnemyParticle>().PlayLandingParticle();
            ThisActor.StartCoroutine(AreaAttackCoroutine(distance, singleLayer, isLast));
        }

        private IEnumerator AreaAttackCoroutine(int _distance, bool singleLayer, bool isLast = true)
        {
            var area = new List<Vector3>();
            var distance = 1;
            while (distance <= _distance)
            {
                for (var i = -distance; i <= distance; i++)
                {
                    for(var j = -distance; j <= distance; j++)
                    {
                        var attackPos = new Vector3(i, 0, j);
                        if (area.Contains(attackPos)) continue;
                        Define.GetManager<MapManager>().AttackBlock(CharacterActor.Position + attackPos, DefaultStat.Atk, DefaultStat.Ats, CharacterActor, MovementType.Roll);
                        if(distance == 1 || singleLayer)
                            area.Add(attackPos);
                    }

                }

                yield return new WaitForSeconds(0.25f);
                distance++;
            }
            
            Define.GetManager<MapManager>().AttackBlock(CharacterActor.Position, DefaultStat.Atk, DefaultStat.Ats, CharacterActor, MovementType.None, isLast);
        }


        public void SoulAttack(Vector3 dir, float delay, bool isLast = true)
        {
            Attack();
            ThisActor.StartCoroutine(SoulAttackCoroutine(dir, delay, isLast));
        }

        private IEnumerator SoulAttackCoroutine(Vector3 dir, float delay, bool isLast = true)
        {
            var degree = dir.ToDegree().GetRotation();
            var range = new Vector3[] { new(1, 0, -2), new(1, 0, -1), new(1, 0, 0), new(1, 0, 1), new(1, 0, 2) };
            var distance = 0;
            var block = Define.GetManager<MapManager>().GetBlock(ThisActor.Position);
            while (distance <= 20)
            {
                var count = 0;
                for (var i = 0; i < 5; i++)
                {
                    var attackPos = CharacterActor.Position + (degree * (range[i] + Vector3.right * distance));
                    Define.GetManager<MapManager>().AttackBlock(attackPos, DefaultStat.Atk * 2f, DefaultStat.Ats,
                        CharacterActor, MovementType.Shake);
                    block = Define.GetManager<MapManager>().GetBlock(attackPos);
                    if (block != null)
                        if (block.isWalkable)
                            count++;
                }

                if (count == 0)
                    break;
                yield return new WaitForSeconds(delay);
                distance++;
            }

            Define.GetManager<MapManager>().AttackBlock(CharacterActor.Position, DefaultStat.Atk, DefaultStat.Ats,
                CharacterActor, MovementType.None, isLast);
        }
    }
}