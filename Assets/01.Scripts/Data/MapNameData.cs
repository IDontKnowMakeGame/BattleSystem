using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/MapNamaData")]
public class MapNameData : ScriptableObject
{
    public List<string> tutorialMapName = new List<string>();
    public List<string> lobbyMapName = new List<string>();
    public List<string> firstMapName = new List<string>();
    public List<string> secondMapName = new List<string>();

}
