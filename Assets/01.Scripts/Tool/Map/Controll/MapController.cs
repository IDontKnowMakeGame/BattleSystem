using System;
using Blocks;
using Managements.Managers;
using UnityEditor;
using UnityEngine;

namespace Tool.Map.Controll
{
    public class MapController : EditorWindow
    {
        private float width = 10;
        private float height = 10;
        private static Block[] blocks;
        [MenuItem("Tools/MapController")]
        public static void ShowWindow()
        {
            blocks = MapManager.GetBlockOnMap();
            MapController window = (MapController)EditorWindow.GetWindow(typeof(MapController));
            foreach (var vector in blocks.GetMaxMinVector3s())
            {
                Debug.Log(vector);
            }
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            width = position.width / 160;
            height = position.height / 90;
            Texture2D myTexture= Resources.Load("Image/Box") as Texture2D;
            var areas = blocks.GetMaxMinVector3s();
            ;
            var areaRect = new Rect(areas[0].x * width, -areas[0].z * height,
                (areas[1].x) * width,
                (-areas[1].z) * height);
            GUI.Box(areaRect, "");
            foreach (var block in blocks)
            {
                var oldColor = GUI.backgroundColor;
                GUI.backgroundColor = block.isWalkable ? Color.green : Color.red;
                Rect thisRect = new Rect(block.transform.position.x * width, -block.transform.position.z * height, width, height);
                if (GUI.Button(thisRect, ""))
                {
                    block.ToggleIsWalkable();
                }
                GUI.backgroundColor = oldColor;
            }
        }
    }
}