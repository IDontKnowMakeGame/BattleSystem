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
    public int feather = 0;

    public string currentWeapon = "oldSword";
    public string firstWeapon = "oldSword";
    public string secondWeapon = "oldTwinSword";

    public string firstHelo = "";
    public string secondHelo = "";
    public string thirdHelo = "";

    public List<ItemInfo> equipUseableItem; //0~4 
}
[Serializable]
public class ItemInfo
{
    public string name;
    public int count;
    public int maxCnt;
    public int equipNumber = 0; //1
}

[Serializable]
public class WeaponInfo
{
    public string name;
    public int level;
}
public class Inventory
{
    public List<WeaponInfo> inventoryInWeaponList;
    public List<string> inventoryInHeloList;
    public List<ItemInfo> inventoryInUsableItemList;
}

public class MapInformation
{

}
public class WeaponClassList
{
    public List<WeaponClassLevel> weaponclassList;
}
[Serializable]
public class WeaponClassLevel
{
    public string name;
    public int level;
    public int killedCount;
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
    public static WeaponClassList WeaponClassListData;

    private string URL;
    public override void Awake()
    {
        InventoryData = DataJson.LoadJsonFile<Inventory>(Application.streamingAssetsPath + "/SAVE/User", "InvectoryData");
        UserData = DataJson.LoadJsonFile<User>(Application.streamingAssetsPath + "/SAVE/User", "UserData");
        SavePointData = DataJson.LoadJsonFile<SavePoint>(Application.streamingAssetsPath + "/SAVE/User", "SavePointData");
        WeaponClassListData = DataJson.LoadJsonFile<WeaponClassList>(Application.streamingAssetsPath + "/SAVE/Weapon", "ClassLevelData");
        
        if(WeaponClassListData.weaponclassList.Count <= 0)
        {
            CreateWeaponClassListData();
        }

    }
    #region UserData
    public void SaveToUserData()
    {
        string json = DataJson.ObjectToJson(UserData);
        DataJson.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/User", "UserData", json);
    }
    public int GetFeather()
    {
        return UserData.feather;
    }
    public void AddFeahter(int value)
    {
        UserData.feather = Math.Clamp(UserData.feather + value, 0, int.MaxValue);
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
    public void EquipUsableItem(string name,int equipnumber = 0)
    {
        ItemInfo data = LoadUsableItemToInventory(name);
        if (data == null)
            Debug.LogError($"Not  Have Item : {name}");

        data.equipNumber = equipnumber;
        UserData.equipUseableItem.Add(data);

        SaveToUserData();
    }
    public void UnmountItem(int number)
    {
        for(int i=0;i<UserData.equipUseableItem.Count;i++)
        {
            if (UserData.equipUseableItem[i].equipNumber == number)
            {
                UserData.equipUseableItem.Remove(UserData.equipUseableItem[i]);
            }
        }

        SaveToUserData();
    }
    public ItemInfo LoadUsableItem(string name)
    {
        foreach(ItemInfo info in UserData.equipUseableItem)
        {
            if(info.name == name)
            {
                return info;
            }
        }
        return null;
    }
    #endregion

    #region SavePointData
    public void SaveToSavePointData()
    {
        string json = DataJson.ObjectToJson(SavePointData);
        DataJson.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/User", "SavePointData", json);
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

    #region WeaponClassData
    public void CreateWeaponClassListData()
    {
        WeaponClassListData.weaponclassList.Add(CreateWeaponClassLevel("BasicSword"));
        WeaponClassListData.weaponclassList.Add(CreateWeaponClassLevel("GreateSword"));
        WeaponClassListData.weaponclassList.Add(CreateWeaponClassLevel("TwinSword"));
        WeaponClassListData.weaponclassList.Add(CreateWeaponClassLevel("Spear"));
        WeaponClassListData.weaponclassList.Add(CreateWeaponClassLevel("Bow"));
        SaveWeaponClassListData();
    }

    public WeaponClassLevel CreateWeaponClassLevel(string name)
    {
        WeaponClassLevel weaponClass = new WeaponClassLevel();
        weaponClass.name = name;
        weaponClass.level = 0;
        weaponClass.killedCount = 0;
        return weaponClass;
    }

    public WeaponClassLevel LoadWeaponClassLevel(string name)
    {
        foreach(WeaponClassLevel data in WeaponClassListData.weaponclassList)
        {
            if(data.name == name)
            {
                return data;
            }
        }
        return null;
    }

    public void SaveWeaponClassListData(WeaponClassLevel data)
    {
        for(int i = 0;i<WeaponClassListData.weaponclassList.Count;i++)
        {
            if(WeaponClassListData.weaponclassList[i].name == data.name)
            {
                WeaponClassListData.weaponclassList[i] = data;
                SaveWeaponClassListData();
                return;
            }
        }

        Debug.LogError("Not Found WeaponClassName : There are no weapons with matching names.");
    }

    public void SaveWeaponClassListData()
    {
        string json = DataJson.ObjectToJson(WeaponClassListData);
        DataJson.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/Weapon", "ClassLevelData", json);
    }
    #endregion

    #region Inventory
    public void SaveToInventoryData()
    {
        string json = DataJson.ObjectToJson(InventoryData);
        DataJson.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/User", "InvectoryData",json);
    }
    public void AddWeaponToInventory(WeaponInfo name)
    {
        InventoryData.inventoryInWeaponList.Add(name);
        SaveToInventoryData();
    }
    public List<WeaponInfo> LoadWeaponData()
    {
        return InventoryData.inventoryInWeaponList;
    }
    public WeaponInfo LoadWeaponData(string name)
    {
        foreach (WeaponInfo weapon in InventoryData.inventoryInWeaponList)
        {
            if (weapon.name == name)
            {
                return weapon;
            }
        }
        return null;
    }
    public int LoadWeaponLevelData(string name)
    {
        foreach (WeaponInfo weapon in InventoryData.inventoryInWeaponList)
        {
            if (weapon.name == name)
            {
                return weapon.level;
            }
        }
        return 1;
    }
    public bool HaveWeapon(string name)
    {
        foreach(WeaponInfo weapon in InventoryData.inventoryInWeaponList)
        {
            if(weapon.name == name)
            {
                return true;
            }
        }
        return false;
    }

    public void AddHeloToInventory(string name)
    {
        InventoryData.inventoryInHeloList.Add(name);
        SaveToInventoryData();
    }
    public void AddUsableItemToInventory(ItemInfo data)
    {
        InventoryData.inventoryInUsableItemList.Add(data);
        SaveToInventoryData();
    }
    public ItemInfo LoadUsableItemToInventory(string name)
    {
        foreach(ItemInfo info in InventoryData.inventoryInUsableItemList)
        {
            if (info.name == name)
                return info;
        }
        return null;
    }

    #endregion

}
