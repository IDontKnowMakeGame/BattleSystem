using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    public List<SaveItemData> inventoryInWeaponList;
    public List<SaveItemData> inventoryInHaloList;
    public List<SaveItemData> inventoryInUsableItemList;
    public List<SaveItemData> inventoryInQuestItemList;
}
