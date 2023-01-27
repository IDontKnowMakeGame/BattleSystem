using System.Collections;
using System.Collections.Generic;
using Core;
using Managements.Managers;
using Units.AI.States.Enemy.Boss.CrazyGhost;
using Units.Base.Enemy;
using Units.Base.Player;
using Units.Behaviours.Unit;
using UnityEngine;
using Input = UnityEngine.Input;

public class CrazyGhostBase : EnemyBase
{
    protected override void Init()
    {
        InGame.BossBase = this;
        AddBehaviour<UnitEquiq>().isEnemy = true;
        AddBehaviour<EnemyMove>();
        AddBehaviour(thisStat);
        var fsm = AddBehaviour<UnitFSM>();
        fsm.SetDefaultState<IdleState>();
        base.Init();
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Define.GetManager<MapManager>().Damage(Position + Vector3.back, 2, 0.5f, Color.red);
        }
        base.Update();
    }
}
