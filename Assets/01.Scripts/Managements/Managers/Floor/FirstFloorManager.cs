using System;
using System.Collections.Generic;
using _01.Scripts.Tools;
using Core;
using UnityEngine;

namespace Managements.Managers.Floor
{
    
    public class FirstFloorManager : MonoBehaviour, IFloor
    {
        [field: SerializeField]
        public int FloorNumber { get; set; } = 0;
        [field: SerializeField]
        public Vector3 PlayerSpawnPos { get; set; } 
        [field: SerializeField]
        public Vector3 BossSpawnPos { get; set; }
        [field: SerializeField]
        public List<Vector3> BossArea { get; set; }
        [field: SerializeField]
        public CameraArea CurrentCameraArea { get; set; }

        public List<Room> Rooms = new();

        private void Awake()
        {
            Define.GetManager<FloorManager>().CurrentFloor = this;
        }
    }
}