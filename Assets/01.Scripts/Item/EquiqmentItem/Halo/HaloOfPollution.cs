using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors.Characters;
using Core;

public class HaloOfPollution : Halo
{
    private int damage;

    public override void Equiqment(CharacterActor actor)
    {
        percent = 50;
        damage = 50;
        Define.GetManager<EventManager>().StartListening(EventFlag.PollutionHalo, Using);
    }

    public override void UnEquipment(CharacterActor actor)
    {
        Define.GetManager<EventManager>()?.StopListening(EventFlag.PollutionHalo, Using);
    }


    public override void Update()
    {

    }

    protected override void Using(EventParam eventParam)
    {
        if (ConditionCheck())
        {
            Debug.Log(eventParam.unit);
            eventParam.unit.GetAct<CharacterStatAct>().Damage(damage, InGame.Player);
        }
    }
}