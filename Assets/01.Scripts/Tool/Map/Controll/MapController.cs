using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Actors.Characters;
using Blocks;
using Managements;
using Managements.Managers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Tool.Map.Controll
{
    public class MapController : EditorWindow
    {
        private float width;
        private float height;
        private static Dictionary<Vector3, Block> blocks;
        private static Rect mapRect;
        private static Vector3[] mapPoses;
        private Dictionary<Vector3, Block> selectedBlocks = new();
        private static Dictionary<Vector3, GameObject> meshes = new(); 
        private Dictionary<Vector3, Vector2> blockPosDic = new();
        private Transform mapModelRoot = null;
        public static Dictionary<Vector3, GameObject> SpawnCharacters = new();
        private static Rooms.Room[] rooms;
        private static GameObject[] enemies;

        private float space;
        private int spaceIdx = 2;
        private int idx = 1;

        private Vector3 mouseStartPos;
        private Vector3 mouseEndPos;
        private bool isMouseDrag;

        [MenuItem("Tools/MapController")]
        public static void ShowWindow()
        {
            Init();
            MapController window = (MapController)EditorWindow.GetWindow(typeof(MapController));
            window.Show();
        }

        public static void Init()
        {
            blocks = MapManager.GetDictWithBlocks();
            rooms = MapManager.GetRoomsOnMap();
            meshes = rooms.SelectMany(r => r.modelRoot.AllChildrenObjArray()).ToDictionary(r => r.transform.position.SetY(0));
            Debug.Log(meshes.Count);
            var objects = Resources.LoadAll("Prefabs/Enemies");
            enemies = new GameObject[objects.Length];
            for (int i = 0; i < objects.Length; i++)
            {
                enemies[i] = (GameObject)objects[i];
            }
            
            mapPoses = blocks.GetMaxMinVector3s();
            SpawnCharacters = GameManagement.Instance.SpawnCharacters.ToDictionary(x => x.Position.SetY(0), y => y.Prefab);
        }

        private void Update()
        {
            Repaint();
        }

        private void OnGUI()
        {
            if(blocks == null)
                Init();
            GetArea();
            ShowBlocks();

            idx = 0;
            ShowBlockList();
            ShowToggleBtn(7);
            ShowRoomCreator(7);
            ShowRoomController(3);
            ShowModelRemover(_height: 3);
            ShowRoomArea(3);
            ShowRoomMerge(3);
            ShowModelMerge(3);
            ShowSwitchCamera(7);
            ShowEnemyList();
            ShowInvisibleBtn(7);

            CheckCharacter();
            DragHandle();
        }

        Vector2 scrollPos3 = Vector2.zero;
        private void ShowBlockList()
        {
            var index = idx * height;
            var enemyList = enemies.ToList();
            var enemyListRect = new Rect((mapPoses[1].x) * width + 50, mapRect.y + index, 300, height * 2 * (enemyList.Count + 1));
            GUI.Box(enemyListRect, "");
            scrollPos2 = GUI.BeginScrollView(enemyListRect, scrollPos2, new Rect(0, 0, 300, height * (enemyList.Count + 1)));
            var listHeight = 0f;
            var blockRect = new Rect(0, listHeight, 300, height * 2);
            if (GUI.Button(blockRect, "Block List"))
            {
                selectedBlocks.Clear();
                selectedBlocks.AddRange(blocks.Where(b => b.Value is not EmptyBlock and not FallingBlock).Select(b => b.Value).ToDictionary(b => b.transform.position));
            }
            listHeight += height * 2;
            var emptyRect = new Rect(0, listHeight, 300, height * 2);
            if (GUI.Button(emptyRect, "EmptyBlock List"))
            {
                selectedBlocks.Clear();
                selectedBlocks.AddRange(blocks.Where(b => b.Value is EmptyBlock and not FallingBlock).Select(b => b.Value).ToDictionary(b => b.transform.position));
            }
            listHeight += height * 2;
            var fallingRect = new Rect(0, listHeight, 300, height * 2);
            if (GUI.Button(fallingRect, "FallingBlock List"))
            {
                selectedBlocks.Clear();
                selectedBlocks.AddRange(blocks.Where(b => b.Value is FallingBlock).Select(b => b.Value).ToDictionary(b => b.transform.position));
            }
            listHeight += height * 2;
            var enemyRect2 = new Rect(0, listHeight, 300, height * 2);
            if (GUI.Button(enemyRect2, "All"))
            {
                selectedBlocks.Clear();
                selectedBlocks.AddRange(blocks);
            }
            GUI.EndScrollView();
            idx += (enemyList.Count + spaceIdx) * 2;
        }

        private void ShowModelRemover(int _height)
        {
            var index = idx * height;
            var removeBtnRect = new Rect((mapPoses[1].x) * width + 50, mapRect.y + index, 300, height * _height);
            if (GUI.Button(removeBtnRect, "Remove Model"))
            {
                foreach (var block in selectedBlocks)
                {
                    Debug.Log(block.Key);
                    if(meshes.TryGetValue(block.Key.SetY(0), out var mesh))
                        DestroyImmediate(mesh);
                    Debug.Log(mesh);
                }
            }
            idx += _height * spaceIdx;
        }

        private void ShowModelMerge(int _height)
        {
            var index = idx * height;
            var mergeBtnRect = new Rect((mapPoses[1].x) * width + 50, mapRect.y + index, 300, height * _height);
            if (GUI.Button(mergeBtnRect, "Merge Model"))
            {
                var block_M = selectedBlocks.Values.Select(block => block.transform.Find("Anchor/Model"))
                    .Where(model => model != null).Select(x => x.GetComponent<ProBuilderMesh>());
                var combine = CombineMeshes.Combine(block_M.ToList());
                combine[0].transform.SetParent(mapModelRoot);
            }
            idx += _height * spaceIdx;
        }
        
        private void ShowRoomMerge(int _height)
        {
            var index = idx * height;
            var meregBtnRect = new Rect((mapPoses[1].x) * width + 50, mapRect.y + index, 300, height * _height);
            if (GUI.Button(meregBtnRect, "Merge Room"))
            {
                foreach (var room in rooms)
                {
                    var blocks_T = room.modelRoot.AllChildrenObjListT();
                        
                    var blocks_M = blocks_T.Select(x => x.GetComponent<ProBuilderMesh>()).ToList();
                    var combine = CombineMeshes.Combine(blocks_M);
                    foreach (var model_T in blocks_T)
                    {
                        DestroyImmediate(model_T.gameObject);
                    }
                        
                    combine[0].transform.SetParent(room.modelRoot);
                }

                selectedBlocks.Clear();
            }

            idx += _height;
        }

        private void ShowRoomArea(int _height)
        {
            var index = idx * height;
            var areaBtnRect = new Rect((mapPoses[1].x) * width + 50, mapRect.y + index, 300, height * _height);
            if (GUI.Button(areaBtnRect, "SetArea"))
            {
                foreach (var room in rooms)
                {
                    if (room.name == roomText)
                    {
                        var blocks_T = room.transform.AllChildrenObjListT().ToArray();
                        var minMax = blocks_T.GetMaxMinVector3s();
                        room.StartPos = minMax[0];
                        room.EndPos = minMax[1];
                        Init();
                        break;
                    }
                }

                selectedBlocks.Clear();
            }

            idx += _height;
        }

        private void ShowRoomController(int _height)
        {
            var index = idx * height;
            var addBtnRect = new Rect((mapPoses[1].x) * width + 50, mapRect.y + index, 150, height * _height);
            if (GUI.Button(addBtnRect, "Add Blocks"))
            {
                foreach (var room in rooms)
                {
                    if (room.name == roomText)
                    {
                        selectedBlocks.Values.ToList().ForEach(block => block.transform.SetParent(room.transform));
                        
                        var rootTrm = GameObject.Find("MapTiled").transform;
                        var modelParentTrm = GameObject.Find(roomText + "Model").transform;
                        foreach (var block in selectedBlocks.Values)
                        {
                            var modelTrm = block.transform.GetChild(0).GetChild(0);
                            var model = Instantiate(modelTrm, modelParentTrm);
                            model.position = block.transform.position;
                        }

                        
                        Init();
                        break;
                    }
                }

                selectedBlocks.Clear();
            }
            
            var removeBtnRect = new Rect((mapPoses[1].x) * width + 200, mapRect.y + index, 150, height * _height);
            if (GUI.Button(removeBtnRect, "Remove Room"))
            {
                foreach (var room in rooms)
                {
                    if (room.name == roomText)
                    {
                        DestroyImmediate(room.modelRoot.gameObject);
                        var root = room.transform.parent;
                        foreach (var block in room.transform.AllChildrenObjListT())
                        {
                            block.transform.SetParent(root);
                        }
                        DestroyImmediate(room.gameObject);
                        Init();
                        break;
                    }
                }

                selectedBlocks.Clear();
            }

            idx += _height;
        }

        private void ShowInvisibleBtn(int _height)
        {
            var index = idx * height;
            var invisibleBtnRect = new Rect((mapPoses[1].x) * width + 50, mapRect.y + index, 300, height * _height);
            if (GUI.Button(invisibleBtnRect, "Invisible"))
            {
                foreach (var block in selectedBlocks)
                {
                    var model = block.Value.transform.GetChild(0).GetChild(0).gameObject;
                    model.SetActive(!model.activeSelf);
                }
                selectedBlocks.Clear();
            }

            idx += _height + spaceIdx;
        }

        Vector2 scrollPos2 = Vector2.zero;
        private void ShowEnemyList()
        {
            var index = idx * height;
            var enemyList = enemies.ToList();
            var enemyListRect = new Rect((mapPoses[1].x) * width + 50, mapRect.y + index, 300, height * 2 * (enemyList.Count + 1));
            GUI.Box(enemyListRect, "");
            scrollPos2 = GUI.BeginScrollView(enemyListRect, scrollPos2, new Rect(0, 0, 300, height * (enemyList.Count + 1)));
            var listHeight = 0f;
            foreach (var enemy in enemyList)
            {
                listHeight = enemyList.IndexOf(enemy) * height * 2;
                var enemyRect = new Rect(0, listHeight, 300, height * 2);
                if (GUI.Button(enemyRect, enemy.name))
                {
                    foreach (var block in selectedBlocks)
                    {
                        if (SpawnCharacters.ContainsKey(block.Key))
                        {
                            continue;
                        }
                        var spawnCharacter = new SpawnCharacter
                        {
                            Position = block.Value.transform.position.SetY(1),
                            Prefab = enemy
                        };
                        GameManagement.Instance.SpawnCharacters.Add(spawnCharacter);
                        SpawnCharacters = GameManagement.Instance.SpawnCharacters.ToDictionary(x => x.Position.SetY(0), y => y.Prefab);
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
                    GameManagement.Instance.SpawnCharacters.Remove(GameManagement.Instance.GetSpawnCharacter(block.Value.transform.position.SetY(1)));
                }
                selectedBlocks.Clear();
                SpawnCharacters = GameManagement.Instance.SpawnCharacters.ToDictionary(x => x.Position.SetY(0), y => y.Prefab);
            }
            GUI.EndScrollView();
            idx += (enemyList.Count + spaceIdx) * 2;
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
                if (mapModelRoot == null)
                {
                    var mapModel = GameObject.Find("MapModel");
                    if(mapModel == null)
                        mapModel = new GameObject {name = "MapModel"};
                    mapModelRoot = mapModel.transform;
                    
                }

                var rootTrm = GameObject.Find("MapTiled").transform;
                var modelParentTrm = new GameObject(roomText + "Model").transform;
                Transform model = null;
                // foreach (var r in rooms)
                // {
                //     if (r.name == roomText)
                //     {
                //         modelParentTrm.SetParent(mapModelRoot);
                //         r.modelRoot = modelParentTrm;
                //         foreach (var block in selectedBlocks.Values)
                //         {
                //             var modelTrm = block.transform.GetChild(0).GetChild(0);
                //             model = Instantiate(modelTrm, modelParentTrm);
                //             model.position = block.transform.position;
                //         }
                //         selectedBlocks.Clear();
                //         return;
                //     }
                // }
                var parentTrm = new GameObject(roomText).transform;
                parentTrm.SetParent(rootTrm);
                var room = parentTrm.AddComponent<Rooms.Room>();
                foreach (var block in selectedBlocks.Values)
                {
                    block.transform.SetParent(parentTrm);
                    var modelTrm = block.transform.GetChild(0).GetChild(0);
                    model = Instantiate(modelTrm, modelParentTrm);
                    model.position = block.transform.position;
                }

                modelParentTrm.SetParent(mapModelRoot);
                room.modelRoot = modelParentTrm;
                selectedBlocks.Clear();
                rooms = MapManager.GetRoomsOnMap();
            }
            idx += _height;
            
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
                    roomText = room.name;
                    selectedBlocks.Clear();
                    foreach (Transform child in room.transform)
                    {
                        selectedBlocks.Add(child.transform.position.SetY(0), child.GetComponent<Block>());
                    }
                }
            }
            var roadRect = new Rect(0, roomList.Count * height * 2, 150, height * 2);
            
            if (GUI.Button(roadRect, "Road"))
            {
                roomText = null;
                selectedBlocks.Clear();
                var leftBlocks = GameObject.Find("MapTiled").transform.AllChildrenObjListT();
                foreach (var block in leftBlocks)
                {
                    if (block.gameObject.HasComponent<Block>())
                    {
                        selectedBlocks.Add(block.transform.position.SetY(0), block.GetComponent<Block>());
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

            idx += _height;
            index = idx * height;
            if (selectedBlocks.Count > 0)
            {
                var firstSelectedBlock = selectedBlocks.First().Value;
                if (firstSelectedBlock.HasSwitchCamera)
                {
                    var switchTitleVerticalRect = new Rect((mapPoses[1].x) * width + 50, mapRect.y + index, 150, height * 2);
                    var switchInputVerticalRect = new Rect((mapPoses[1].x) * width + 200, mapRect.y + index, 150, height * 2);
                    GUI.Label(switchTitleVerticalRect, "Vertical Target Angle", EditorStyles.wordWrappedMiniLabel);
                    firstSelectedBlock.switchCamera.VerticalTargetAngle = EditorGUI.FloatField(switchInputVerticalRect, firstSelectedBlock.switchCamera.VerticalTargetAngle);

                    idx += 2;
                    index = idx * height;

                    var switchTitleHorizontalRect = new Rect((mapPoses[1].x) * width + 50, mapRect.y + index, 150, height * 2);
                    var switchInputHorizontalRect = new Rect((mapPoses[1].x) * width + 200, mapRect.y + index, 150, height * 2);
                    GUI.Label(switchTitleHorizontalRect, "Horizontal Target Angle", EditorStyles.wordWrappedMiniLabel);
                    firstSelectedBlock.switchCamera.HorizontalTargetAngle = EditorGUI.FloatField(switchInputHorizontalRect, firstSelectedBlock.switchCamera.HorizontalTargetAngle);
                    
                    idx += 2;
                    index = idx * height;
            
                    var switchTitleTargetFovRect = new Rect((mapPoses[1].x) * width + 50, mapRect.y + index, 150, height * 2);
                    var switchInputTargetFovRect = new Rect((mapPoses[1].x) * width + 200, mapRect.y + index, 150, height * 2);
                    GUI.Label(switchTitleTargetFovRect, "Target Fov", EditorStyles.wordWrappedMiniLabel);
                    firstSelectedBlock.switchCamera.TargetFov = EditorGUI.FloatField(switchInputTargetFovRect, firstSelectedBlock.switchCamera.TargetFov);
            
                    idx += 2;
                    index = idx * height;
                    
                    var switchTitleDurationRect = new Rect((mapPoses[1].x) * width + 50, mapRect.y + index, 150, height * 2);
                    var switchInputDurationRect = new Rect((mapPoses[1].x) * width + 200, mapRect.y + index, 150, height * 2);
                    GUI.Label(switchTitleDurationRect, "Duration", EditorStyles.wordWrappedMiniLabel);
                    firstSelectedBlock.switchCamera.Duration = EditorGUI.FloatField(switchInputDurationRect, firstSelectedBlock.switchCamera.Duration);
                    idx += 2;
                }
            }
            
            idx += spaceIdx;
        }

        private void CheckCharacter()
        {
            foreach (var block in blocks)
            {
                var thisRect = new Rect(blockPosDic[block.Key], new Vector2(width, height));
                if (thisRect.Contains(Event.current.mousePosition))
                {
                    var mousePos = Event.current.mousePosition;
                    if(SpawnCharacters.ContainsKey(block.Key) == false)
                        continue;
                    var characters = SpawnCharacters[block.Key];
                    var name = characters.name;
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
            nextColor.a = 0.8f;
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
                if (block.Value is EmptyBlock)
                    color = Color.gray;
                if  (block.Value is FallingBlock)
                    color = Color.blue;
                if (block.Value.isWalkable == false)
                    color = Color.red;
                if(block.Value.HasSwitchCamera)
                    color = Color.magenta;
                if(SpawnCharacters.ContainsKey(block.Key))
                    color = Color.white;
                if(selectedBlocks.ContainsKey(block.Key))
                    color = Color.yellow;
                if (block.Value.transform.GetChild(0).GetChild(0).gameObject.activeSelf == false)
                    color.a = 0.5f;
                GUI.backgroundColor = color;
                var _rect = new Rect(block.Key.x * width, -block.Key.z * height, width, height);

                if (GUI.Button(_rect, ""))
                {
                    if(selectedBlocks.ContainsKey(block.Key))
                        selectedBlocks.Remove(block.Key);
                    else
                    {
                        selectedBlocks.Add(block.Key, block.Value);
                    }
                }
                
                GUI.backgroundColor = oldColor;
                blockPosDic[block.Key] = _rect.position;
            }
        }
    }
}