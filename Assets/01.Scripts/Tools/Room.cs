using System;
using System.Collections.Generic;
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
        public bool IsPlayerIn = false;
        
        public bool CheckPlayerIn()
        {
            if (InGame.PlayerBase.Position.IsInArea(StartPos, EndPos))
            {
                IsPlayerIn = true;
                Define.GetManager<FloorManager>().CurrentFloor.CurrentCameraArea = this;
                InGame.CameraMove.CurrentArea = this;
                return true;
            }

            return false;
        }
    }
    public class Room : MonoBehaviour
    {
        public int RoomIdx;
        public List<CameraArea> CameraAreas = new();

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
            foreach (var area in CameraAreas)
            {
                if (area.IsPlayerIn == true)
                    break;
                area.CheckPlayerIn();
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var area in CameraAreas)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(area.StartPos, Vector3.one);
                Gizmos.color = Color.blue;
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