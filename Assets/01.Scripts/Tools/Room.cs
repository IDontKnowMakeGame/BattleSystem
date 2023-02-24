using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Managements.Managers;
using UnityEngine;

namespace _01.Scripts.Tools
{
    [Serializable]
    public class CameraArea
    {
        public int Index;
        public Vector3 StartPos;
        public Vector3 EndPos;
        public float Rotation = 0;
        public bool IsPlayerIn = false;
        public bool IsRoute = false;
        public bool IsFollow = false;
        
        public bool CheckPlayerIn()
        {
            if (InGame.PlayerBase.Position.IsInArea(StartPos, EndPos))
            {
                IsPlayerIn = true;
                Define.GetManager<FloorManager>().CurrentFloor.CurrentCameraArea = this;
                InGame.CameraMove.CurrentArea = this;
                return true;
            }
            else
            {
                IsPlayerIn = false;
            }

            return false;
        }
    }
    public class Room : MonoBehaviour
    {
        public int RoomIdx;
        public List<CameraArea> CameraAreas = new();
        public List<CameraArea> RouteAreas = new();

        private void Awake()
        {
            foreach (var area in CameraAreas)
            {
                if (InGame.PlayerBase.Position.IsInArea(area.StartPos, area.EndPos))
                {
                    area.IsPlayerIn = true;
                    Define.GetManager<FloorManager>().CurrentFloor.CurrentCameraArea = area;
                    InGame.CameraMove.CurrentArea = area;
                    return;
                }
            }
        }

        private void Update()
        {
            var areas = CameraAreas.Concat(RouteAreas);
            foreach (var area in areas)
            {
                area.CheckPlayerIn();
            }
        }

        private void OnDrawGizmos()
        {
            var areas = CameraAreas.Concat(RouteAreas);
            foreach (var area in areas)
            {
                Gizmos.color = area.IsRoute ? Color.magenta : Color.red;
                Gizmos.DrawWireCube(area.StartPos, Vector3.one);
                Gizmos.color = area.IsRoute ? Color.cyan : Color.blue;
                Gizmos.DrawWireCube(area.EndPos, Vector3.one);
                if(InGame.PlayerBase.Position.IsInArea(area.StartPos, area.EndPos))
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(area.StartPos, InGame.PlayerBase.Position);
                    Gizmos.DrawLine(area.EndPos, InGame.PlayerBase.Position);
                }
                else
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(area.StartPos, area.EndPos);
                }
            }
        }
    }
}