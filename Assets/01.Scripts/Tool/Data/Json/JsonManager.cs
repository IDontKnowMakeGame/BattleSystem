using System.IO;
using System.Text;
using Tool.Excel;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace Tool.Data.Json
{
    public class JsonManager
    {
        public static string ObjectToJson(object obj)
        {
            return JsonUtility.ToJson(obj, true);
        }
        public static T JsonToObject<T>(string jsonData)
        {
            return JsonUtility.FromJson<T>(jsonData);
        }
        public static void SaveJsonFile(string createPath, string fileName, string jsonData)
        {
            if (!Directory.Exists(createPath))
            {
                Directory.CreateDirectory(createPath);
            }
            FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(jsonData);
            fileStream.Write(data, 0, data.Length); fileStream.Close();
        }
        public static T LoadJsonFile<T>(string loadPath, string fileName) where T : new()
        {
            if (!Directory.Exists(loadPath))
            {
                Directory.CreateDirectory(loadPath);
            }
            if (!File.Exists(string.Format("{0}/{1}.json", loadPath, fileName)))
            {
                SaveJsonFile(loadPath, fileName, ObjectToJson(new T()));
            }
            FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open);
            byte[] data = new byte[fileStream.Length]; fileStream.Read(data, 0, data.Length);
            fileStream.Close();
            string jsonData = Encoding.UTF8.GetString(data);
            return JsonToObject<T>(jsonData);
        }

        public static void ReadSheetToJson<T>(string id) where T : new()
        {
            ExcelDataReader.GetData(ParseData<T>);
        }

        private static void ParseData<T>(string json) where T : new()
        {
            Debug.Log(json);
            SaveJsonFile(Application.streamingAssetsPath + $"/Save/Json/{typeof(T)}", typeof(T).ToString(), json);
            var obj = LoadJsonFile<T>(Application.streamingAssetsPath + $"/Save/Json/{typeof(T)}", typeof(T).ToString());
            Debug.Log(obj);
        }
    }
}
