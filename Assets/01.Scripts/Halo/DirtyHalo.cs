using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Base.Player;
using Core;

public class DirtyHalo : Halo
{
    private PlayerAttack playerAttack;

    public override void Init()
    {
        base.Init();
        playerAttack = InGame.PlayerBase.GetBehaviour<PlayerAttack>();
        percent = 5;
    }

    protected override void Using()
    {
        if(playerAttack.isAttack && ConditionCheck())
        {
            Debug.Log("µ¥¹ÌÁö 50");
        }
    }
}
