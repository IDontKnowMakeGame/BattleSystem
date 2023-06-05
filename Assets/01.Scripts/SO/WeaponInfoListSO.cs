using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/WeaponInfoList")]
public class WeaponInfoListSO : ScriptableObject
{
    public List<WeaponInfoSO> weapons = new List<WeaponInfoSO>();
}
