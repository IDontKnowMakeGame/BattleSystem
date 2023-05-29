using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors.Characters.Enemy;
using System;
using Core;
using Data;

struct QuestValue
{
    public RoomSO roomSO;
    public List<int> intList;
    public List<QuestName> myQuest;
}

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    private List<RoomSO> allRoomSo;

    private Dictionary<string, QuestValue> allRoomSODic = new Dictionary<string, QuestValue>();

    private Dictionary<Vector3, string> roomData = new Dictionary<Vector3, string>(); 
    
    private Dictionary<EnemyType, QuestValue> checkMonsterDic = new Dictionary<EnemyType, QuestValue>();
    private HashSet<RoomSO> checkRoom = new HashSet<RoomSO>();
    private Dictionary<ItemID, QuestValue> checkHaveItem = new Dictionary<ItemID, QuestValue>();

    public static QuestManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        RoomSet();
    }

    private void Start()
    {
        StartOpenQuestCheck();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(checkMonsterDic[EnemyType.OldShade].intList.Count);
        }
    }

    public void StartOpenQuestCheck()
    {
        List<QuestName> openQuestList = DataManager.PlayerOpenQuestData_.openQuestList;

        foreach(QuestName currentQuest in openQuestList)
        {
            QuestCheck(currentQuest);
        }
    }

    public void AddQuestCheck()
    {
        List<QuestName> openQuestList = DataManager.PlayerOpenQuestData_.openQuestList;
        QuestName currentQuest = openQuestList[openQuestList.Count - 1];

        QuestCheck(currentQuest);
    }

    private void QuestCheck(QuestName currentQuest)
    {
        Debug.Log("들어옴");
        switch (currentQuest)
        {
            case QuestName.FirstFloorBossKill:
                AddMonsterKillMission(currentQuest, EnemyType.CrazyGhostActor, 1);
                break;
            case QuestName.S10AreaEnter:
                AddRoomMission(currentQuest, "S10");
                break;
            case QuestName.Shade10Kill:
                AddMonsterKillMission(currentQuest, EnemyType.OldShade, 10);
                break;
            case QuestName.FallenAngelSupplyThing:
                AddHaveItemMission(currentQuest, ItemID.AngelWingFragment);
                break;

        }
    }

    private void RoomSet()
    {
        foreach (RoomSO currentSO in allRoomSo)
        {
            QuestValue newValue = new QuestValue();
            newValue.roomSO = currentSO;
            allRoomSODic.Add(currentSO.name, newValue);
            for (var z = currentSO.startPos.z; z <= currentSO.endPos.z; z += 1)
            {
                for (var x = currentSO.startPos.x; x <= currentSO.endPos.x; x += 1)
                {
                    roomData.Add(new Vector3(x, 0, z), currentSO.name);
                }
            }
        }
    }

    #region Add Mission
    public void AddMonsterKillMission(QuestName currentQuest, EnemyType type, int cnt)
    { 
        if(checkMonsterDic.ContainsKey(type))
        {
            checkMonsterDic[type].intList.Add(cnt);
            checkMonsterDic[type].myQuest.Add(currentQuest);
        }
        else
        {
            QuestValue quest = new QuestValue();
            quest.intList = new List<int>() { cnt };
            quest.myQuest = new List<QuestName> { currentQuest };
            checkMonsterDic.Add(type, quest);
        }

    }

    public void AddRoomMission(QuestName currentQuest, string roomCode)
    {
        QuestValue quest = allRoomSODic[roomCode];
        quest.myQuest = new List<QuestName> { currentQuest };
        allRoomSODic[roomCode] = quest;
        checkRoom.Add(allRoomSODic[roomCode].roomSO);
    }

    public void AddHaveItemMission(QuestName currentQuest, ItemID item)
    {
        if(!checkHaveItem.ContainsKey(item))
        {
            QuestValue quest = new QuestValue();
            quest.myQuest = new List<QuestName> { currentQuest };
            checkHaveItem.Add(item, quest);
        }
    }
    #endregion

    #region Check Mission
    public void CheckKillMission(EnemyType type)
    {
        Debug.Log(type + "입니다.");
        if(checkMonsterDic.ContainsKey(type))
        {
            QuestValue quest = checkMonsterDic[type];
            List<int> check = quest.intList;
            List<QuestName> questList = quest.myQuest;

            for(int i = 0; i < check.Count; i++)
            {
                check[i]--;


                if (check[i] == 0)
                {
                    check.RemoveAt(i);
                    Define.GetManager<DataManager>().ReadyClearQuest(questList[i]);
                    questList.RemoveAt(i);
                    i--;
                }
            }

            if (check.Count == 0)
                checkMonsterDic.Remove(type);
            else
            {
                quest.intList = check;
                checkMonsterDic[type] = quest;
            }
        }
    }

    public void CheckRoomMission(Vector3 pos)
    {
        string result = string.Empty;
        if (roomData.TryGetValue(pos.SetY(0), out result))
        {
            if(checkRoom.Contains(allRoomSODic[result].roomSO))
            {
                Define.GetManager<DataManager>().ReadyClearQuest(allRoomSODic[result].myQuest[0]);
                checkRoom.Remove(allRoomSODic[result].roomSO);
            }
        }
    }

    public void CheckHaveItemMission(ItemID itemID)
    {
        if(checkHaveItem.ContainsKey(itemID) && DataManager.HaveQuestItem(itemID))
        {
            QuestName myQuest = checkHaveItem[itemID].myQuest[0];
            Define.GetManager<DataManager>().ReadyClearQuest(myQuest);
            checkHaveItem.Remove(itemID);
        }
    }
    #endregion
}
