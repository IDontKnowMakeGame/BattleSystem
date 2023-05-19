using System;
using System.Collections.Generic;
using System.Linq;
using Blocks;
using Managements;
using Managements.Managers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.U2D.Animation;
using UnityEngine;

namespace Tool.Map.Controll
{
    public class MapControllers : EditorWindow
    {
        private float width;
        private float height;
        private static Dictionary<Vector3, Block> blocks;
        private static Rect mapRect;
        private static Vector3[] mapPoses;
        private Dictionary<Vector3, Block> selectedBlocks = new();
        private Dictionary<Vector3, Vector2> blockPosDic = new();
        private static Rooms.Room[] rooms;

        private float space;
        private int spaceIdx = 2;
        private int idx = 1;

        private Vector3 mouseStartPos;
        private Vector3 mouseEndPos;
        private bool isMouseDrag;

        [MenuItem("Tools/sMapControllers")]
        public static void ShowWindow()
        {
            blocks = MapManager.GetDictWithBlocks();
            rooms = MapManager.GetRoomsOnMap();
            mapPoses = blocks.GetMaxMinVector3s();
            
            MapControllers window = (MapControllers)EditorWindow.GetWindow(typeof(MapControllers));
            window.Show();
        }

        private void Update()
        {
            Repaint();
        }

        private void OnGUI()
        {
            GetArea();
            ShowBlocks();

            idx = 0;
            ShowToggleBtn(7);
            ShowRoomCreator(7);
            ShowSwitchCamera(7);

            CheckCharacter();
            DragHandle();
        }

        private void ShowToggleBtn(int _height)
        {
            var index = idx * height;
            var toggleBtnRect = new Rect((mapPoses[1].x) * width + 50, mapRect.y + index, 300, height * _height);
            if (GUI.Button(toggleBtnRect, "Toggle"))
            {
                foreach (var block in selectedBlocks)
                {
                    block.Value.ToggleIsWalkable();
                }
                selectedBlocks.Clear();
            }

            idx += _height + spaceIdx;
        }

        private string roomText;
        private void ShowRoomCreator(int _height)
        {
            var index = idx * height;
            var roomTextRect = new Rect((mapPoses[1].x) * width + 50, mapRect.y + index, 300, height * _height * 0.33f);
            roomText = GUI.TextField(roomTextRect, roomText);
            var roomBtnRect = new Rect((mapPoses[1].x) * width + 50, mapRect.y + index + (height * _height * 0.33f), 300, height * _height * 0.66f);
            if (GUI.Button(roomBtnRect, "Create Room"))
            {
                var parentTrm = new GameObject(roomText).transform;
                parentTrm.AddComponent<Rooms.Room>();
                var rootTrm = GameObject.Find("MapTiled").transform;
                parentTrm.SetParent(rootTrm);
                foreach (var block in selectedBlocks.Values)
                {
                    block.transform.SetParent(parentTrm);
                }
                selectedBlocks.Clear();
                rooms = MapManager.GetRoomsOnMap();
            }
            idx += _height + spaceIdx;
            
            var roomListRect = new Rect((mapPoses[1].x) * width + 375, height * 3, 150, position.height);
            GUI.Box(roomListRect, "");
            Vector2 scrollPos = Vector2.zero;
            scrollPos = GUI.BeginScrollView(roomListRect, scrollPos, new Rect(0, 0, 150, height * 82));
            if(rooms == null)
                rooms = MapManager.GetRoomsOnMap();
            var roomList = rooms.ToList();
            foreach (var room in roomList)
            {
                var roomRect = new Rect(0, roomList.IndexOf(room) * height * 2, 150, height * 2);
                if (GUI.Button(roomRect, room.name))
                {
                    selectedBlocks.Clear();
                    foreach (Transform child in room.transform)
                    {
                        selectedBlocks.Add(child.transform.position.SetY(0), child.GetComponent<Block>());
                    }
                }
            }
            GUI.EndScrollView();
        }

