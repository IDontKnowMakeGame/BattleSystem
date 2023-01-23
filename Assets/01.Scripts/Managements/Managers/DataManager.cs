using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.CompilerServices;
using System;
using System.Threading.Tasks;
using Managements.Managers.Base;
using Unit.Core;
using Managements;

public class DataJson : MonoBehaviour
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


}

public class SavePoint
{
    public bool firstFloor = false;
    public bool scecondFloor = false;
    public bool thirdFloor = false;
}

public class User
{

    public int currentFloor = 0;
    public SavePoint savePoint;

    public string currentWeapon;
    public string firstWeapon;
    public string SecondWeapon;

    public List<string> inventoryInWeaponList;
    public List<string> inventoryInHeloList;
    public List<string> inventoryInUsableItemList;
}

public class WeaponStateData
{
    public WeaponStateData(string name,string weaponClass, int damage, float attackSpeed, float attackAfterDelay, int weaponWeight)
    {
        this.name = name;
        this.weaponClass = weaponClass;
        this.damage = damage;
        this.attackSpeed = attackSpeed;
        this.attackAfterDelay = attackAfterDelay;
        this.weaponWeight = weaponWeight;
    }

    public string name;
    public string weaponClass;
    public int damage;
    public float attackSpeed;
    public float attackAfterDelay;
    public int weaponWeight;
}

public class DataManager : Manager
{
    public static User UserData;

    public List<WeaponStateData> weaponStateDataList = new List<WeaponStateData>();

    public bool isSettingComplate = false;

    private string URL;
    public override void Awake()
    {
        UserData = DataJson.LoadJsonFile<User>(Application.dataPath + "/SAVE/User", "UserData");

        URL = "https://docs.google.com/spreadsheets/d/1y6kR8URl2pG-sAijzFArfcsj1SRQXP1CvNo_k19Vbjs/export?format=tsv&range=A2:F30";

        DownloadItemSO();
    }

    private async void DownloadItemSO()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        await www.SendWebRequest();
        SetWeaponStateData(www.downloadHandler.text);
    }

    private void SetWeaponStateData(string tsv)
    {
        string[] row = tsv.Split('\n');

        int weaponCount = row.Length;
        for (int i = 0;i<weaponCount;i++)
        {
            string[] col = row[i].Split("\t");
            weaponStateDataList.Add(new WeaponStateData(
                col[0],
                col[1],
                int.Parse(col[2]), 
                float.Parse(col[3]), 
                float.Parse(col[4]), 
                int.Parse(col[5]))
            );
        }

        Debug.Log(weaponStateDataList.Count);

        isSettingComplate = true;
    }

    public void GetWeaponStateData(string name,Action<WeaponStats> action)
    {
        GameManagement.Instance.StartCoroutine(WaitForGetWeaponData(name, action));
    }
    public IEnumerator WaitForGetWeaponData(string name, Action<WeaponStats> action)
    {
        yield return new WaitUntil(() => isSettingComplate);
        foreach (WeaponStateData data in weaponStateDataList)
        {
            if (data.name == name)
            {
                action(WeaponSerializable(data));
                yield break;
            }

        }
        action(null);
    }

    public WeaponStats WeaponSerializable(WeaponStateData data)
    {
        WeaponStats state = new WeaponStats();

        state.Afs = data.attackAfterDelay;
        state.Atk = data.damage;
        state.Ats = data.attackSpeed;
        state.Weight = data.weaponWeight;

        return state;
    }
}
