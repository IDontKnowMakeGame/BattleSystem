using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnger : SliderUI
{
    public override void Start()
    {
        addflag = EventFlag.AddAnger;
        base.Start();
    }
}
