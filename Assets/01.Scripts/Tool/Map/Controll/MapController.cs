using System;
using System.Collections.Generic;
using System.Linq;
using Blocks;
using Managements.Managers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Tool.Map.Controll
{
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
        [MenuItem("Tools/MapController")]
        public static void ShowWindow()
        {
            blocks = MapManager.GetBlockOnMap();
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
            var toggleBtnRect = new Rect((areas[1].x) * width + 50, height * 3, 100, 70);
            if (GUI.Button(toggleBtnRect, "Toggle"))
            {
                foreach (var block in selectedBlocks)
                {
                    block.ToggleIsWalkable();
                }
                selectedBlocks.Clear();
            }
            var roomTextRect = new Rect((areas[1].x) * width + 50, height * 11, 100, 20);
            roomText = GUI.TextField(roomTextRect, roomText);
            var roomBtnRect = new Rect((areas[1].x) * width + 50, height * 13, 100, 50);
            if (GUI.Button(roomBtnRect, "Create Room"))
            {
                var parentTrm = new GameObject(roomText).transform;
                var rootTrm = GameObject.Find("MapTiled").transform;
                parentTrm.SetParent(rootTrm);
                foreach (var block in selectedBlocks)
                {
                    block.transform.SetParent(parentTrm);
                }
                selectedBlocks.Clear();
            }
            
            var cameraBtnRect = new Rect((areas[1].x) * width + 50, height * 19, 100, 70);
            if (GUI.Button(cameraBtnRect, "Toggle\n Switch\n Camera"))
            {
                foreach (var block in selectedBlocks)
                {
                    block.ToggleHasSwitchCamera();
                    block.isWalkable = block.HasSwitchCamera;
                }
                selectedBlocks.Clear();
            }
        }
    }
}