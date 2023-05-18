using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acts.Base;

[System.Serializable]
public class PlayerFlooding : Act
{
    [SerializeField]
    private int maxCnt = 7;

    private int floodCount = 0;

    public override void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            ChangeflooadCnt(1);
        }
    }

    private void ChangeflooadCnt(int addCnt)
    {
        floodCount += addCnt;

        Debug.Log(floodCount + "ÀÔ´Ï´Ù.");

        if(floodCount >= maxCnt)
        {
            ThisActor.GetAct<PlayerStatAct>().Die();
        }
    }
}
