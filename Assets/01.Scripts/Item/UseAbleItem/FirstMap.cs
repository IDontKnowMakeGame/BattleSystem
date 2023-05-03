using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMap : Map
{
    public override bool UseItem()
    {
        UIManager.Instance.UIFirstFloorMap.ShowFirstFloorMap();
        return true;
    }
}
