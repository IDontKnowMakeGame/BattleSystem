using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors.Characters.Enemy;
using System;
using Core;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    private List<RoomSO> allRoomSo;

    private Dictionary<string, RoomSO> allRoomSODic = new Dictionary<string, RoomSO>();
    private Dictionary<Vector3, string> roomData = new Dictionary<Vector3, string>(); 
    
    private Dictionary<EnemyType, List<int>> checkMonsterDic = new Dictionary<EnemyType, List<int>>();
    private HashSet<RoomSO> checkRoom = new HashSet<RoomSO>();

    public static QuestManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        RoomSet();
    }

    private void Start()
    {
        AddMonsterKillMission(EnemyType.CrazyGhostActor, 1);
        AddRoomMission("S10");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(roomData[InGame.Player.Position.SetY(0)]);
        }
    }

    private void RoomSet()
    {
        foreach (RoomSO currentSO in allRoomSo)
        {
            allRoomSODic.Add(currentSO.name, currentSO);

            Debug.Log(currentSO.startPos.z);
            for (var z = currentSO.startPos.z; z <= currentSO.endPos.z; z += 1)
            {
                for (var x = currentSO.startPos.x; x <= currentSO.endPos.x; x += 1)
                {
                    Debug.Log(new Vector3(x, 0, z));
                    roomData.Add(new Vector3(x, 0, z), currentSO.name);
                }
            }
        }
    }

    #region Add Mission
    public void AddMonsterKillMission(EnemyType type, int cnt)
    { 
        if(checkMonsterDic.ContainsKey(type))
        {
            checkMonsterDic[type].Add(cnt);
        }
        else
        {
            checkMonsterDic[type] = new List<int>() { cnt };
        }

    }

    public void AddRoomMission(string roomCode)
    {
        checkRoom.Add(allRoomSODic[roomCode]);
    }
    #endregion

    #region Check Mission
    public void CheckKillMission(EnemyType type)
    {
        if(checkMonsterDic.ContainsKey(type))
        {
            List<int> check = checkMonsterDic[type];

            for(int i = 0; i < check.Count; i++)
            {
                check[i]--;

                if(check[i] == 0)
                {
                    check.Remove(i);
                    Debug.Log("미션 클리어!");
                    i--;
                }
            }

            if (check.Count == 0)
                checkMonsterDic.Remove(type);
            else
                checkMonsterDic[type] = check;
        }
    }

    public void CheckRoomMission(Vector3 pos)
    {
        string result = string.Empty;
        if (roomData.TryGetValue(pos.SetY(0), out result))
        {
            if(checkRoom.Contains(allRoomSODic[result]))
            {
                Debug.Log("Mission 성공");
                checkRoom.Remove(allRoomSODic[result]);
            }
        }
    }
    #endregion
}
