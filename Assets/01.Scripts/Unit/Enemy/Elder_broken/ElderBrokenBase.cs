using System.Collections;
using System.Collections.Generic;
using Unit.Enemy.Base;
using UnityEngine;

public class ElderBrokenBase : EnemyBase
{
    protected override void Init()
    {
        base.Init();
        AddBehaviour<EnemyFSM>();
        AddBehaviour<EnemyMove>();
    }
} 
