using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using UnityEditor;

public enum Mode
{
    StartPos,
    CenterPos
}

[Serializable]
public class GridObjects
{
    public GameObject[] tiles;
    public GameObject wall;
    public float offsetX = 1;
    public float offsetZ = 1;
    public float wallOffsetX = 0.5f;
    public float setWallY = -0.5f;
    public float wallOffsetZ = 0.5f;
}

#if UNITY_EDITOR
public class MapGeneration : EditorWindow
{
    private void OnEnable()
    {
        ScriptableObject target = this;
        so = new SerializedObject(target);
    }

    #region Editor
    public GameObject[] SetTiles;
    SerializedObject so;

    [MenuItem("Tools/MapGeneration")]
    public static void ShowWindow()
    {
        GetWindow<MapGeneration>("MapGeneration");
    }

    private void OnGUI()
    {
        GUILayout.Label("MapGeneration", EditorStyles.boldLabel);

        GUILayout.Label("URL Setting", EditorStyles.miniBoldLabel);
        InputURL = EditorGUILayout.TextField("URL(Until \"range=\"):", InputURL);

        GUILayout.Label("Tiles Setting", EditorStyles.miniBoldLabel);

        GUILayout.Label("Tiles:");
        so.Update();
        SerializedProperty stringsProperty = so.FindProperty("SetTiles");
        EditorGUILayout.PropertyField(stringsProperty, true);
        so.ApplyModifiedProperties();

        gridObjects.offsetX = EditorGUILayout.FloatField("Offset X:", gridObjects.offsetX);
        gridObjects.offsetZ = EditorGUILayout.FloatField("Offset Z:", gridObjects.offsetZ);

        GUILayout.Label("Wall Setting", EditorStyles.miniBoldLabel);
        gridObjects.wall = EditorGUILayout.ObjectField("Wall Object", gridObjects.wall, typeof(GameObject), true) as GameObject;
        gridObjects.wallOffsetX = EditorGUILayout.FloatField("Wall Offset X:", gridObjects.wallOffsetX);
        gridObjects.setWallY = EditorGUILayout.FloatField("Wall Set Y:", gridObjects.setWallY);
        gridObjects.wallOffsetZ = EditorGUILayout.FloatField("Wall Offset Z:", gridObjects.wallOffsetZ);


        GUILayout.Label("Map Setting", EditorStyles.miniBoldLabel);

        posMode = (Mode)EditorGUILayout.EnumPopup("CurrentMode:", posMode);
        posX = EditorGUILayout.FloatField("Start X:", posX);
        posZ = EditorGUILayout.FloatField("Start Z:", posZ);
        rangeStart = EditorGUILayout.TextField("Excel Range Start:", rangeStart);
        rangeEnd = EditorGUILayout.TextField("Excel Range End:", rangeEnd);
        spawnWall = EditorGUILayout.Toggle("Spawn Wall", spawnWall);

        if (GUILayout.Button("Generation"))
        {
            gridObjects.tiles = SetTiles;
            SpawnMap();
            GetData();
        }
    }

    #endregion

    #region Map Generation 
    [Header("GridObjects(Prefab)")]
    [SerializeField]
    private GridObjects gridObjects;

    public Mode posMode = Mode.StartPos;
    public float posX = 0;
    public float posZ = 0;

    private float changeX = 0;
    private float changeZ = 0;

    public string rangeStart = "A1";
    public string rangeEnd = "B2";

    public bool spawnWall;

    private HashSet<Vector3> currentTile;

    private Vector3[] dir =
    {
        // ????
        new Vector3(-1, 0, 0),
        // ??????
        new Vector3(1, 0 , 0),
        // ????
        new Vector3(0, 0, -1),
        // ??
        new Vector3(0, 0, 1),
    };

    private GameObject tiledParent;

    private int count = 1;

    #region excel
    private string InputURL = "https://docs.google.com/spreadsheets/d/14rbIKCHzWCK1VHf1qcgOi7S3TwRGIfXlDE-SGML7kxs/export?format=tsv&range=";
    private string URL;

    private void GetData()
    {
        DownloadItemSO();
    }

