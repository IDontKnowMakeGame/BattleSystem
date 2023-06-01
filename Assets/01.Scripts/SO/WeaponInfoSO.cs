using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponInfoSO
{
    public ItemID ItemID;
    public string weaponNameText;
    [TextArea(0,20)]
    public string explanationText;
}
