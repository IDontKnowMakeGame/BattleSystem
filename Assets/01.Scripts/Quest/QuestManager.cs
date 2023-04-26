using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors.Characters.Enemy;
using System;

public class QuestManager : MonoBehaviour
{
    private Dictionary<EnemyType, List<int>> CheckMonsterDic = new Dictionary<EnemyType, List<int>>();

    public static QuestManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AddMonsterKillMission(EnemyType.CrazyGhostActor, 1);
    }

    #region Add Mission
    public void AddMonsterKillMission(EnemyType type, int cnt)
    { 
        if(CheckMonsterDic.ContainsKey(type))
        {
            CheckMonsterDic[type].Add(cnt);
        }
        else
        {
            CheckMonsterDic[type] = new List<int>() { cnt };
        }

    }
    #endregion

    #region Check Mission
    public void CheckKillMission(EnemyType type)
    {
        if(CheckMonsterDic.ContainsKey(type))
        {
            List<int> check = CheckMonsterDic[type];

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
                CheckMonsterDic.Remove(type);
            else
                CheckMonsterDic[type] = check;
        }
    }
    #endregion
}
