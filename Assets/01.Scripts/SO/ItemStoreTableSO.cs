using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemPrice
{
    public ItemID itemID;
    public int price;
    public bool limitItem = false;
    public int limitCnt = 1;
}

[CreateAssetMenu(menuName = "SO/ItemStore/ItemTable")]
public class ItemStoreTableSO : ScriptableObject
{
    public List<ItemPrice> table;
}
