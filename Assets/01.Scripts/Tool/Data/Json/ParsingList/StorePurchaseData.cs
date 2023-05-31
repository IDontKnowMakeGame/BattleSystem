using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PurchaseItemInfoData
{
    public ItemID itemID;
    public int purchaseCnt;
}
public class StorePurchaseData
{
    public List<PurchaseItemInfoData> itemList = new List<PurchaseItemInfoData>();
}
