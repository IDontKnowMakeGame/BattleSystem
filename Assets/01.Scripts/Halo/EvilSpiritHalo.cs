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
        playerStat.NowStats.Hp += 100;
    }

    public override void Exit()
    {
        playerStat.NowStats.Hp -= 100;        
    }
}
