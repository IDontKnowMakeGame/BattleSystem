using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

[Serializable]
public class GridObjects
{
    public GameObject[] tiles;
    public float offsetX;
    public float offsetY;
}

public class MapGeneration : MonoBehaviour
{
    [Header("GridObjects(Prefab)")]
    [SerializeField]
    private GridObjects gridObjects;

    public float startX = 0;
    public float startY = 0;

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
        startX = 0;
        startY = 0;
    }

    void SetItemSO(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for(int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');
            for(int j = 0; j < columnSize; j++)
            {
                if(column[j] != string.Empty)
                    SpawnTile(Int32.Parse(column[j]));
                startX += gridObjects.offsetX;
            }
            startX = 0;
            startY -= gridObjects.offsetY;
        }
    }

    void SpawnTile(int idx)
    {
        if (idx >= 0)
        {
            Instantiate(gridObjects.tiles[idx], new Vector3(startX, startY, 0), Quaternion.identity, tiledParent.transform);
        }
    }
    #endregion

    [ContextMenu("SpawnMap")]
    private void SpawnMap()
    {
        URL = "https://docs.google.com/spreadsheets/d/14rbIKCHzWCK1VHf1qcgOi7S3TwRGIfXlDE-SGML7kxs/export?format=tsv&range=" + $"{rangeStart}:{rangeEnd}";
        tiledParent = new GameObject("MapTiled");
        StartCoroutine(DownloadItemSO());
    }
}