    private async void DownloadItemSO()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        await www.SendWebRequest();
        SetItemSO(www.downloadHandler.text);
    }

    void SetItemSO(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        //tiledParent.GetComponent<MapController>().Height = rowSize;
        //tiledParent.GetComponent<MapController>().Width = columnSize;

        if (posMode == Mode.StartPos)
        {
            SearchStartModeTile(row, rowSize, columnSize);
        }
        else if (posMode == Mode.CenterPos)
        {
            for (int i = rowSize / 2; i < rowSize; i++)
            {
                FindMapTile(i, row, rowSize, columnSize, -1);
            }

            for (int i = (rowSize / 2) - 1; i >= 0; i--)
            {
                FindMapTile(i, row, rowSize, columnSize, 1);
            }
        }

        if (spawnWall)
        {
            SpawnWall();
        }
    }

    void SearchStartModeTile(string[] row, int rowSize, int columnSize)
    {
        int num;
        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');
            for (int j = 0; j < columnSize; j++)
            {
                if (column[j] != string.Empty && Int32.TryParse(column[j], out num))
                    SpawnTile(num, changeX, changeZ);
                changeX += gridObjects.offsetX;
            }
            changeX = posX;
            changeZ -= gridObjects.offsetZ;
        }
    }

    void FindMapTile(int curRow, string[] row, int rowSize, int columnSize, int increase)
    {
        int num;
        string[] column = row[curRow].Split('\t');
        for (int j = columnSize / 2; j < columnSize; j++)
        {
            if (column[j] != string.Empty && Int32.TryParse(column[j], out num))
                SpawnTile(num, Mathf.Abs((columnSize / 2) - j) * gridObjects.offsetX + posX, Mathf.Abs((rowSize / 2) - curRow) *
                    (increase * gridObjects.offsetZ) + posZ);
        }
        for (int j = (columnSize / 2) - 1; j >= 0; j--)
        {
            if (column[j] != string.Empty && Int32.TryParse(column[j], out num))
                SpawnTile(num, Mathf.Abs((columnSize / 2) - j) * -gridObjects.offsetX + posX, Mathf.Abs((rowSize / 2) - curRow) *
                    (increase * gridObjects.offsetZ) + posZ);
        }
    }

    void SpawnTile(int idx, float spawnX, float spawnZ)
    {
        if (idx >= 0)
        {
            var position = new Vector3(spawnX, 0, spawnZ);
            currentTile.Add(position);
            var blockObject = Instantiate(gridObjects.tiles[idx], position, Quaternion.identity, tiledParent.transform);
            blockObject.AddComponent<BlockBase>();
            blockObject.name = $"Tile #{count++}";
        }
    }

    void SpawnWall()
    {
        GameObject wallParent = new GameObject("Wall");
        wallParent.transform.SetParent(tiledParent.transform);

        // ???? ?????? ???? ??
        Dictionary<Vector3, bool[]> checkWall = new Dictionary<Vector3, bool[]>();
        foreach (Vector3 checkTile in currentTile)
        {
            for (int i = 0; i < dir.Length; i++)
            {
                Vector3 checkPos = checkTile + dir[i];
                if (!currentTile.Contains(checkPos) && (!checkWall.ContainsKey(checkPos) || !checkWall[checkPos][i]))
                {
                    bool[] checkDir = new bool[4];
                    if (!checkWall.ContainsKey(checkPos))
                    {
                        checkDir[i] = true;
                        checkWall.Add(checkPos, checkDir);
                    }
                    else
                    {
                        checkDir = checkWall[checkPos];
                        checkDir[i] = true;
                        checkWall[checkPos] = checkDir;
                    }


                    Vector3 rotateVal = new Vector3();
                    checkPos.y = gridObjects.setWallY;

                    switch (i)
                    {
                        // ???? ??????
                        case 0:
                            rotateVal = new Vector3(0f, 90f, 0f);
                            checkPos.x += gridObjects.wallOffsetX;
                            break;
                        case 1:
                            rotateVal = new Vector3(0f, 90f, 0f);
                            checkPos.x -= gridObjects.wallOffsetX;
                            break;
                        // ?? ????
                        case 2:
                            rotateVal = new Vector3(0f, 0f, 0f);
                            checkPos.z += gridObjects.wallOffsetZ;
                            break;
                        case 3:
                            rotateVal = new Vector3(0f, 0f, 0f);
                            checkPos.z -= gridObjects.wallOffsetZ;
                            break;
                    }
                    Instantiate(gridObjects.wall, checkPos, Quaternion.Euler(rotateVal), wallParent.transform);
                }
            }
        }
    }

    #endregion
    private void SpawnMap()
    {
        changeX = posX;
        changeZ = posZ;
        count = 1;
        URL = InputURL + $"{rangeStart}:{rangeEnd}";
        tiledParent = new GameObject("MapTiled");
        currentTile = new HashSet<Vector3>();
    }

    #endregion
}
#endif