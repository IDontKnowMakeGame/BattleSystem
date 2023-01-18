using System.Collections;
using System.Collections.Generic;
using Core;
using Managements.Managers;
using UnityEngine;

public class BlockBase : Units.Base.Units
{
    private Units.Base.Units _unitOnBlock;
    protected override void Init()
    {
        Define.GetManager<MapManager>().AddBlock(this);
        base.Init();
    }
    
    public void UnitOnBlock(Units.Base.Units unit = null)
    {
        _unitOnBlock = unit;
    }

    public bool IsUnitOn()
    {
            return _unitOnBlock != null;
    }

    public Units.Base.Units GetUnit()
    {
        return _unitOnBlock;
    }
}
