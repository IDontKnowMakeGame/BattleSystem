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
        playerStat.OriginStats.Hp += 100;
        playerStat.NowStats.Hp += 100;
        playerStat.ChangeHP();
    }

    public override void Exit()
    {
        playerStat.OriginStats.Hp -= 100;        
        playerStat.NowStats.Hp -= 100;
        playerStat.ChangeHP();
    }
}
