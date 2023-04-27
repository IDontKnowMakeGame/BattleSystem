using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMap : Map
{
    public override void UseItem()
    {
        UIManager.Instance.UIFirstFloorMap.ShowFirstFloorMap();
    }
}
