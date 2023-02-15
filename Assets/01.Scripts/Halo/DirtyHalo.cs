using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Base.Player;
using Core;
using Managements;
using Units.Behaviours.Unit;

public class DirtyHalo : Halo
{
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
            eventParam.unitParam.GetBehaviour<UnitStat>().Damaged(50, InGame.PlayerBase);
            GameObject obj = GameManagement.Instance.GetManager<ResourceManagers>().Instantiate("Damage");
            obj.GetComponent<DamagePopUp>().DamageText(50, eventParam.unitParam.Position);
        }
    }

    public override void OnDestroy()
    {
        var manager = Define.GetManager<EventManager>();
        manager?.StopListening(EventFlag.DirtyHalo, Using);
    }
    public override void OnApplicationQuit()
    {
        var manager = Define.GetManager<EventManager>();
        manager?.StopListening(EventFlag.DirtyHalo, Using);   
    }
}
