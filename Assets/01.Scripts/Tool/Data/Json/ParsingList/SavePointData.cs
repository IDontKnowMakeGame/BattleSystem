using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Floor
{
    LOBBY = 0,
    FIRST = 2,
    SECOND = 4,
    THIRD = 8,
}

[System.Serializable]
public class SavePointData
{
    public Floor currentFloor = Floor.LOBBY;
}
