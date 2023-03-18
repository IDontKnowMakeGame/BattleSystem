using Tool.Data.Json;
using Tool.Data.Json.ParsingList;
using Tool.Excel;
using UnityEditor;
using UnityEngine;

namespace Tool.Weapon
{
#if UNITY_EDITOR
    public class ItemDataLoader : EditorWindow
    {
        [MenuItem("Tools/ItemDataLoader")]
        public static void ShowWindow()
        {
            GetWindow<ItemDataLoader>("ItemDataLoader");
        }

        private void OnGUI()
        {
            GUILayout.Label("ItemDataLoader", EditorStyles.boldLabel);

            GUILayout.Label("URL Setting", EditorStyles.miniBoldLabel);
            //ExcelDataReader.ID = EditorGUILayout.TextField("ID", ExcelDataReader.ID);
            ExcelDataReader.ID = "1y6kR8URl2pG-sAijzFArfcsj1SRQXP1CvNo_k19Vbjs";
            ExcelDataReader.StartIdx = EditorGUILayout.TextField("Json Cell", ExcelDataReader.StartIdx);

            if (GUILayout.Button("Generation"))
            {
                ExcelDataReader.EndIdx = ExcelDataReader.StartIdx;
                JsonManager.ReadSheetToJson<ItemTable>(ExcelDataReader.ID);
            }
        }

        private void SaveData(string data)
        {
            JsonManager.ReadSheetToJson<ItemTable>(data);
        }
    }
#endif
}