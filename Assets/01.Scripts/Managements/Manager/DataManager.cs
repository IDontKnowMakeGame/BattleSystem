using System;
using System.Collections;
using System.Collections.Generic;
using Tool.Data.Json;
using UnityEngine;

public class DataManager : Managements.Managers.Manager
{
    public static User UserData;
    //public static SavePoint SavePointData;
    public static Inventory InventoryData;
    public static WeaponClassLevelList WeaponClassLevelListData;
    public static WeaponLevelList WeaponLevelListData;

    private string URL;
    public override void Awake()
    {
        InventoryData = JsonManager.LoadJsonFile<Inventory>(Application.streamingAssetsPath + "/SAVE/User", "InvectoryData");
        UserData = JsonManager.LoadJsonFile<User>(Application.streamingAssetsPath + "/SAVE/User", "UserData");
        //SavePointData = JsonManager.LoadJsonFile<SavePoint>(Application.streamingAssetsPath + "/SAVE/User", "SavePointData");
        WeaponClassLevelListData = JsonManager.LoadJsonFile<WeaponClassLevelList>(Application.streamingAssetsPath + "/SAVE/Weapon", "ClassLevelData");
        WeaponLevelListData = JsonManager.LoadJsonFile<WeaponLevelList>(Application.streamingAssetsPath + "/SAVE/Weapon", "WeaponLevelData");

        if (WeaponClassLevelListData.weaponclassList.Count <= 0)
        {
            CreateWeaponClassListData();
        }

    }

    #region UserData
    public void SaveToUserData()
    {
        string json = JsonManager.ObjectToJson(UserData);
        JsonManager.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/User", "UserData", json);
    }
    public int GetFeather()
    {
        return UserData.feather;
    }
    public void SetFeahter(int value)
    {
        UserData.feather = Math.Clamp(value, 0, int.MaxValue);

        SaveToUserData();
    }
    public void AddFeahter(int value)
    {
        UserData.feather = Math.Clamp(UserData.feather + value, 0, int.MaxValue);

        SaveToUserData();
    }

    public void SwapWeaponData()
    {
        if (UserData.firstWeapon == ItemID.None || UserData.secondWeapon == ItemID.None)
            return;

        ItemID tempID = UserData.firstWeapon;
        UserData.firstWeapon = UserData.secondWeapon;
        UserData.secondWeapon = tempID;

        SaveToUserData();
    }

    public void ChangeUserWeaponData(ItemID id, bool isFirstWeapon = true)
    {
        if (isFirstWeapon)
        {
            UserData.firstWeapon = id;
        }
        else
        {
            UserData.secondWeapon = id;
        }

        SaveToUserData();
    }
    public void ChangeUserMaxHp(int value)
    {
        UserData.maxHp = Math.Clamp(value, 100, 1200);

        SaveToUserData();
    }
    public void EquipUsableItem(ItemID id, int equipnumber)
    {
        ItemData data = LoadUsableItemFromInventory(id);
        if (data == null)
            Debug.LogError($"Not  Have Item : {id}");

        data.equipNumber = equipnumber;
        UserData.equipUseableItem.Add(data);

        SaveToUserData();
    }
    public void UnmountItem(int number)
    {
        for (int i = 0; i < UserData.equipUseableItem.Count; i++)
        {
            if (UserData.equipUseableItem[i].equipNumber == number)
            {
                UserData.equipUseableItem.Remove(UserData.equipUseableItem[i]);
            }
        }

        SaveToUserData();
    }
    public ItemData LoadUsableItem(ItemID id)
    {
        foreach (ItemData info in UserData.equipUseableItem)
        {
            if (info.id == id)
            {
                return info;
            }
        }
        return null;
    }
    #endregion

    #region SavePointData
    
    #endregion

    #region WeaponLevel
    public void SaveWeaponLevelListData()
    {
        string json = JsonManager.ObjectToJson(WeaponLevelListData);
        JsonManager.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/Weapon", "WeaponLevelData", json);
    }

    public int LoadWeaponLevelData(ItemID id)
    {
        foreach (WeaponLevel info in WeaponLevelListData.weaponInfoDatas)
        {
            if (info.id == id)
                return info.level;
        }

        return 0;
    }

    public void SaveWeaponLevelData(ItemID id, int changeLevel)
    {
        for (int i = 0; i < WeaponLevelListData.weaponInfoDatas.Count; i++)
        {
            if (WeaponLevelListData.weaponInfoDatas[i].id == id)
            {
                WeaponLevelListData.weaponInfoDatas[i].level = changeLevel;
                SaveWeaponLevelListData();
                return;
            }
        }

        WeaponLevel data = new WeaponLevel();
        data.id = id;
        data.level = changeLevel;

        WeaponLevelListData.weaponInfoDatas.Add(data);
        SaveWeaponLevelListData();
    }

    public void SaveUpGradeWeaponLevelData(ItemID id)
    {
        for (int i = 0; i < WeaponLevelListData.weaponInfoDatas.Count; i++)
        {
            if (WeaponLevelListData.weaponInfoDatas[i].id == id)
            {
                WeaponLevelListData.weaponInfoDatas[i].level++;
                SaveWeaponLevelListData();
                return;
            }
        }

        WeaponLevel data = new WeaponLevel();
        data.id = id;
        data.level = 1;

        WeaponLevelListData.weaponInfoDatas.Add(data);
        SaveWeaponLevelListData();
    }

    #endregion

