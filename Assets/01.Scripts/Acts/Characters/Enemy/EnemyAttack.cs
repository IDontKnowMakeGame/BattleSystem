using Acts.Base;
using Core;
using Managements.Managers;
using UnityEngine;

namespace Acts.Characters.Enemy
{
    public class EnemyAttack : Act
    {
        private float _defaultDamage = 0;
        public override void Start()
        {
            var stat = ThisActor.GetAct<CharacterEquipmentAct>().CurrentWeapon.WeaponInfo;
            _defaultDamage = stat.Atk;
        }

        public void DefaultAttack(Vector3 dir)
        {
            var map = Define.GetManager<MapManager>();
            map.AttackBlock(ThisActor.Position + dir, _defaultDamage, ThisActor);
        }
    }
}