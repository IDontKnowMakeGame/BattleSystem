using Core;
using Units.Base.Default;
using UnityEngine;

public class BlockBase : Unit
{
    #region Astar
    private GameObject tileOBJ;
    private int x;
    private int z;

    public bool isWalkable = false;
    public bool canEnemyEnter = true;
    public bool canBossEnter = false;
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
    private Unit _unitOnBlock;
    protected override void Init()
    {
        base.Init();
        tileOBJ = this.gameObject;
        isWalkable = true;
        Vector3 pos = transform.position;
        x = (int)pos.x;
        z = (int)pos.z;
    }
    
    public void UnitOnBlock(Unit unit = null)
    {
        _unitOnBlock = unit;
    }

    public bool IsUnitOn()
    {
        return _unitOnBlock != null;
    }

    public Unit GetUnit()
    {
        return _unitOnBlock;
    }
}