    #region WeaponClassData
    public void SaveWeaponClassListData()
    {
        string json = JsonManager.ObjectToJson(WeaponClassLevelListData);
        JsonManager.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/Weapon", "ClassLevelData", json);
    }
    public void CreateWeaponClassListData()
    {
        WeaponClassLevelListData.weaponclassList.Add(CreateWeaponClassLevel("BasicSword"));
        WeaponClassLevelListData.weaponclassList.Add(CreateWeaponClassLevel("GreateSword"));
        WeaponClassLevelListData.weaponclassList.Add(CreateWeaponClassLevel("TwinSword"));
        WeaponClassLevelListData.weaponclassList.Add(CreateWeaponClassLevel("Spear"));
        WeaponClassLevelListData.weaponclassList.Add(CreateWeaponClassLevel("Bow"));
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
        foreach (WeaponClassLevel data in WeaponClassLevelListData.weaponclassList)
        {
            if (data.name == name)
            {
                return data;
            }
        }
        return null;
    }

    public void SaveWeaponClassListData(string name,int upgradeLevel = 1)
    {
        for (int i = 0; i < WeaponClassLevelListData.weaponclassList.Count; i++)
        {
            if (WeaponClassLevelListData.weaponclassList[i].name == name)
            {
                WeaponClassLevelListData.weaponclassList[i].level += upgradeLevel;
                SaveWeaponClassListData();
                return;
            }
        }

        Debug.LogError("Not Found WeaponClassName : There are no weapons with matching names.");
    }


    #endregion

    #region Inventory
    public void SaveToInventoryData()
    {
        string json = JsonManager.ObjectToJson(InventoryData);
        JsonManager.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/User", "InvectoryData", json);
    }

    public void AddItemInInventory(ItemData item)
    {
        if ((int)item.id < 100)
        { //Weapon
            if (HaveWeapon(item.id)) return;
            InventoryData.inventoryInWeaponList.Add(item);

        }
        else if ((int)item.id < 200)
        { //Helo
            if (HaveHelo(item.id)) return;
            InventoryData.inventoryInHeloList.Add(item);
        }
        else if ((int)item.id < 300)
        { //UseableItem

            ItemData info = LoadUsableItemFromInventory(item.id);
            if (info == null)
            {
                info = item;
                InventoryData.inventoryInUsableItemList.Add(item);
            }
            else
            {
                AddItemCount(item);
            }
        }
        else if ((int)item.id < 400)
        { //QuestItem
            if (HaveQuestItem(item.id)) return;
            InventoryData.inventoryInQuestItemList.Add(item);
        }

        SaveToInventoryData();
    }
    public ItemData LoadItemFromInventory(ItemID id)
    {
        if ((int)id < 100)
        { //Weapon
            return LoadWeaponDataFromInventory(id);
        }
        else if ((int)id < 200)
        { //Helo
            return LoadHeloDataFromInventory(id);
        }
        else if ((int)id < 300)
        { //UseableItem
            return LoadUsableItemFromInventory(id);
        }
        else if ((int)id < 400)
        { //QuestItem
            return LoadQuestFromInventory(id);
        }

        return null;
    }

    public List<ItemData> LoadWeaponDataFromInventory()
    {
        return InventoryData.inventoryInWeaponList;
    }
    public ItemData LoadWeaponDataFromInventory(ItemID id)
    {
        foreach (ItemData info in InventoryData.inventoryInWeaponList)
        {
            if (info.id == id)
                return info;
        }
        return null;
    }
    public bool HaveWeapon(ItemID id)
    {
        foreach (ItemData weapon in InventoryData.inventoryInWeaponList)
        {
            if (weapon.id == id)
            {
                return true;
            }
        }
        return false;
    }

    public List<ItemData> LoadHeloDataFromInventory()
    {
        return InventoryData.inventoryInHeloList;
    }
    public ItemData LoadHeloDataFromInventory(ItemID id)
    {
        foreach (ItemData info in InventoryData.inventoryInHeloList)
        {
            if (info.id == id)
                return info;
        }
        return null;
    }
    public bool HaveHelo(ItemID id)
    {
        foreach (ItemData helo in InventoryData.inventoryInHeloList)
        {
            if (helo.id == id)
            {
                return true;
            }
        }
        return false;
    }

    public List<ItemData> LoadUsableItemFromInventory()
    {
        return InventoryData.inventoryInUsableItemList;
    }
    public ItemData LoadUsableItemFromInventory(ItemID id)
    {
        foreach (ItemData info in InventoryData.inventoryInUsableItemList)
        {
            if (info.id == id)
                return info;
        }
        return null;
    }
    public void ChangeItemInfo(ItemData data)
    {
        for (int i = 0; i < InventoryData.inventoryInUsableItemList.Count; i++)
        {
            if (InventoryData.inventoryInUsableItemList[i].id == data.id)
            {
                InventoryData.inventoryInUsableItemList[i] = data;

                SaveToInventoryData();
                return;
            }
        }
    }
    public void AddItemCount(ItemData data)
    {
        for (int i = 0; i < InventoryData.inventoryInUsableItemList.Count; i++)
        {
            if (InventoryData.inventoryInUsableItemList[i].id == data.id)
            {
                ItemData info = InventoryData.inventoryInUsableItemList[i];
                InventoryData.inventoryInUsableItemList[i].currentCnt = Math.Clamp((info.currentCnt + data.currentCnt), 0, info.maxCnt);

                SaveToInventoryData();
                return;
            }
        }
    }

    public List<ItemData> LoadQuestFromInventory()
    {
        return InventoryData.inventoryInQuestItemList;
    }
    public ItemData LoadQuestFromInventory(ItemID id)
    {
        foreach (ItemData info in InventoryData.inventoryInQuestItemList)
        {
            if (info.id == id)
                return info;
        }
        return null;
    }
    public bool HaveQuestItem(ItemID id)
    {
        foreach (ItemData item in InventoryData.inventoryInQuestItemList)
        {
            if (item.id == id)
            {
                return true;
            }
        }
        return false;
    }

    #endregion

}
