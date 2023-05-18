using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acts.Base;

[System.Serializable]
public class PlayerFlooding : Act
{
    [SerializeField]
    private int maxCnt = 7;
    [SerializeField]
    private float maxTimer = 5f;

    private int floodCount = 0;

    private bool warmMode = false;

    private float timer = 0f;

    public override void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            ChangeflooadCnt(1);
        }

        if (floodCount > 0)
        {
            if (warmMode)
            {
                timer += Time.deltaTime;

                if (timer >= maxTimer)
                {
                    floodCount--;
                    Debug.Log(floodCount + "감소입니다.");
                    timer = 0f;
                }
            }
        }
    }

    private void ChangeflooadCnt(int addCnt)
    {
        floodCount += addCnt;

        Debug.Log(floodCount + "입니다.");

        if(floodCount >= maxCnt)
        {
            ThisActor.GetAct<PlayerStatAct>().Die();
        }
    }

    public void ChangeWarmMode(bool mode)
    {
        Debug.Log(mode);
        if(warmMode != mode)
        {
            if(mode)
            {
                timer = 0f;
            }
            warmMode = mode;
        }
    }
}
