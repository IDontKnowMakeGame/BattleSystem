using Acts.Base;
using Core;
using Data;
using Managements.Managers;
using UnityEngine;

namespace Acts.Characters.Enemy
{
    public class EnemyAttack : Act
    {
        private ItemInfo _defaultStat = null;
        public override void Start()
        {
            _defaultStat = ThisActor.GetAct<CharacterEquipmentAct>().CurrentWeapon.WeaponInfo;
        }

        public void DefaultAttack(Vector3 dir)
        {
            var map = Define.GetManager<MapManager>();
            map.AttackBlock(ThisActor.Position + dir, _defaultStat.Atk, _defaultStat.Ats, ThisActor);
        }

        public void ForwardAttak(Vector3 dir)
        {
            var map = Define.GetManager<MapManager>();
            var originPos = ThisActor.Position;
            var nextPos = originPos + dir;
            var degree = originPos.GetDegree(nextPos);
            for (var i = -1; i <= 1; i++)
            {
                var attackPos = new Vector3(i, 0, 1);
                attackPos = attackPos.Rotate(degree);
                attackPos += originPos;
                Debug.Log(attackPos);
                map.AttackBlock(attackPos, _defaultStat.Atk, _defaultStat.Ats, ThisActor);
            }
        }
        
        
    }
}