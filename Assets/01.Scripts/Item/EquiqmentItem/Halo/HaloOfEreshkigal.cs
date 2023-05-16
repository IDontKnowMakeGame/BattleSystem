using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors.Characters;
using Core;

public class HaloOfEreshkigal : Halo
{
    int cnt;

    public override void Equiqment(CharacterActor actor)
    {
        cnt = 0;
        Define.GetManager<EventManager>().StartListening(EventFlag.HaloOfEreshkigal, Using);
    }

    public override void UnEquipment(CharacterActor actor)
    {
        Define.GetManager<EventManager>()?.StopListening(EventFlag.HaloOfEreshkigal, Using);
        InGame.Player.GetAct<PlayerStatAct>().Sub(StatType.ATK, 5 * cnt);
    }

    protected override void Using(EventParam eventParam)
    {
        if (eventParam.boolParam)
        {
            InGame.Player.GetAct<PlayerStatAct>().Plus(StatType.ATK, 5);
            cnt++;
        }
        else
        {
            if (cnt > 0)
            {
                if (eventParam.stringParam == "Die")
                {
                    InGame.Player.GetAct<PlayerStatAct>().Sub(StatType.ATK, 5 * cnt);
                    cnt = 0;
                }
                else
                {
                    InGame.Player.GetAct<PlayerStatAct>().Sub(StatType.ATK, 5);
                    cnt--;
                }
            }
        }
    }
}
