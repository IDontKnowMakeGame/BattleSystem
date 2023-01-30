using System.Collections;
using System.Collections.Generic;
using Units.Base.Unit;
using Units.Behaviours.Unit;
using UnityEngine;

public class EnemyBase : UnitBase
{
    [SerializeField] protected UnitStat thisStat;

    public UnitStat ThisStat { get => thisStat; }
}
