using System.Collections;
using System.Collections.Generic;
using Units.Base.Unit;
using Units.Behaviours.Unit;
using UnityEngine;

public class EnemyBase : UnitBase
{
    private UnitStat _thisStat;

    public virtual UnitStat ThisStat { get => _thisStat; }
}
