using System;
using Units.Behaviours.Unit;
using UnityEngine;

namespace Units.Base.Enemy.Boss
{
    public class BossBase : EnemyBase
    {
        [SerializeField] protected new BossStat thisStat;
        public override UnitStat ThisStat => thisStat;
    }
}