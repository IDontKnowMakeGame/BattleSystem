using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.Linq;
using System;

public class WeaponStatusFromExel : EditorWindow
{
    public List<WeaponStateData> weaponStateDataList = new List<WeaponStateData>();
    public string[] asd = { "asd", "asd" };

    string URL = "https://docs.google.com/spreadsheets/d/1y6kR8URl2pG-sAijzFArfcsj1SRQXP1CvNo_k19Vbjs/export?format=tsv&range=A2:F30";

    [MenuItem("Tools/WeaponStatusFromExel")]
    public static void ShowWindow()
    {
        GetWindow<WeaponStatusFromExel>("WeaponStatusFromExel");
    }
    private void OnGUI()
    {
        GUILayout.Label("Exel Data to Json", EditorStyles.boldLabel);

        URL = EditorGUILayout.TextField("URL", URL);

        if(GUILayout.Button("Press me"))
        {
            Debug.Log("ClickBtn");
            GetData();
        }
    }

    private void GetData()
    {
        DownloadItemSO();
    }
    #region WeaponStateData
    private async void DownloadItemSO()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        await www.SendWebRequest();
        Debug.Log("Exel Load Complate");
        Debug.Log(www.downloadHandler.text);
        SetWeaponStateData(www.downloadHandler.text);
    }
    private void SetWeaponStateData(string tsv)
    {
        string[] row = tsv.Split('\n');

        int weaponCount = row.Length;
        for (int i = 0; i < weaponCount; i++)
        {
            string[] col = row[i].Split("\t");
            //weaponStateDataList.Add(new WeaponStateData(
            //    col[0],
            //    col[1],
            //    int.Parse(col[2]),
            //    float.Parse(col[3]),
            //    float.Parse(col[4]),
            //    int.Parse(col[5]))
            //);
            WeaponStateData data = new WeaponStateData();
            data.name = col[0];
            data.weaponClass = col[1];
            data.damage = int.Parse(col[2]);
            data.attackSpeed = float.Parse(col[3]);
            data.attackAfterDelay = float.Parse(col[4]);
            data.weaponWeight = int.Parse(col[5]);
            weaponStateDataList.Add(data);
        }
        Debug.Log($"{weaponStateDataList[0]}");
        DataToJson();
    }

    private void DataToJson()
    {
        WeaponStateDataList list = new WeaponStateDataList();
        list.weaponList = weaponStateDataList;

        string json = DataJson.ObjectToJson(list);
        DataJson.SaveJsonFile(Application.dataPath + "/SAVE/Weapon", "WeaponStatus",json);
    }
    #endregion
}
