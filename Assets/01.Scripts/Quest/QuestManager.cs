using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors.Characters.Enemy;
using System;
using Core;

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
    private Dictionary<EnemyType, QuestValue> checkMonsterDic = new Dictionary<EnemyType, QuestValue>();

    private Dictionary<Vector3, string> roomData = new Dictionary<Vector3, string>(); 
    
    private HashSet<RoomSO> checkRoom = new HashSet<RoomSO>();

    public static QuestManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        RoomSet();
    }

    private void Start()
    {
        QuestCheck(QuestName.FirstFloorBossKill);
        QuestCheck(QuestName.S10AreaEnter);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(roomData[InGame.Player.Position.SetY(0)]);
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
        switch (currentQuest)
        {
            case QuestName.FirstFloorBossKill:
                AddMonsterKillMission(currentQuest, EnemyType.CrazyGhostActor, 1);
                break;
            case QuestName.S10AreaEnter:
                AddRoomMission(currentQuest, "S10");
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
    #endregion

    #region Check Mission
    public void CheckKillMission(EnemyType type)
    {
        if(checkMonsterDic.ContainsKey(type))
        {
            QuestValue quest = checkMonsterDic[type];
            List<int> check = quest.intList;
            List<QuestName> questList = quest.myQuest;

            for(int i = 0; i < check.Count; i++)
            {
                check[i]--;

                if(check[i] == 0)
                {
                    check.RemoveAt(i);
                    Define.GetManager<DataManager>().ReadyClearQuest(questList[i]);
                    questList.RemoveAt(i);
                    Debug.Log("�̼� Ŭ����!");
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
                Debug.Log("�̼� Ŭ����!");
                Define.GetManager<DataManager>().ReadyClearQuest(allRoomSODic[result].myQuest[0]);
                checkRoom.Remove(allRoomSODic[result].roomSO);
            }
        }
    }
    #endregion
}
