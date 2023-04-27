﻿using Actors.Characters;
using Acts.Base;
using Blocks.Acts;
using Core;
using Data;
using Managements.Managers;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace Acts.Characters.Enemy
{
    public class EnemyAttack : Act
    {
        protected CharacterActor CharacterActor => ThisActor as CharacterActor;
        protected ItemInfo DefaultStat => ThisActor.GetAct<CharacterEquipmentAct>().CurrentWeapon.WeaponInfo;

        protected void Attack()
        {
            CharacterActor.AddState(CharacterState.Attack);
        }

        public void DefaultAttack(Vector3 dir, bool isLast = true)
        {
            Attack();
            Define.GetManager<MapManager>().AttackBlock(ThisActor.Position - dir, DefaultStat.Atk, DefaultStat.Ats, CharacterActor, MovementType.None, isLast);
        }
        
        public void HorizontalAttack(Vector3 pos, bool isLast = true)
        {
            Attack();
            var degree = ThisActor.Position.GetDegree(pos).GetRotation().GetDirection();
            var range = new Vector3[] { new (1, 0, -1), new (1, 0, 0), new (1, 0, 1) };
            for (var r = 0; r < 3; r++)
            {
                var attackPos = CharacterActor.Position + (degree * range[r]);
                Define.GetManager<MapManager>()
                    .AttackBlock(attackPos, DefaultStat.Atk, 0.1f, CharacterActor, MovementType.None);

                Define.GetManager<MapManager>().AttackBlock(CharacterActor.Position, DefaultStat.Atk, DefaultStat.Ats,
                    CharacterActor, MovementType.None, isLast);
            }
        }

        public void VerticalAttack(Vector3 pos, bool isLast = true)
        {
            Attack();
            var degree = ThisActor.Position.GetDegree(pos).GetRotation().GetDirection();
            var range = Vector3.right;
            for (var r = 1; r <= 5; r++)
            {
                var attackPos = CharacterActor.Position + (degree * range * r);
                Define.GetManager<MapManager>().AttackBlock(attackPos, DefaultStat.Atk, DefaultStat.Ats, CharacterActor);
            }
            
            Define.GetManager<MapManager>().AttackBlock(CharacterActor.Position, DefaultStat.Atk, DefaultStat.Ats, CharacterActor, MovementType.None, isLast);
        }

        public void ForwardAttack(Vector3 pos, bool isLast = true)
        {
            Attack();
            var degree = ThisActor.Position.GetDegree(pos).GetRotation().GetDirection();
            for(var i = -1; i <= 1; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    if(i == 0 && j == 0) continue;
                    var attackPos = CharacterActor.Position + (degree * new Vector3(j, 0, i));
                    Define.GetManager<MapManager>().AttackBlock(attackPos, DefaultStat.Atk, DefaultStat.Ats, CharacterActor);   
                }
            }
            Define.GetManager<MapManager>().AttackBlock(CharacterActor.Position, DefaultStat.Atk, DefaultStat.Ats, CharacterActor, MovementType.None, isLast);
        }
    }
}