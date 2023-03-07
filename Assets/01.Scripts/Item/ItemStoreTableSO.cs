using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemPrice
{
    public ItemID itemID;
    public int price;
}

[CreateAssetMenu(menuName ="SO/ItemStore/ItemTable")]
public class ItemStoreTableSO : ScriptableObject
{
    public List<ItemPrice> table;
}
