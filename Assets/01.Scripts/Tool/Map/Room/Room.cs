using System;
using Core;
using UnityEngine;
using System.Collections.Generic;

namespace Tool.Map.Rooms
{
    [Serializable]
    public class Room : MonoBehaviour, IRoom
    {
        [field:SerializeField]
        public Vector3 StartPos { get; set; }
        [field:SerializeField]
        public Vector3 EndPos { get; set; }
        [field: SerializeField]
        public List<Room> connectRoom;

        public Transform modelRoot = null;
        public Transform roomObjs = null;
    }
}