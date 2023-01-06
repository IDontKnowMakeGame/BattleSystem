using System.Collections;
using System.Collections.Generic;
using Unit;
using UnityEngine;

public class EnemyBase : UnitBase
{
    [SerializeField] private UnitStat enemyStat;

    protected override void Init()
    {
        enemyStat = AddBehaviour<UnitStat>();
    }
}
