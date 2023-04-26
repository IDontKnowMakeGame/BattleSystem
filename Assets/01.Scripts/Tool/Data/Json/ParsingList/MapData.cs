using Actors.Characters.Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Floor
{
    Lobby = 0,
    First = 1,
    Second = 2,
    Third = 3,
}

[Serializable]
public class MapInfo
{
    public Floor floor;
    public bool isBossKill = false;
    public List<int> onCristalList = new List<int>();
    public List<int> openChestList = new List<int>();
    public List<EnemyType> killUniqueEnemy = new List<EnemyType>();
}

public class MapData
{
    public Floor currentFloor = 0;

    public List<MapInfo> mapData = new List<MapInfo>();

}
