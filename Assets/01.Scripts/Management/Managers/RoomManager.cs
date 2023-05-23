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
        private Transform mapParent = null;

        public override void Awake()
        {
            base.Awake();
            mapParent = GameObject.FindGameObjectWithTag("Map")?.transform;
        }

        public void CurrentRoomSetting()
        {
            Room currentRoom = Define.GetManager<MapManager>().GetBlock(InGame.Player.Position).transform.parent.GetComponent<Room>();
            
            if(currentRoom != null)
            Debug.Log("�����:" + currentRoom.gameObject.name);
            
            Transform parentRoom = Define.GetManager<MapManager>().GetBlock(InGame.Player.Position).transform.parent.parent;

            if (currentRoom != null && saveRoom != currentRoom)
            {
                saveRoom = currentRoom;



                // ��� Room ������Ʈ ����
                foreach (Transform room in parentRoom)
                {
                    room.GetComponent<Room>()?.modelRoot.gameObject.SetActive(false);
                    
                }

                // ��� map ������Ʈ ����
                foreach (Transform map in mapParent)
                {
                    map.gameObject.SetActive(false);

                }

                // ���� ��� ����� �븸 Ű��
                currentRoom.modelRoot.gameObject.SetActive(true);
                currentRoom.roomObjs.gameObject.SetActive(true);
                foreach (Room connectRoom in currentRoom.connectRoom)
                {
                    connectRoom.modelRoot.gameObject.SetActive(true);
                    connectRoom.roomObjs.gameObject.SetActive(true);
                }

            }
        }
    }
}