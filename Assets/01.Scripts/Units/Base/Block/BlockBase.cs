using System.Collections;
using System.Collections.Generic;
using Core;
using Managements.Managers;
using Unit.Block;
using UnityEngine;

public class BlockBase : Units.Base.Units
{
    #region Astar
    private GameObject tileOBJ;
    private int x;
    private int z;

    public bool isWalkable = false;
    private int g;
    private int h;

    private BlockBase parent;


    public GameObject TileOBJ { get => tileOBJ; }
    public int X { get => x; }
    public int Z { get => z; }
    public int G
    {
        get
        {
            return g;
        }
        set
        {
            g = value;
        }
    }
    public int H
    {
        get
        {
            return h;
        }
        set
        {
            h = value;
        }
    }
    public BlockBase Parent
    {
        get
        {
            return parent;
        }
        set
        {
            parent = value;
        }
    }

    public bool ChangeTile
    {
        set
        {
            isWalkable = value;
        }
    }

    public int fCost
    {
        get { return g + h; }
    }
    #endregion

    [SerializeField] private BlockRender blockRender;
    private Units.Base.Units _unitOnBlock;
    protected override void Init()
    {
        base.Init();
        Define.GetManager<MapManager>().AddBlock(this);
        AddBehaviour(blockRender);
        tileOBJ = this.gameObject;
        isWalkable = true;
        Vector3 pos = transform.position;
        x = (int)pos.x;
        z = (int)pos.z;
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
