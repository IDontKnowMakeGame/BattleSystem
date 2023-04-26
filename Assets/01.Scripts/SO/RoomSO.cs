using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "ScriptableObject/RoomData")]
public class RoomSO : ScriptableObject
{
    public string name;
    public Vector3 startPos;
    public Vector3 endPos;
}
