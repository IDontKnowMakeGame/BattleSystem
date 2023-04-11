using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeapon : MonoBehaviour
{
    private void Start()
    {
        Define.GetManager<DataManager>().AddItemInInventory(Data.ItemID.OldStraightSword);
        Define.GetManager<DataManager>().AddItemInInventory(Data.ItemID.OldGreatSword);
        Define.GetManager<DataManager>().AddItemInInventory(Data.ItemID.OldTwinSword);
        Define.GetManager<DataManager>().AddItemInInventory(Data.ItemID.OldSpear);
        Define.GetManager<DataManager>().AddItemInInventory(Data.ItemID.OldBow);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
