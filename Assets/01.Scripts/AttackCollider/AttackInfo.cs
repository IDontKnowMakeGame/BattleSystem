using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackInfo
{
    public AttackInfo()
    {
        sizeX = none;
        sizeZ = none;
        offsetX = none;
        offsetZ = none;
        wantDir = DirType.None;
    }

    private int sizeX;
    private int sizeZ;
    private int offsetX;
    private int offsetZ;
    private DirType wantDir;
    const int none = -987654321;

    public int SizeX
    {
        get => sizeX;
        set => sizeX = value;
    }
    public int SizeZ
    {
        get => sizeZ;
        set => sizeZ = value;
    }
    public int OffsetX
    {
        get => offsetX;
        set => offsetX = value;
    }
    public int OffsetZ
    {
        get => offsetZ;
        set => offsetZ = value;
    }

    public int None
    {
        get => none;
    }

    public DirType WantDir => wantDir;

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

    public void ResetInfo()
    {
        sizeX = none;
        sizeZ = none;
        offsetX = none;
        offsetZ = none;
        wantDir = DirType.None;
    }

    private void Start()
    {
        AddDir(DirType.Up);
        AddDir(DirType.Down);
    }
}
