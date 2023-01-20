using System.Collections;
using System.Collections.Generic;
using Core;
using Managements.Managers;
using Unit.Block;
using UnityEngine;

public class BlockBase : Units.Base.Units
{
    [SerializeField] private BlockRender blockRender;
    private Units.Base.Units _unitOnBlock;
    protected override void Init()
    {
        Define.GetManager<MapManager>().AddBlock(this);
        AddBehaviour<BlockRender>(blockRender);
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
