using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Units.AI.States.Enemy.Common.ElderCommonGhost;
using Units.Base.Enemy;
using Units.Behaviours.Unit;
using UnityEngine;

public class CommonElderGhostBase : CommonEnemyBase
{
    [SerializeField] private UnitEquiq _enemyWeapons;
    [SerializeField] private CharacterRender characterRender;
    protected override void Init()
    {
        AddBehaviour(ThisStat);
        AddBehaviour(_enemyWeapons);
        AddBehaviour<EnemyMove>();
        var fsm = AddBehaviour<UnitFSM>();
        fsm.SetDefaultState<IdleState>();
        base.Init();
        ChangeBehaviour(characterRender);
    }
}
