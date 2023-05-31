using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "SO/HaloTextInfo")]
public class HaloTextInfoSO : ScriptableObject
{
    public ItemID ItemID;
    public string nameText;
    public string skillNameText;
    [TextArea(0, 10)]
    public string skillExplanationText;
    [TextArea(0,10)]
    public string haloExplanationText;
}