using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : SliderUI
{
    public override void Start()
    {
        addflag = EventFlag.AddPlayerHP;
        base.Start();
    }
}
