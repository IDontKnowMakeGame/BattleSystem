using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class ColliderStat
{
    private int _sizeX;
    private int _sizeZ;
    private int _offsetX;
    private int _offsetZ;

    public int SizeX => _sizeX;
    public int SizeZ => _sizeZ;
    public int OffsetX => _offsetX;
    public int OffsetZ => _offsetZ;

    public ColliderStat()
    {
        _sizeX = InGame.None;
        _sizeZ = InGame.None;
        _offsetX = InGame.None;
        _offsetZ = InGame.None;
    }

    public ColliderStat(int sizeX, int sizeZ, int offsetX, int offsetZ)
    {
        _sizeX = sizeX;
        _sizeZ = sizeZ;
        _offsetX = offsetX;
        _offsetZ = offsetZ;
    }
}

public class AttackInfo
{
    public AttackInfo()
    {
        wantDir = DirType.None;
    }

    private DirType wantDir;
    private int reachFrame;
    private Vector3 pressInput;

    private ColliderStat leftStat = new ColliderStat();
    private ColliderStat rightStat = new ColliderStat();
    private ColliderStat upStat = new ColliderStat();
    private ColliderStat downStat = new ColliderStat();

    public DirType WantDir => wantDir;

    public int ReachFrame
    {
        get => reachFrame;
        set => reachFrame = value;
    }

    public Vector3 PressInput
    {
        get => pressInput;
        set => pressInput = value;
    }

    public ColliderStat LeftStat
    {
        get => leftStat;
        set => leftStat = value;
    }

    public ColliderStat RightStat
    {
        get => rightStat;
        set => rightStat = value;
    }

    public ColliderStat UpStat
    {
        get => upStat;
        set => upStat = value;
    }

    public ColliderStat DownStat
    {
        get => downStat;
        set => downStat = value;
    }


    public void AddDir(DirType getDir)
    {
        wantDir |= getDir;
    }

    public void RemoveDir(DirType getDir)
    {
        wantDir &= getDir;
    }

    public void ResetDir()
    {
        wantDir = DirType.None;
    }

    public void AllDir()
    {
        wantDir = DirType.All;
    }

	public DirType DirTypes(Vector3 vec)
	{
		if (vec == Vector3.forward)
			return DirType.Up;
		else if (vec == Vector3.back)
			return DirType.Down;
		else if (vec == Vector3.left)
			return DirType.Left;
		else if (vec == Vector3.right)
			return DirType.Right;
		else
			return DirType.Down;
	}
}