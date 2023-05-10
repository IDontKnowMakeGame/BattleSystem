using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Blocks;
using Managements.Managers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Tool.Map.Controll
{
    #if UNITY_EDITOR
    public class MapController : EditorWindow
    {
        private Vector3[] areas;
        private Rect areaRect;
        private float width = 10;
        private float height = 10;
        private Vector3 mouseStartPos;
        private Vector3 mouseEndPos;
        private bool isMouseDrag;
        private string roomText;
        private static Block[] blocks;
        private static List<Block> selectedBlocks = new List<Block>();
        private static Dictionary<Block, Vector2> blockPosDic = new ();
        private static Rooms.Room[] rooms;
        [MenuItem("Tools/MapController")]
        public static void ShowWindow()
        {
            blocks = MapManager.GetBlockOnMap();
            rooms = MapManager.GetRoomsOnMap();
            MapController window = (MapController)EditorWindow.GetWindow(typeof(MapController));
            window.Show();
        }

        private void Update()
        {
            Repaint();
        }

        private void OnGUI()
        {
            
            if (blocks == null)
            {
                blocks = MapManager.GetBlockOnMap();
            }
            
            areas = blocks.GetMaxMinVector3s();
            GetArea();
            UpdateMap();

            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            
            UpdateButtons();
            UpdateInputField();
            UpdateList();
            InputHandle();  
        }
        
        Rect rect = new Rect();
        private void InputHandle()
        {
            if (!hasFocus) return; 
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && !isMouseDrag)
            {
                Debug.Log(1);
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
                    if (selectedBlocks.Contains(block))
                        continue;
                    var blockPos = blockPosDic[block];
                    var x = mouseStartPos.x < mouseEndPos.x ? mouseStartPos.x : mouseEndPos.x;
                    var x2 = mouseStartPos.x < mouseEndPos.x ? mouseEndPos.x : mouseStartPos.x;
                    var y = mouseStartPos.y < mouseEndPos.y ? mouseStartPos.y : mouseEndPos.y;
                    var y2 = mouseStartPos.y < mouseEndPos.y ? mouseEndPos.y : mouseStartPos.y;
                    
                    if(x < blockPos.x + 10 && x2 > blockPos.x - 10 && y < blockPos.y + 10 && y2 > blockPos.y - 10)
                        selectedBlocks.Add(block);
                }
            }

            if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
            {
                Debug.Log(rect);
                mouseStartPos = Vector3.zero;
                mouseEndPos = Vector3.zero;
                rect = new Rect();
                isMouseDrag = false;
                UpdateMap();
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
            areaRect = new Rect(areas[0].x * width, -areas[0].z * height,
                (areas[1].x) * width,
                (-areas[1].z) * height);
        }
        private void UpdateMap()
        {
            GUI.Box(areaRect, "");
            foreach (var block in blocks)
            {
                var oldColor = GUI.backgroundColor;
                var color = block.isWalkable ? Color.green : Color.red;
                if(block.HasSwitchCamera)
                    color = Color.magenta;
                if(selectedBlocks.Contains(block))
                    color = Color.yellow;
                GUI.backgroundColor = color;
                var x = block.transform.position.x * width;
                var y = -block.transform.position.z* height;
                Rect thisRect = new Rect(x, y , width, height);

                //GUI.Box(thisRect, "");
                
                if (GUI.Button(thisRect, ""))
                {
                    if (selectedBlocks.Contains(block))
                        selectedBlocks.Remove(block);
                    else
                        selectedBlocks.Add(block);
                }
                GUI.backgroundColor = oldColor;

                blockPosDic[block] = new Vector2(x, y);
            }
        }

        private void UpdateButtons()
        {
            var toggleBtnRect = new Rect((areas[1].x) * width + 50, height * 3, 300, height * 7);
            if (GUI.Button(toggleBtnRect, "Toggle"))
            {
                foreach (var block in selectedBlocks)
                {
                    block.ToggleIsWalkable();
                }
                selectedBlocks.Clear();
            }
            var roomTextRect = new Rect((areas[1].x) * width + 50, height * 12, 300, height * 2);
            roomText = GUI.TextField(roomTextRect, roomText);
            var roomBtnRect = new Rect((areas[1].x) * width + 50, height * 14, 300, height * 5);
            if (GUI.Button(roomBtnRect, "Create Room"))
            {
                var parentTrm = new GameObject(roomText).transform;
                parentTrm.AddComponent<Rooms.Room>();
                var rootTrm = GameObject.Find("MapTiled").transform;
                parentTrm.SetParent(rootTrm);
                foreach (var block in selectedBlocks)
                {
                    block.transform.SetParent(parentTrm);
                }
                selectedBlocks.Clear();
                rooms = MapManager.GetRoomsOnMap();
            }
            
            var cameraBtnRect = new Rect((areas[1].x) * width + 50, height * 22, 300, height * 7);
            if (GUI.Button(cameraBtnRect, "Toggle Switch Camera"))
            {
                foreach (var block in selectedBlocks)
                {
                    block.ToggleHasSwitchCamera();
                    block.isWalkable = block.HasSwitchCamera;
                }
                selectedBlocks.Clear();
            }
        }

        private void UpdateInputField()
        {
            if (selectedBlocks.Count > 0)
            {
                if (selectedBlocks[0].HasSwitchCamera)
                {
                    var switchTitleVerticalRect = new Rect((areas[1].x) * width + 50, height * 30, 150, height * 2);
                    var switchInputVerticalRect = new Rect((areas[1].x) * width + 200, height * 30, 150, height * 2);
                    GUI.Label(switchTitleVerticalRect, "Vertical Target Angle", EditorStyles.wordWrappedMiniLabel);
                    selectedBlocks[0].switchCamera.VerticalTargetAngle = EditorGUI.FloatField(switchInputVerticalRect, selectedBlocks[0].switchCamera.VerticalTargetAngle);
                    
                    var switchTitleHorizontalRect = new Rect((areas[1].x) * width + 50, height * 32, 150, height * 2);
                    var switchInputHorizontalRect = new Rect((areas[1].x) * width + 200, height * 32, 150, height * 2);
                    GUI.Label(switchTitleHorizontalRect, "Horizontal Target Angle", EditorStyles.wordWrappedMiniLabel);
                    selectedBlocks[0].switchCamera.HorizontalTargetAngle = EditorGUI.FloatField(switchInputHorizontalRect, selectedBlocks[0].switchCamera.HorizontalTargetAngle);
            
                    var switchTitleTargetFovRect = new Rect((areas[1].x) * width + 50, height * 34, 150, height * 2);
                    var switchInputTargetFovRect = new Rect((areas[1].x) * width + 200, height * 34, 150, height * 2);
                    GUI.Label(switchTitleTargetFovRect, "Target Fov", EditorStyles.wordWrappedMiniLabel);
                    selectedBlocks[0].switchCamera.TargetFov = EditorGUI.FloatField(switchInputTargetFovRect, selectedBlocks[0].switchCamera.TargetFov);
            
                    var switchTitleDurationRect = new Rect((areas[1].x) * width + 50, height * 36, 150, height * 2);
                    var switchInputDurationRect = new Rect((areas[1].x) * width + 200, height * 36, 150, height * 2);
                    GUI.Label(switchTitleDurationRect, "Duration", EditorStyles.wordWrappedMiniLabel);
                    selectedBlocks[0].switchCamera.Duration = EditorGUI.FloatField(switchInputDurationRect, selectedBlocks[0].switchCamera.Duration);
                }
            }
        }

        private void UpdateList()
        {
            var listRect = new Rect((areas[1].x) * width + 375, height * 3, 150, position.height);
            GUI.Box(listRect, "");
            Vector2 scrollPos = Vector2.zero;
            scrollPos = GUI.BeginScrollView(listRect, scrollPos, new Rect(0, 0, 150, position.height));
            var roomList = rooms.ToList();
            foreach (var room in roomList)
            {
                var roomRect = new Rect(0, roomList.IndexOf(room) * height * 2, 150, height * 2);
                if (GUI.Button(roomRect, room.name))
                {
                    selectedBlocks.Clear();
                    foreach (Transform child in room.transform)
                    {
                        selectedBlocks.Add(child.GetComponent<Block>());
                    }
                }
            }
            GUI.EndScrollView();
        }
    }
    #endif
}