using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderBrokenBase : EnemyBase
{
    protected override void Init()
    {
        AddBehaviour<EnemyFSM>();
    }
}
