using System.Collections;
using System.Collections.Generic;
using Core;
using Managements.Managers;
using UnityEngine;

public class BlockBase : Units.Base.Units
{
    protected override void Init()
    {
        Define.GetManager<MapManager>().AddBlock(this);
        base.Init();
    }
}
