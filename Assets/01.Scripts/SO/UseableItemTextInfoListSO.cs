using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UseableItemTextInfo
{
    public ItemID ItemID;
    public string name;
    [TextArea(0,5)]
    public string explanation;
}
[CreateAssetMenu(menuName ="SO/UseableItemTextInfoList")]
public class UseableItemTextInfoListSO : ScriptableObject
{
    public List<UseableItemTextInfo> list = new List<UseableItemTextInfo>();
}
