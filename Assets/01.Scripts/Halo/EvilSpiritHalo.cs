using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Base.Player;
using Core;

public class EvilSpiritHalo : Halo
{ 
    public override void Init()
    {
        base.Init();
        playerStat = InGame.PlayerBase.GetBehaviour<PlayerStat>();
    }
}
