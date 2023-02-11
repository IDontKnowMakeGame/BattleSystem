using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Base.Player;
using Core;
using Units.Base.Unit;

public class DirtyHalo : Halo
{
    private bool trigger = false;
    private bool UpDamage = false;

    public override void Init()
    {
        base.Init();
        percent = 50;
    }

    protected override void Using()
    {
        if (!InGame.PlayerBase.State.HasFlag(BaseState.Attacking) && !trigger)
        {
            trigger = true;
            if (ConditionCheck())
            {
                UpDamage = true;
                Debug.Log("데미지 50");
            }
        }
        else if (InGame.PlayerBase.State.HasFlag(BaseState.Attacking) && trigger)
        {
            trigger = false;
            UpDamage = false;
        }
    }

    public override void Exit()
    {
        if(UpDamage)
        {
            Debug.Log("데미지 50 감소");
            UpDamage = false;
        }
    }
}
