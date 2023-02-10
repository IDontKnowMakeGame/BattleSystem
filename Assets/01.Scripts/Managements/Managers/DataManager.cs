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

#region Data
public enum Floor
{
    LOBBY = 0,
    FIRST,
    SECOND,
    THIRD
}
public class SavePoint
{
    public Floor currentFloor = Floor.LOBBY;

    public bool firstFloor = true;
    public bool secondFloor = false;
    public bool thirdFloor = false;
}

public class User
{
    public int maxHp = 100;

    public string currentWeapon = "oldSword";
    public string firstWeapon = "oldSword";
    public string secondWeapon = "oldTwinSword";
}

public class Inventory
{
    public List<string> inventoryInWeaponList;
    public List<string> inventoryInHeloList;
    public List<string> inventoryInUsableItemList;
}

public class MapInformation
{

}
public class WeaponStateDataList
{
    public List<WeaponStateData> weaponList;
}
[Serializable]
public class WeaponStateData
{
    public string name;
    public string weaponClass;
    public int damage;
    public float attackSpeed;
    public float attackAfterDelay;
    public int weaponWeight;
}
#endregion

public class DataManager : Manager
{
    public static User UserData;
    public static SavePoint SavePointData;
    public static Inventory InventoryData;

    private string URL;
    public override void Awake()
    {
        GameObject oas = GameManagement.Instance.GetManager<ResourceManagers>().Instantiate("DamagePoppu");
        GameManagement.Instance.GetManager<ResourceManagers>().Destroy(oas);
        UserData = DataJson.LoadJsonFile<User>(Application.dataPath + "/SAVE/User", "UserData");
        SavePointData = DataJson.LoadJsonFile<SavePoint>(Application.dataPath + "/SAVE/User", "SavePointData");
        InventoryData = DataJson.LoadJsonFile<Inventory>(Application.dataPath + "/SAVE/User", "InvectoryData");
       
    }
    #region UserData
    public void SaveToUserData()
    {
        string json = DataJson.ObjectToJson(UserData);
        DataJson.SaveJsonFile(Application.dataPath + "/SAVE/User", "UserData", json);
    }
    public void SwapCurrentWeaponData()
    {
        UserData.currentWeapon = "";
    }
    public void SwapCurrentWeaponData(string name)
    {
        if(UserData.firstWeapon != name && UserData.secondWeapon != name)
        {
            Debug.LogError($"User Haven't Weapon : {name}");
            return;
        }

        UserData.currentWeapon = name;
        
    }
    public void ChangeUserWeaponData(string name,bool isFirstWeapon = true)
    {
        if(isFirstWeapon)
        {
            if(UserData.currentWeapon == UserData.firstWeapon)
            {
                UserData.currentWeapon = name;
            }
            UserData.firstWeapon = name;
        }
        else
        {
            if (UserData.currentWeapon == UserData.secondWeapon)
            {
                UserData.currentWeapon = name;
            }
            UserData.secondWeapon = name;
        }

        SaveToUserData();
    }
    public void ChangeUserMaxHp(int value)
    {
        UserData.maxHp = value < 0 ? 1 : value;

        SaveToUserData();
    }
    #endregion

    #region SavePointData
    public void SaveToSavePointData()
    {
        string json = DataJson.ObjectToJson(SavePointData);
        DataJson.SaveJsonFile(Application.dataPath + "/SAVE/User", "SavePointData", json);
    }
    public void ChangeCurrentFloor(Floor floor)
    {
        SavePointData.currentFloor = floor;

        SaveToSavePointData();
    }
    public void OpenFloor(Floor floor)
    {
        switch(floor)
        {
            case Floor.SECOND:
                SavePointData.secondFloor = true;
                break;
            case Floor.THIRD:
                SavePointData.thirdFloor = true;
                break;
            default:
                break;
        }

        SaveToSavePointData();
    }
    #endregion

    #region WeaponStateData
  

    #endregion
}
