using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Tool.Map.Rooms;

namespace Managements.Managers
{
    public class RoomManager : Manager
    {

        private Room saveRoom = null;

        public void CurrentRoomSetting()
        {
            Room currentRoom = Define.GetManager<MapManager>().GetBlock(InGame.Player.Position).transform.parent.GetComponent<Room>();
            Transform parentRoom = Define.GetManager<MapManager>().GetBlock(InGame.Player.Position).transform.parent.parent;
            if (currentRoom != null && saveRoom != currentRoom)
            {
                saveRoom = currentRoom;

                // 모든 Room 오브젝트 끄기
                foreach (Transform room in parentRoom)
                {
                    room.gameObject.SetActive(false);
                }

                // 현재 룸과 연결된 룸만 키기
                currentRoom.gameObject.SetActive(true);
                foreach(Room connectRoom in currentRoom.connectRoom)
                {
                    connectRoom.gameObject.SetActive(true);
                }

            }
        }
    }
}