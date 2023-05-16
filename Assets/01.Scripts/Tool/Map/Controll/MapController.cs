﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Actors.Bases;
using Actors.Characters.Enemy;
using Blocks;
using Core;
using Managements;
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
        private static GameObject[] enemies;
        [MenuItem("Tools/MapController")]
        public static void ShowWindow()
        {
            blocks = MapManager.GetBlockOnMap();
            rooms = MapManager.GetRoomsOnMap();
            var objcets = Resources.LoadAll("Prefabs/Enemies");
            enemies = new GameObject[objcets.Length];
            for (int i = 0; i < objcets.Length; i++)
            {
                enemies[i] = (GameObject)objcets[i];
            }
            MapController window = (MapController)EditorWindow.GetWindow(typeof(MapController));
            window.Show();
        }

        private void OnEnable()
        {
            blocks = MapManager.GetBlockOnMap();
            rooms = MapManager.GetRoomsOnMap();
            var objcets = Resources.LoadAll("Prefabs/Enemies");
            enemies = new GameObject[objcets.Length];
            for (int i = 0; i < objcets.Length; i++)
            {
                enemies[i] = (GameObject)objcets[i];
            }
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
            CheckCharacter();
            InputHandle();  
        }

        private void CheckCharacter()
        {
            foreach (var block in blocks)
            {
                var thisRect = new Rect(blockPosDic[block], new Vector2(width, height));
                if (thisRect.Contains(Event.current.mousePosition))
                {
                    var mousePos = Event.current.mousePosition;
                    var characters = GameManagement.Instance.SpawnCharacters.Where(x => x.Position == block.transform.position.SetY(1));
                    var character = characters.FirstOrDefault();
                    if(character == null)
                        continue;
                    var name = character.Prefab.name;
                    GUI.Box(new (mousePos.x, mousePos.y, name.Length * 10, 20), name);
                }
            }
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
                    
                    if(x < blockPos.x + width && x2 > blockPos.x - width && y < blockPos.y + height && y2 > blockPos.y - height)
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
                if(GameManagement.Instance.SpawnCharacters.Any(x => x.Position == block.transform.position.SetY(1)))
                        color = Color.white;
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
            var roomListRect = new Rect((areas[1].x) * width + 375, height * 3, 150, height * 82);
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
                        selectedBlocks.Add(child.GetComponent<Block>());
                    }
                }
            }
            GUI.EndScrollView();
            
            var enemyListRect = new Rect((areas[1].x) * width + 50, height * 42, 300, height * 43);
            GUI.Box(enemyListRect, "");
            Vector2 scrollPos2 = Vector2.zero;
            scrollPos2 = GUI.BeginScrollView(enemyListRect, scrollPos2, new Rect(0, 0, 300, height * 43));
            var enemyList = enemies.ToList();
            var listHeight = 0f;
            foreach (var enemy in enemyList)
            {
                listHeight = enemyList.IndexOf(enemy) * height * 2;
                var enemyRect = new Rect(0, listHeight, 300, height * 2);
                if (GUI.Button(enemyRect, enemy.name))
                {
                    foreach (var block in selectedBlocks)
                    {
                        if (GameManagement.Instance.SpawnCharacters.Any(x => x.Position == block.transform.position.SetY(1)))
                        {
                            continue;
                        }
                        var spawnCharacter = new SpawnCharacter
                        {
                            Position = block.transform.position.SetY(1),
                            Prefab = enemy
                        };
                        GameManagement.Instance.SpawnCharacters.Add(spawnCharacter);
                    }
                    selectedBlocks.Clear();
                }
            }
            listHeight += height * 2;
            var enemyRect2 = new Rect(0, listHeight, 300, height * 2);
            if (GUI.Button(enemyRect2, "Clear"))
            {
                foreach (var block in selectedBlocks)
                {
                    GameManagement.Instance.SpawnCharacters.Remove(GameManagement.Instance.GetSpawnCharacter(block.transform.position.SetY(1)));
                }
                selectedBlocks.Clear();
            }
            GUI.EndScrollView();
        }
    }
    #endif
}