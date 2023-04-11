using Actors.Characters;
using Acts.Base;
using Blocks.Acts;
using Core;
using Data;
using Managements.Managers;
using UnityEngine;

namespace Acts.Characters.Enemy
{
    public class EnemyAttack : Act
    {
        private CharacterActor CharacterActor => ThisActor as CharacterActor;
        private ItemInfo DefaultStat => ThisActor.GetAct<CharacterEquipmentAct>().CurrentWeapon.WeaponInfo;

        public void HorizontalAttack(Vector3 pos)
        {
            var degree = ThisActor.Position.GetDegree(pos).GetRotation().GetDirection();
            var range = new Vector3[] { new (-1, 0, 1), new (0, 0, 1), new (1, 0, 1) };
            for (var r = 0; r < 3; r++)
            {
                var attackPos = CharacterActor.Position + (degree * range[r]);
                Define.GetManager<MapManager>().AttackBlock(attackPos, DefaultStat.Atk, DefaultStat.Ats, CharacterActor);
            }
            Define.GetManager<MapManager>().AttackBlock(CharacterActor.Position, DefaultStat.Atk, DefaultStat.Ats, CharacterActor, MovementType.None, true);
        }
        
        public void VerticalAttack(Vector3 pos)
        {
            var degree = pos.GetDegree(CharacterActor.Position).GetRotation().GetDirection();
            var range = new Vector3[] { new (0, 0, 1), new (0, 0, 2), new (1, 0, 3) };
            for (var r = 0; r < 3; r++)
            {
                var attackPos = CharacterActor.Position + pos + range[r];
                Define.GetManager<MapManager>().AttackBlock(attackPos, DefaultStat.Atk, DefaultStat.Ats, CharacterActor);
            }
            Define.GetManager<MapManager>().AttackBlock(CharacterActor.Position, DefaultStat.Atk, DefaultStat.Ats, CharacterActor, MovementType.None, true);
        }
    }
}