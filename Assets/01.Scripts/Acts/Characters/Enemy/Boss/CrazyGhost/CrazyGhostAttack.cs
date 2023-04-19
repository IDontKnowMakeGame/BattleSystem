using System.Collections;
using System.Collections.Generic;
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
        public void AreaAttack(Vector3 pos)
        {
            Attack();
            ThisActor.StartCoroutine(AreaAttackCoroutine(pos));
        }

        private IEnumerator AreaAttackCoroutine(Vector3 pos)
        {
            var area = new List<Vector3>();
            var distance = 1;
            while (distance <= 5)
            {
                for (var i = -distance; i <= distance; i++)
                {
                    for(var j = -distance; j <= distance; j++)
                    {
                        var attackPos = new Vector3(i, 0, j);
                        if (area.Contains(attackPos)) continue;
                        Define.GetManager<MapManager>().AttackBlock(CharacterActor.Position + attackPos, DefaultStat.Atk, DefaultStat.Ats, CharacterActor, MovementType.None);
                        if(distance == 1)
                            area.Add(attackPos);
                    }

                }

                yield return new WaitForSeconds(0.5f);
                distance++;
            }
            
            Define.GetManager<MapManager>().AttackBlock(CharacterActor.Position, DefaultStat.Atk, DefaultStat.Ats, CharacterActor, MovementType.None, true);
        }

        public void SoulAttack(Vector3 pos)
        {
            Attack();
            ThisActor.StartCoroutine(SoulAttackCoroutine(pos));
        }
        
        private IEnumerator SoulAttackCoroutine(Vector3 pos)
        {
            var degree = ThisActor.Position.GetDegree(pos).GetRotation().GetDirection();
            var range = new Vector3[] { new (-2, 0, 1), new (-1, 0, 1), new (0, 0, 1), new (1, 0, 1), new (2, 0, 1) };
            var distance = 0;
            var block = Define.GetManager<MapManager>().GetBlock(ThisActor.Position);
            while (distance <= 20)
            {
                var count = 0;
                for (var i = 0; i < 5; i++)
                {
                    var attackPos = CharacterActor.Position + (degree * (range[i] + Vector3.forward * distance));
                    Define.GetManager<MapManager>().AttackBlock(attackPos, DefaultStat.Atk * 2f, DefaultStat.Ats, CharacterActor, MovementType.Roll);
                    block = Define.GetManager<MapManager>().GetBlock(attackPos);
                    if (block != null)
                        if(block.isWalkable)
                            count++;
                }

                if (count == 0)
                    break;
                yield return new WaitForSeconds(0.05f);
                distance++;
            }
            
            Define.GetManager<MapManager>().AttackBlock(CharacterActor.Position, DefaultStat.Atk, DefaultStat.Ats, CharacterActor, MovementType.None, true);
        }
    }
}