        private void ShowSwitchCamera(int _height)
        {
            var index = idx * height;
            var cameraBtnRect = new Rect((mapPoses[1].x) * width + 50, mapRect.y + index, 300, height * _height);
            if (GUI.Button(cameraBtnRect, "Toggle Switch Camera"))
            {
                foreach (var block in selectedBlocks)
                {
                    block.Value.ToggleHasSwitchCamera();
                    block.Value.isWalkable = block.Value.HasSwitchCamera;
                }
                selectedBlocks.Clear();
            }
        }

        private void CheckCharacter()
        {
            foreach (var block in blocks)
            {
                var thisRect = new Rect(blockPosDic[block.Key], new Vector2(width, height));
                if (thisRect.Contains(Event.current.mousePosition))
                {
                    var mousePos = Event.current.mousePosition;
                    var characters =
                        GameManagement.Instance.SpawnCharacters.Where(x =>
                            x.Position == block.Value.transform.position.SetY(1));
                    var character = characters.FirstOrDefault();
                    if (character == null)
                        continue;
                    var name = character.Prefab.name;
                    GUI.Box(new(mousePos.x, mousePos.y, name.Length * 10, 20), name);
                }
            }
        }

        Rect rect;
        private void DragHandle()
        {
            if (!hasFocus) return; 
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && !isMouseDrag)
            {
                mouseStartPos = Event.current.mousePosition;
                selectedBlocks.Clear();
            }

            if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
            {
                mouseEndPos = Event.current.mousePosition;
                var widths = mouseEndPos.x - mouseStartPos.x;
                var heights = mouseEndPos.y - mouseStartPos.y;
                rect = new Rect(mouseStartPos.x, mouseStartPos.y, widths, heights);
                isMouseDrag = true;


                foreach (var block in blocks)
                {
                    if (selectedBlocks.ContainsKey(block.Key))
                        continue;
                    var blockPos = blockPosDic[block.Key];
                    var x = mouseStartPos.x < mouseEndPos.x ? mouseStartPos.x : mouseEndPos.x;
                    var y = mouseStartPos.y < mouseEndPos.y ? mouseStartPos.y : mouseEndPos.y;

                    var min = new Vector2(x, y);
                    var size = new Vector2(Mathf.Abs(widths), Mathf.Abs(heights));
                    var dragRect = new Rect(min, size);
                    
                    if (dragRect.Contains(blockPos))
                    {
                        selectedBlocks.Add(block.Key, block.Value);
                    }
                }
            }
            if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
            {
                mouseStartPos = Vector3.zero;
                mouseEndPos = Vector3.zero;
                rect = new Rect();
                isMouseDrag = false;
                ShowBlocks();
            }

            var color = GUI.backgroundColor;
            var nextColor = Color.cyan;
            nextColor.a = 0.5f;
            GUI.backgroundColor = nextColor;
            GUI.Box(rect, "");
            GUI.backgroundColor = color;    
        }

        private void GetArea()
        {
            width = position.width / 160;
            height = position.height / 90;
            mapRect = new Rect(mapPoses[0].x * width, -mapPoses[0].z * height,
                (mapPoses[1].x) * width,
                (-mapPoses[1].z) * height);
            space = spaceIdx * height;
        }

        private void ShowBlocks()
        {
            GUI.Box(mapRect, "");

            foreach (var block in blocks)
            {
                var oldColor = GUI.backgroundColor;
                var color = Color.green;
                if (block.Value.isWalkable == false)
                    color = Color.red;
                if(block.Value.HasSwitchCamera)
                    color = Color.magenta;
                if(GameManagement.Instance.SpawnCharacters.Any(x => x.Position == block.Value.transform.position.SetY(1)))
                    color = Color.white;
                if(selectedBlocks.ContainsKey(block.Key))
                    color = Color.yellow;
                GUI.backgroundColor = color;
                var rect = new Rect(block.Key.x * width, -block.Key.z * height, width, height);

                if (GUI.Button(rect, ""))
                {
                    if(selectedBlocks.ContainsKey(block.Key))
                        selectedBlocks.Remove(block.Key);
                    else
                    {
                        selectedBlocks.Add(block.Key, block.Value);
                    }
                }
                
                GUI.backgroundColor = oldColor;
                blockPosDic[block.Key] = rect.position;
            }
        }
    }
}