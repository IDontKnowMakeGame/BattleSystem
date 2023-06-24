using Actors.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class HaloOfTime : Halo
{
    public static float currentTime = 1f;
    public override void Init()
    {
        base.Init();
        
    }
    bool use = false;
    public override void Equiqment(CharacterActor actor)
    {
        use = true;
        currentTime = 2f;
        EventParam eventParam = new EventParam();
        eventParam.floatParam = currentTime;
        SetTimeScale(eventParam);
        Define.GetManager<EventManager>()?.StartListening(EventFlag.HaloOfTime, SetTimeScale);
    }

    public override void UnEquipment(CharacterActor actor)
    {
        currentTime = 1f;
        EventParam eventParam = new EventParam();
        eventParam.floatParam = currentTime;
        SetTimeScale(eventParam);
        Define.GetManager<EventManager>()?.StopListening(EventFlag.HaloOfTime, SetTimeScale);
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
        {
            Time.timeScale = eventParam.floatParam;
            currentTime = eventParam.floatParam;
            Debug.Log($"Time Set : {eventParam.floatParam}");
        }
            
    }
}
