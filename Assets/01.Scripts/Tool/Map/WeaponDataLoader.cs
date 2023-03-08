using Tool.Data.Json;
using Tool.Data.Json.ParsingList;
using Tool.Excel;
using UnityEditor;
using UnityEngine;

namespace Tool.Weapon
{
#if UNITY_EDITOR
    public class WeaponDataLoader : EditorWindow
    {
        [MenuItem("Tools/WeaponDataLoader")]
        public static void ShowWindow()
        {
            GetWindow<WeaponDataLoader>("WeaponDataLoader");
        }

        private async void OnGUI()
        {
            GUILayout.Label("WeaponDataLoader", EditorStyles.boldLabel);

            GUILayout.Label("URL Setting", EditorStyles.miniBoldLabel);
            ExcelDataReader.ID = EditorGUILayout.TextField("ID", ExcelDataReader.ID);
            ExcelDataReader.StartIdx = EditorGUILayout.TextField("Json Cell", ExcelDataReader.StartIdx);

            if (GUILayout.Button("Generation"))
            {
                ExcelDataReader.EndIdx = ExcelDataReader.StartIdx;
                JsonManager.ReadSheetToJson<WeaponLists>(ExcelDataReader.ID);
            }
        }

        private void SaveData(string data)
        {
            JsonManager.ReadSheetToJson<WeaponLists>(data);
        }
    }
#endif
}