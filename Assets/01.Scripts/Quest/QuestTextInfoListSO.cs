using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestTextInfo
{
    public QuestName QuestName;
    public string name;
    public string description;
}

[CreateAssetMenu(menuName ="SO/QuestTextInfoListSO")]
public class QuestTextInfoListSO : ScriptableObject
{
    public List<QuestTextInfo> list;
}
