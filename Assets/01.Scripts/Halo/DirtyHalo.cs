using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Base.Player;
using Core;
using Units.Base.Unit;

public class DirtyHalo : Halo
{
    bool UpDamage = false;
    public override void Init()
    {
        base.Init();
        percent = 50;
        Define.GetManager<EventManager>().StartListening(EventFlag.DirtyHalo, Using);
    }

    protected override void Using(EventParam eventParam)
    {
        if (ConditionCheck())
        {
            if(!UpDamage)
                playerStat.addstat.Atk += 50;
            UpDamage = true;
        }  
        else
        {
            Exit();
        }
    }

    public override void Exit()
    {
        if(UpDamage)
        {
            playerStat.addstat.Atk -= 50;
        }
        UpDamage = false;
    }

    public override void OnDestroy()
    {
        Define.GetManager<EventManager>().StopListening(EventFlag.DirtyHalo, Using);
    }
    public override void OnApplicationQuit()
    {
        Define.GetManager<EventManager>().StopListening(EventFlag.DirtyHalo, Using);    
    }
}
