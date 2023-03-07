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

    public string firstWeapon;
    public string secondWeapon;

    public string firstHelo = "";
    public string secondHelo = "";
    public string thirdHelo = "";

    public List<ItemInfo> equipUseableItem; //0~4 x
    public List<Contract> contractsInfo;
}
[Serializable]
public class Contract
{
    public string name;
    public int deHP;
    public int addAtk;
}
[Serializable]
public class ItemInfo
{
    public ItemID id;
    public string name;
    public int currentCnt;
    public int maxCnt;
    public int equipNumber = 0; //1
}


public class Inventory
{
    public List<ItemInfo> inventoryInWeaponList;
    public List<ItemInfo> inventoryInHeloList;
    public List<ItemInfo> inventoryInUsableItemList;
    public List<ItemInfo> inventoryInQuestItemList;
}

public class MapInformation
{

}
public class WeaponLevelList
{
    public List<WeaponInfo> weaponInfoDatas;
}
[Serializable]
public class WeaponInfo
{
    public string name;
    public int level;
}
public class WeaponClassLevelList
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
    public static WeaponClassLevelList WeaponClassListData;
    public static WeaponStateDataList WeaponStateDataListData;
    public static WeaponLevelList WeaponLevelListData;

    private string URL;
    public override void Awake()
    {
        InventoryData = DataJson.LoadJsonFile<Inventory>(Application.streamingAssetsPath + "/SAVE/User", "InvectoryData");
        UserData = DataJson.LoadJsonFile<User>(Application.streamingAssetsPath + "/SAVE/User", "UserData");
        SavePointData = DataJson.LoadJsonFile<SavePoint>(Application.streamingAssetsPath + "/SAVE/User", "SavePointData");
        WeaponClassListData = DataJson.LoadJsonFile<WeaponClassLevelList>(Application.streamingAssetsPath + "/SAVE/Weapon", "ClassLevelData");
        WeaponLevelListData = DataJson.LoadJsonFile<WeaponLevelList>(Application.streamingAssetsPath + "/SAVE/Weapon", "WeaponLevelData");
        WeaponStateDataListData = DataJson.LoadJsonFile<WeaponStateDataList>(Application.streamingAssetsPath + "/SAVE/Weapon", "WeaponStatus");

        if (WeaponClassListData.weaponclassList.Count <= 0)
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
    public void SetFeahter(int value)
    {
        UserData.feather = Math.Clamp( value, 0, int.MaxValue);

        SaveToUserData();
    }
    public void AddFeahter(int value)
    {
        UserData.feather = Math.Clamp(UserData.feather + value, 0, int.MaxValue);

        SaveToUserData();
    }

    public void SwapWeaponData(string name)
    {
        if(UserData.firstWeapon != name && UserData.secondWeapon != name)
        {
            Debug.LogError($"User Haven't Weapon : {name}");
            return;
        }


        SaveToUserData();
    }
    public void ChangeUserWeaponData(string name,bool isFirstWeapon = true)
    {
        if(isFirstWeapon)
        {
            UserData.firstWeapon = name;
        }
        else
        {
            UserData.secondWeapon = name;
        }

        SaveToUserData();
    }
    public void ChangeUserMaxHp(int value)
    {
        UserData.maxHp = Math.Clamp(value, 100, 1200);

        SaveToUserData();
    }
    public void EquipUsableItem(int id,int equipnumber)
    {
        ItemInfo data = LoadUsableItemFromInventory(id);
        if (data == null)
            Debug.LogError($"Not  Have Item : {id}");

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
    public ItemInfo LoadUsableItem(int id)
    {
        foreach(ItemInfo info in UserData.equipUseableItem)
        {
            if(info.id == id)
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
    public WeaponStateData LoadWeaponStateData(string name)
    {
        foreach(WeaponStateData data in WeaponStateDataListData.weaponList)
        {
            if(data.name == name)
            {
                return data;
            }
        }

        return null;
    }
    #endregion

    #region WeaponLevel
    public void SaveWeaponLevelListData()
    {
        string json = DataJson.ObjectToJson(WeaponLevelListData);
        DataJson.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/Weapon", "WeaponLevelData", json);
    }

    public int LoadWeaponLevelData(string name)
    {
        foreach(WeaponInfo info in WeaponLevelListData.weaponInfoDatas)
        {
            if (info.name == name)
                return info.level;
        }

        return 0;
    }

    public void SaveWeaponLevelData(string name,int changeLevel)
    {
        for(int i = 0;i<WeaponLevelListData.weaponInfoDatas.Count;i++)
        {
            if(WeaponLevelListData.weaponInfoDatas[i].name == name)
            {
                WeaponLevelListData.weaponInfoDatas[i].level = changeLevel;
                SaveWeaponLevelListData();
                return;
            }
        }

        WeaponInfo data = new WeaponInfo();
        data.name = name;
        data.level = changeLevel;

        WeaponLevelListData.weaponInfoDatas.Add(data);
        SaveWeaponLevelListData();
    }

    public void SaveUpGradeWeaponLevelData(string name)
    {
        for (int i = 0; i < WeaponLevelListData.weaponInfoDatas.Count; i++)
        {
            if (WeaponLevelListData.weaponInfoDatas[i].name == name)
            {
                WeaponLevelListData.weaponInfoDatas[i].level++;
                SaveWeaponLevelListData();
                return;
            }
        }

        WeaponInfo data = new WeaponInfo();
        data.name = name;
        data.level = 1;

        WeaponLevelListData.weaponInfoDatas.Add(data);
        SaveWeaponLevelListData();
    }

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

    public void AddItemInInventory(ItemInfo item)
    {

    }    
    public bool HaveWeapon(int name)
    {
        foreach(string weapon in InventoryData.inventoryInWeaponList)
        {
            if(weapon == name)
            {
                return true;
            }
        }
        return false;
    }
    public List<ItemInfo> LoadUsableItemFromInventory()
    {
        return InventoryData.inventoryInUsableItemList;
    }
    public ItemInfo LoadUsableItemFromInventory(int id)
    {
        foreach(ItemInfo info in InventoryData.inventoryInUsableItemList)
        {
            if (info.id == id)
                return info;
        }
        return null;
    }
    public void ChangeItemInfo(ItemInfo data)
    {
        for(int i = 0;i< InventoryData.inventoryInUsableItemList.Count;i++)
        {
            if(InventoryData.inventoryInUsableItemList[i].id == data.id)
            {
                InventoryData.inventoryInUsableItemList[i] = data;
                return;
            }
        }
    }

    #endregion

}
