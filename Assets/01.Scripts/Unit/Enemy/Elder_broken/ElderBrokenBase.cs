using System.Collections;
using System.Collections.Generic;
using Unit.Enemy.AI.ElderBroken.State;
using Unit.Enemy.Base;
using UnityEngine;

public class ElderBrokenBase : EnemyBase
{
    protected override void Init()
    {
        base.Init();
        AddBehaviour<EnemyFSM>().SetDefaultState<RoamingState>();
        AddBehaviour<EnemyMove>();
    }
} 
