using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAdrenaline : SliderUI
{
    public override void Awake()
    {
        addflag = EventFlag.AddAdrenaline;
        base.Awake();
    }
}
