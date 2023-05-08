using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors.Characters;
using Core;

public class HaloOfDismantle : Halo
{
    public override void Equiqment(CharacterActor actor)
    {
        InGame.Player.GetAct<PlayerStatAct>().Sub(StatType.Weight, 1);
    }

    public override void UnEquipment(CharacterActor actor)
    {
        InGame.Player.GetAct<PlayerStatAct>().Plus(StatType.Weight, 1);
    }
}
