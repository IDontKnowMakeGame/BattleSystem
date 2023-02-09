using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Base.Player;
using Core;

public abstract class Halo : EquipmentItem
{
    protected float percent = 0;
    protected PlayerStat playerStat; 

    public override void Update()
    {
        base.Update();
        Using();
    }

    public virtual void Init() 
    {
        playerStat = InGame.PlayerBase.GetBehaviour<PlayerStat>();
    }
    protected virtual void Using() { }

    protected bool ConditionCheck()
    {
        int random = Random.Range(1, 101);

        if (random <= percent)
            return true;
        return false;
    }
}
