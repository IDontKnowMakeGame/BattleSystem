using Actors.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class HaloOfTime : Halo
{
    bool use = false;
    public override void Equiqment(CharacterActor actor)
    {
        use = true;
        Define.GetManager<EventManager>().StartListening(EventFlag.HaloOfTime, Using);
        Debug.Log("타임 스케일 착용 완료");
    }

    public override void UnEquipment(CharacterActor actor)
    {
        EventParam eventParam = new EventParam();
        eventParam.floatParam = 1f;
        SetTimeScale(eventParam);
        Define.GetManager<EventManager>().StopListening(EventFlag.HaloOfTime, Using);
        use = false;
    }

    public override void Update()
    {
/*        if(Input.GetKeyDown(KeyCode.UpArrow) && use)
        {
            SetTimeScale(1.5f);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && use)
        {
            SetTimeScale(0.5f);
        }*/
    }

    public void SetTimeScale(EventParam eventParam)
    {
        if(use)
            Time.timeScale = eventParam.floatParam;
    }
}
