using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using UnityEngine.Networking;

#if UNITY_EDITOR
public class ExcelToEnumConverter : EditorWindow
{
    #region Editor
    [MenuItem("Tools/ExcelToEnumConverter")]
    public static void ShowWindow()
    {
        GetWindow<ExcelToEnumConverter>("ExcelToEnumConverter");
    }

    private void OnGUI()
    {
        GUILayout.Label("ExcelToEnum", EditorStyles.boldLabel);

        inputURL = EditorGUILayout.TextField("URL(Until \"Range=\"):", inputURL);
        enumCount = EditorGUILayout.TextField("EnumCount(empty, end X):", enumCount);
        enumName = EditorGUILayout.TextField("File Name:", enumName);

        if (GUILayout.Button("Converter"))
        {
            MakeClass();
        }
    }
    #endregion

    #region ExcelToEnum
    private string inputURL = "https://docs.google.com/spreadsheets/d/1gya2C8tkrLr5HymQ4eccOAdZdZ1fhmYBe2AaSXMTee0/export?gid=830885422&format=tsv&range=";
    private string URL;

    string pth = "Assets/01.Scripts/Data";
    StreamWriter sw;

    public string enumCount = "3";
    public string enumName = "Weapons";
    private async void DownloadExcel()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        await www.SendWebRequest();
        SetItemSO(www.downloadHandler.text);
    }

    void SetItemSO(string tsv)
    {
        string[] row = tsv.Split(Environment.NewLine);
        int rowSize = row.Length;
        string fullPth = pth + '/' + "WeaponEnum" + ".cs";

        
        if (!File.Exists(pth))
        {
            Directory.CreateDirectory(pth);
        }
        if (File.Exists(fullPth))
        {
            File.Delete(fullPth);
        }
        
        sw = new StreamWriter(fullPth);

        string allClassEnum = string.Empty;
        string temp = string.Empty;
        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');

            if (string.IsNullOrWhiteSpace(column[1]))
            {
                temp += column[0] + ",";
            }
            else
                temp += string.Format(ConverterFormat.enumValue, column[0], column[1]);

            temp += Environment.NewLine + '\t';
        }


        allClassEnum += string.Format(ConverterFormat.enumFormat, enumName, temp) + Environment.NewLine;

        sw.Write(allClassEnum);
        sw.Flush();
        sw.Close();
    }
    private void MakeClass()
    {
        URL = inputURL + $"A1:B{enumCount}";
        Debug.Log(URL);
        DownloadExcel();
    }
    #endregion
}
#endif
