using System;
using System.Threading.Tasks;
using Tool.Excel;
using UnityEditor;
using UnityEngine;

namespace Tool.Map
{
    #if UNITY_EDITOR
    public class MapGenerator : EditorWindow
    {
        [MenuItem("Tools/MapGenerator")]
        public static void ShowWindow()
        {
            GetWindow<MapGenerator>("MapGenerator");
        }

        private async void OnGUI()
        {
            GUILayout.Label("MapGeneration", EditorStyles.boldLabel);

            GUILayout.Label("URL Setting", EditorStyles.miniBoldLabel);
            ExcelDataReader.ID = EditorGUILayout.TextField("ID", ExcelDataReader.ID);
            ExcelDataReader.StartIdx = EditorGUILayout.TextField("StartIdx", ExcelDataReader.StartIdx);
            ExcelDataReader.EndIdx = EditorGUILayout.TextField("EndIdx", ExcelDataReader.EndIdx);
            if (GUILayout.Button("Generation"))
            {
                ExcelDataReader.GetData(GenerateMap);
            }
        }

        private void GenerateMap(string data)
        {
            //TODO : Generate Map 

        }
    }
    #endif
}