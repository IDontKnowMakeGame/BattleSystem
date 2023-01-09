using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Core;
using Manager;
using UnityEngine.Networking;

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

    public float startX = 0;
    public float startZ = 0;

    private float changeX = 0;
    private float changeZ = 0;

    public string rangeStart = "A1";
    public string rangeEnd = "B2";

    private GameObject tiledParent;

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
        int num;

        for (int i = rowSize / 2; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');
            for (int j = columnSize / 2; j < columnSize; j++)
            {
                if (column[j] != string.Empty && Int32.TryParse(column[j], out num))
                    SpawnTile(num, Mathf.Abs((columnSize / 2) - j) * gridObjects.offsetX + startX, Mathf.Abs((rowSize / 2) - i) * 
                        -gridObjects.offsetZ + startZ);
            }
            for (int j = (columnSize / 2) - 1; j >= 0; j--)
            {
                if (column[j] != string.Empty && Int32.TryParse(column[j], out num))
                    SpawnTile(num, Mathf.Abs((columnSize / 2) - j) * -gridObjects.offsetX + startX, Mathf.Abs((rowSize / 2) - i) *
                        -gridObjects.offsetZ + startZ);
            }
        }

        for (int i = (rowSize / 2) - 1; i >= 0; i--)
        {
            string[] column = row[i].Split('\t');
            for (int j = columnSize / 2; j < columnSize; j++)
            {
                if (column[j] != string.Empty && Int32.TryParse(column[j], out num))
                    SpawnTile(num, Mathf.Abs((columnSize / 2) - j) * gridObjects.offsetX + startX, Mathf.Abs((rowSize / 2) - i) *
                        gridObjects.offsetZ + startZ);
            }
            for (int j = (columnSize / 2) - 1; j >= 0; j--)
            {
                if (column[j] != string.Empty && Int32.TryParse(column[j], out num))
                    SpawnTile(num, Mathf.Abs((columnSize / 2) - j) * -gridObjects.offsetX + startX, Mathf.Abs((rowSize / 2) - i) *
                        gridObjects.offsetZ + startZ);
            }
        }

        /*
        for(int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');
            for(int j = 0; j < columnSize; j++)
            {
                Debug.Log(column[j]);
                if(column[j] != string.Empty && Int32.TryParse(column[j], out num))
                    SpawnTile(num);
                changeX += gridObjects.offsetX;
            }
            changeX = startX;
            changeZ -= gridObjects.offsetZ;
        }
        */
    }

    void SpawnTile(int idx, float spawnX, float spawnZ)
    {
        if (idx >= 0)
        {
            var position = new Vector3(spawnX, 0, spawnZ);
            var blockObject = Instantiate(gridObjects.tiles[idx], position, Quaternion.identity, tiledParent.transform);
            blockObject.AddComponent<Block>();
            blockObject.name = $"Tile #{idx}";
        }
    }
    #endregion

    [ContextMenu("SpawnMap")]
    private void SpawnMap()
    {
        changeX = startX;
        changeZ = startZ;
        URL = "https://docs.google.com/spreadsheets/d/14rbIKCHzWCK1VHf1qcgOi7S3TwRGIfXlDE-SGML7kxs/export?format=tsv&range=" + $"{rangeStart}:{rangeEnd}";
        tiledParent = new GameObject("MapTiled");
        StartCoroutine(DownloadItemSO());
    }
}
