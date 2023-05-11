using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tool.Map.Rooms
{
    public class RoomDebug : MonoBehaviour
    {
        public List<Room> Rooms = new();

        
        [ContextMenu("Add Rooms")]
        public void AddRooms()
        {
            var rooms = GetComponentsInChildren<Room>();
            foreach (var room in rooms)
            {
                Rooms.Add(room);
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var room in Rooms)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(room.StartPos, Vector3.one);
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(room.EndPos, Vector3.one);
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube((room.StartPos + room.EndPos) / 2, Vector3.one);
            }
        }
    }
}