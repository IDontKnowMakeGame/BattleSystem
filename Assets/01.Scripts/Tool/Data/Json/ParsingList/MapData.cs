using Actors.Characters.Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Floor
{
    Tutorial = 0,
    Lobby = 1,
    KHJScene1 = 2,

}

[Serializable]
public class MapInfo
{
    public Floor floor;
    public bool isBossKill = false;
    public List<int> onCristalList = new List<int>();
    public List<int> openChestList = new List<int>();
    public List<int> openDoorList = new List<int>();
    public List<int> brokenWallList = new List<int>();
    public List<EnemyType> killUniqueEnemy = new List<EnemyType>();
}

public class MapData
{
    public Floor currentFloor = 0;

    public List<MapInfo> mapData = new List<MapInfo>();

}
