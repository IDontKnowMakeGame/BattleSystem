using System.Collections;
using System.Collections.Generic;
using Units.AI.States.Enemy.Boss.CrazyGhost;
using UnityEngine;

public class CrazyGhostBase : EnemyBase
{
    protected override void Init()
    {
        var fsm = AddBehaviour<UnitFSM>();
        fsm.SetDefaultState<IdleState>();
        base.Init();
    }
}
