using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Networking;

public class ExcelToClassConverter : MonoBehaviour
{
    private string URL;

    string pth = "Assets/01.Scripts/Data";
    StreamWriter sw;

    public string rangeStart = "A1";
    public string rangeEnd = "B2";
    public string fileName = "WeaponData";

    IEnumerator DownloadExcel()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();
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
            Debug.Log("??");
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
                temp += string.Format(ConverterFormat.variableFormat, column[j]);
                if (j < columnSize - 1) temp += Environment.NewLine + '\t';
            }
            allClass += string.Format(ConverterFormat.classFormat, className, temp) + Environment.NewLine;
        }

        sw.Write(allClass);
        sw.Flush();
        sw.Close();
    }

    [ContextMenu("MakeClass")]
    private void MakeClass()
    {
        URL = "https://docs.google.com/spreadsheets/d/1gya2C8tkrLr5HymQ4eccOAdZdZ1fhmYBe2AaSXMTee0/export?format=tsv&range=" + $"{rangeStart}:{rangeEnd}";
        StartCoroutine(DownloadExcel());
    }
}
