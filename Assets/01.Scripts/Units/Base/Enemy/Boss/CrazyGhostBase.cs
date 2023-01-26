using System.Collections;
using System.Collections.Generic;
using Units.AI.States.Enemy.Boss.CrazyGhost;
using Units.Base.Player;
using Units.Behaviours.Unit;
using UnityEngine;

public class CrazyGhostBase : EnemyBase
{
    protected override void Init()
    {
        AddBehaviour<UnitEquiq>().isEnemy = true;
        AddBehaviour<PlayerMove>();
        var fsm = AddBehaviour<UnitFSM>();
        fsm.SetDefaultState<IdleState>();
        base.Init();
    }
}
