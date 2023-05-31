using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponInfo
{
    public ItemID ItemID;
    public string explanationText;
}
[CreateAssetMenu(menuName = "SO/WeaponInfoList")]
public class WeaponInfoListSO : ScriptableObject
{
    public List<WeaponInfo> weapons = new List<WeaponInfo>();
}
