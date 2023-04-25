using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum QuestName
{
    none = 0,
    WeaponGift,
    FirstFloorBossKill,
    FallenAngel,
}
[Serializable]
public class QuestInfo
{
    public QuestName questName = QuestName.none;
    public bool IsComplate = false;
    public int rewardfeather = 0;
    public List<ItemID> rewords = new List<ItemID>();
    public List<QuestName> openQuest = new List<QuestName>();
}

[Serializable]
public class QuestData
{
    public int currentQuestLine = 0;
    public List<QuestInfo> quests = new List<QuestInfo>();
}
