using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors.Characters;
using Core;
using Acts.Characters.Enemy;

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
            eventParam.unit?.GetAct<EnemyStatAct>()?.Damage(damage, InGame.Player);
        }
    }
}