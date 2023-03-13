using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Halo : Item
{
    protected float percent = 0;
    //protected actorst playerStat;

    public virtual void Init()
    {
        //playerStat = InGame.PlayerBase.GetBehaviour<PlayerStat>();
    }

    public virtual void Exit() { }

    protected bool ConditionCheck()
    {
        int random = Random.Range(1, 101);

        if (random <= percent)
        {
            return true;
        }
        return false;
    }

    public virtual void OnDestroy()
    {

    }
    public virtual void OnApplicationQuit()
    {

    }
}
