using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

[CreateAssetMenu(fileName = "clips Data", menuName = "Scriptable Object/clips Data")]
public class Clips : ScriptableObject
{
    public List<AnimeClip> clips;
}
