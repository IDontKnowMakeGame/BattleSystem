using Actors.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaloOfTime : Halo
{
    bool use = false;
    public override void Equiqment(CharacterActor actor)
    {
        use = true;
        Debug.Log("타임 스케일 착용 완료");
    }

    public override void UnEquipment(CharacterActor actor)
    {
        use = false;
    }

    public override void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) && use)
        {
            SetTimeScale(1.5f);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && use)
        {
            SetTimeScale(0.5f);
        }
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }
}
