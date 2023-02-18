using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using UnityEngine.Networking;

#if UNITY_EDITOR
public class ExcelToClassConverter : EditorWindow
{
    #region Editor
    [MenuItem("Tools/ExcelToClassConverter")]
    public static void ShowWindow()
    {
        GetWindow<ExcelToClassConverter>("ExcelToClassConverter");
    }

    private void OnGUI()
    {
        GUILayout.Label("ExcelToClass", EditorStyles.boldLabel);

        inputURL = EditorGUILayout.TextField("URL(Until \"Range=\"):", inputURL);
        rangeStart = EditorGUILayout.TextField("Excel Range Start:", rangeStart);
        rangeEnd = EditorGUILayout.TextField("Excel Range End:", rangeEnd);
        fileName = EditorGUILayout.TextField("File Name:", fileName);

        if(GUILayout.Button("Converter"))
        {
            MakeClass();
        }
    }
    #endregion


    #region ExcelToClass
    private string inputURL = "https://docs.google.com/spreadsheets/d/1gya2C8tkrLr5HymQ4eccOAdZdZ1fhmYBe2AaSXMTee0/export?format=tsv&range=";
    private string URL;

    string pth = "Assets/01.Scripts/Data";
    StreamWriter sw;

    public string rangeStart = "A1";
    public string rangeEnd = "B2";
    public string fileName = "WeaponData";
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
        int columnSize = row[0].Split('\t').Length;
        string fullPth = pth + '/' + fileName + ".cs";

        if (!File.Exists(pth))
        {
            Directory.CreateDirectory(pth);
        }
        if(File.Exists(fullPth))
        {
            File.Delete(fullPth);
        }

        sw = new StreamWriter(fullPth);

        string className = string.Empty;
        string allClass = string.Empty;
        for(int i = 0; i < rowSize; i++)
        {
            string temp = string.Empty;
            string[] column = row[i].Split('\t');
            className = column[0];
            for (int j = 1; j < columnSize; j++)
            {
                if(string.IsNullOrWhiteSpace(column[j]))
                {
                    continue;
                }
                temp += string.Format(ConverterFormat.variableFormat, column[j]) + Environment.NewLine + '\t';
            }
            allClass += string.Format(ConverterFormat.classFormat, className, temp) + Environment.NewLine;
        }

        sw.Write(allClass);
        sw.Flush();
        sw.Close();
    }

    private void MakeClass()
    {
        URL = inputURL + $"{rangeStart}:{rangeEnd}";
        DownloadExcel();
    }
    #endregion
}
#endif