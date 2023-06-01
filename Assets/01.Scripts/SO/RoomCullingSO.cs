using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool.Map.Rooms;

[CreateAssetMenu(fileName = "RoomCullingData", menuName = "ScriptableObject/RoomCullingData")]
public class RoomCullingSO : ScriptableObject
{
    public List<Room> connectRoom;
    public Transform modelRoot = null;
}
