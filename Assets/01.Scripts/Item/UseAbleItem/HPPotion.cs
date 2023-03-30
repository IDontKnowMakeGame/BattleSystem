using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Data;

public class HPPotion : UseAbleItem
{
    public override void UseItem()
    {
        InGame.Player.GetAct<PlayerStatAct>().Heal(100);
        UIManager.Instance.InGame.ChangeCurrentHP(InGame.Player.GetAct<PlayerStatAct>().PercentHP());
    }
}
