using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum QuestName
{
    none = 0,
    FirstFloorBossKill = 1,
    FallenAngel = 2,
}
[Serializable]
public class QuestInfo
{
    public QuestName questName = QuestName.none;
    public bool IsComplate = false;
    public List<ItemID> rewords = new List<ItemID>();
}

[Serializable]
public class QuestData
{
    public int currentQuestLine = 0;
    public List<QuestInfo> quests = new List<QuestInfo>();
}
