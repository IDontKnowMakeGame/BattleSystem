using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class QuestItemTextInfo
{
    public ItemID ItemID;
    public string name;
    [TextArea(0,5)]
    public string description;
}

[CreateAssetMenu(menuName = "SO/QuestItemTextInfoList")]
public class QuestItemTextInfoListSO : ScriptableObject
{
    public List<QuestItemTextInfo> list = new List<QuestItemTextInfo>();
}
