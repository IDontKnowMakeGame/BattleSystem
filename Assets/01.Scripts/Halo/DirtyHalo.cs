using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Base.Player;
using Core;
using Units.Base.Unit;

public class DirtyHalo : Halo
{
    private PlayerAttack playerAttack;
    private bool trigger = false;

    public override void Init()
    {
        base.Init();
        playerAttack = InGame.PlayerBase.GetBehaviour<PlayerAttack>();
        percent = 50;
    }

    protected override void Using()
    {
        if (!InGame.PlayerBase.State.HasFlag(BaseState.Attacking) && !trigger)
        {
            trigger = true;
            if (ConditionCheck())
            {
                Debug.Log("µ¥¹ÌÁö 50");
            }
        }
        else if (InGame.PlayerBase.State.HasFlag(BaseState.Attacking) && trigger)
            trigger = false;
    }
}
