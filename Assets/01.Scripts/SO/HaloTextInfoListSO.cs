using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/HaloTextInfoList")]
public class HaloTextInfoListSO : ScriptableObject
{
    public List<HaloTextInfoSO> list = new List<HaloTextInfoSO>();
}
