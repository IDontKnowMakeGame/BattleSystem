using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Floor
{
    Lobby = 0,
    FirstFloor = 2,
    SencondFloor=4,
    ThirdFloor=8
}

public class SavePointData
{
    public Floor currentFloor = Floor.Lobby;

    public int openFloor = 0;

    //public bool firstFloor = true;
    //public bool secondFloor = false;
    //public bool thirdFloor = false;
}
