using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnger : SliderUI
{
    public override void Awake()
    {
        addflag = EventFlag.AddAnger;
        base.Awake();
    }
}
