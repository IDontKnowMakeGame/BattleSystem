using Units.Behaviours.Unit;
using UnityEngine;

namespace Units.Base.Enemy
{
    public class CommonEnemyBase : EnemyBase
    {
        [SerializeField] protected UnitStat thisStat;
        public override UnitStat ThisStat => thisStat;
    }
}