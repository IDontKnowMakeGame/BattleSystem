using System;
using System.Collections.Generic;
using Blocks;
using Managements.Managers;
using UnityEditor;
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

        private float space = 3f;
        private int idx = 1;

        private Vector3 mouseStartPos;
        private Vector3 mouseEndPos;
        private bool isMouseDrag;

        [MenuItem("Tools/sMapControllers")]
        public static void ShowWindow()
        {
            blocks = MapManager.GetDictWithBlocks();
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

            ShowToggleBtn(7);

            DragHandle();
        }

        private void ShowToggleBtn(float _height)
        {
            var toggleBtnRect = new Rect((mapPoses[1].x) * width + 50, height * idx * space, 300, height * _height);
            if (GUI.Button(toggleBtnRect, "Toggle"))
            {
                foreach (var block in selectedBlocks)
                {
                    block.Value.ToggleIsWalkable();
                }
                selectedBlocks.Clear();
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