using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuestData
{
    public List<QuestName> readyQuestList = new List<QuestName>() { QuestName.WeaponGift };
    public List<QuestName> openQuestList = new List<QuestName>();
    public List<QuestName> readyClearQuestList = new List<QuestName>();
    public List<QuestName> clearQuestList = new List<QuestName>();
}
