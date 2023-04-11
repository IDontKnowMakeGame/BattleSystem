using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors.Characters;
using Core;

public class HaloOfGhost : Halo
{
    public override void Equiqment(CharacterActor actor)
    {
        InGame.Player.GetAct<PlayerStatAct>().Plus(StatType.MAXHP, 100);
        UIManager.Instance.InGame.ChanageMaxHP((int)InGame.Player.GetAct<PlayerStatAct>().ChangeStat.maxHP / 10);
        UIManager.Instance.InGame.ChangeCurrentHP(InGame.Player.GetAct<PlayerStatAct>().PercentHP());
    }

    public override void UnEquipment(CharacterActor actor)
    {
        InGame.Player.GetAct<PlayerStatAct>().Sub(StatType.MAXHP, 100);
        UIManager.Instance.InGame.ChanageMaxHP((int)InGame.Player.GetAct<PlayerStatAct>().ChangeStat.maxHP / 10);
        UIManager.Instance.InGame.ChangeCurrentHP(InGame.Player.GetAct<PlayerStatAct>().PercentHP());
    }
}
