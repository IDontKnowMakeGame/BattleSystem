using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToolTipList", menuName = "ScriptableObject/ToolTipList")]
public class ToolTipListSO : ScriptableObject
{
    public List<string> tooltipList;
}
