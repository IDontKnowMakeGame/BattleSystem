using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAdrenaline : SliderUI
{
    public override void Start()
    {
        addflag = EventFlag.AddAdrenaline;
        base.Start();
    }
}
