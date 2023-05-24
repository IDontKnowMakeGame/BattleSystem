using System;
using System.Net;
using UnityEngine.Networking;

namespace Tool.Excel
{
    public static class ExcelDataReader
    {
        public static string ID { get; set; }

        public static string URL { get; set; }
        
        public static string StartIdx = "";
        public static string EndIdx = "";
        public static string Gid = "";
        public static bool IsParsing = false;
        public static bool IsProcessing = false;
        public static string Data;

        public static async void GetData(Action<string> callback)
        {
            string url;
            url = $"https://docs.google.com/spreadsheets/d/{ID}/export?format=tsv&gid={Gid}&range={StartIdx}:{EndIdx}";
            var www = UnityWebRequest.Get(url);
            await www.SendWebRequest();
            Data = www.downloadHandler.text;
            callback?.Invoke(Data);
        }
    }
}