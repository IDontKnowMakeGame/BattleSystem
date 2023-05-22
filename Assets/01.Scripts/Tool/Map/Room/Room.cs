using System;
using Core;
using UnityEngine;

namespace Tool.Map.Rooms
{
    [Serializable]
    public class Room : MonoBehaviour, IRoom
    {
        [field:SerializeField]
        public Vector3 StartPos { get; set; }
        [field:SerializeField]
        public Vector3 EndPos { get; set; }

        public Transform modelRoot = null;
    }
}