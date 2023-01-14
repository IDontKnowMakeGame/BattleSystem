using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public enum Mode
{
    StartPos,
    CenterPos
}

[Serializable]
public class GridObjects
{
    public GameObject[] tiles;
    public float offsetX;
    public float offsetZ;
}

public class MapGeneration : MonoBehaviour
{
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

    private GameObject tiledParent;
    private int count = 1;

    #region excel
    private string URL;

    IEnumerator DownloadItemSO()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();
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
            var blockObject = Instantiate(gridObjects.tiles[idx], position, Quaternion.identity, tiledParent.transform);
            blockObject.AddComponent<BlockBase>();
            blockObject.name = $"Tile #{count++}";
        }
    }
    #endregion

    [ContextMenu("SpawnMap")]
    private void SpawnMap()
    {
        changeX = posX;
        changeZ = posZ;
        count = 1;
        URL = "https://docs.google.com/spreadsheets/d/14rbIKCHzWCK1VHf1qcgOi7S3TwRGIfXlDE-SGML7kxs/export?format=tsv&range=" + $"{rangeStart}:{rangeEnd}";
        tiledParent = new GameObject("MapTiled");
        StartCoroutine(DownloadItemSO());
    }
}
