using Data;
using Managements.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using Tool.Data.Json;
using UnityEngine;

public class DataManager : Manager
{
    public static User UserData_;
    public static SavePointData SavePointData_;
    public static InventoryData InventoryData_;
    public static WeaponClassLevelDataList WeaponClassLevelListData_;
    public static WeaponLevelDataList WeaponLevelListData_;

    private string URL;
    public override void Awake()
    {
        InventoryData_ = JsonManager.LoadJsonFile<InventoryData>(Application.streamingAssetsPath + "/SAVE/User", "InvectoryData");
        UserData_ = JsonManager.LoadJsonFile<User>(Application.streamingAssetsPath + "/SAVE/User", "UserData");
        SavePointData_ = JsonManager.LoadJsonFile<SavePointData>(Application.streamingAssetsPath + "/SAVE/User", "SavePointData");
        WeaponClassLevelListData_ = JsonManager.LoadJsonFile<WeaponClassLevelDataList>(Application.streamingAssetsPath + "/SAVE/Weapon", "ClassLevelData");
        WeaponLevelListData_ = JsonManager.LoadJsonFile<WeaponLevelDataList>(Application.streamingAssetsPath + "/SAVE/Weapon", "WeaponLevelData");

        if (WeaponClassLevelListData_.weaponClassLevelDataList.Count <= 0)
        {
            CreateWeaponClassListData();
        }

    }
    #region UserData
    public void SaveToUserData()
    {
        string json = JsonManager.ObjectToJson(UserData_);
        JsonManager.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/User", "UserData", json);
    }
    public int GetFeather()
    {
        return UserData_.feather;
    }
    public void SetFeahter(int value)
    {
        UserData_.feather = Math.Clamp(value, 0, int.MaxValue);

        SaveToUserData();
    }
    public void AddFeahter(int value)
    {
        UserData_.feather = Math.Clamp(UserData_.feather + value, 0, int.MaxValue);

        SaveToUserData();
    }

    public void SwapWeaponData()
    {


        SaveToUserData();
    }
    public void ChangeUserWeaponData(string name, bool isFirstWeapon = true)
    {
        if (isFirstWeapon)
        {

        }
        else
        {

        }

        SaveToUserData();
    }
    public void ChangeUserMaxHp(int value)
    {
        UserData_.maxHp = Math.Clamp(value, 100, 1200);

        SaveToUserData();
    }
    public void EquipUsableItem(ItemID id, int equipnumber)
    {
        SaveItemData data = LoadUsableItemFromInventory(id);
        if (data == null)
            Debug.LogError($"Not  Have Item : {id}");

        data.equipNumber = equipnumber;
        UserData_.equipUseableItem.Add(data);

        SaveToUserData();
    }
    public void UnmountItem(int number)
    {
        for (int i = 0; i < UserData_.equipUseableItem.Count; i++)
        {
            if (UserData_.equipUseableItem[i].equipNumber == number)
            {
                UserData_.equipUseableItem.Remove(UserData_.equipUseableItem[i]);
            }
        }

        SaveToUserData();
    }
    public SaveItemData LoadUsableItem(ItemID id)
    {
        foreach (SaveItemData info in UserData_.equipUseableItem)
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
    public void SaveToSavePointData()
    {
        string json = JsonManager.ObjectToJson(SavePointData_);
        JsonManager.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/User", "SavePointData", json);
    }
    public void ChangeCurrentFloor(Floor floor)
    {
        SavePointData_.currentFloor = floor;

        SaveToSavePointData();
    }
    public void OpenFloor(Floor floor)
    {
        SaveToSavePointData();
    }
    #endregion


    #region WeaponLevel
    public void SaveWeaponLevelListData()
    {
        string json = JsonManager.ObjectToJson(WeaponLevelListData_);
        JsonManager.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/Weapon", "WeaponLevelData", json);
    }

    public int LoadWeaponLevelData(ItemID id)
    {
        foreach (WeaponLevelData info in WeaponLevelListData_.weaponLevelDataList)
        {
            if (info.id == id)
                return info.level ;
        }

        return 0;
    }

    public void SaveUpGradeWeaponLevelData(ItemID id,int addLevel = 1)
    {
        for (int i = 0; i < WeaponLevelListData_.weaponLevelDataList.Count; i++)
        {
            if (WeaponLevelListData_.weaponLevelDataList[i].id == id)
            {
                WeaponLevelListData_.weaponLevelDataList[i].level += addLevel;
                SaveWeaponLevelListData();
                return;
            }
        }

        WeaponLevelData data = new WeaponLevelData();
        data.id = id;
        data.level = addLevel;

        WeaponLevelListData_.weaponLevelDataList.Add(data);
        SaveWeaponLevelListData();
    }

    #endregion

    #region WeaponClassData
    public void SaveWeaponClassListData()
    {
        string json = JsonManager.ObjectToJson(WeaponClassLevelListData_);
        JsonManager.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/Weapon", "ClassLevelData", json);
    }
    public void CreateWeaponClassListData()
    {
        WeaponClassLevelListData_.weaponClassLevelDataList.Add(CreateWeaponClassLevel("BasicSword"));
        WeaponClassLevelListData_.weaponClassLevelDataList.Add(CreateWeaponClassLevel("GreateSword"));
        WeaponClassLevelListData_.weaponClassLevelDataList.Add(CreateWeaponClassLevel("TwinSword"));
        WeaponClassLevelListData_.weaponClassLevelDataList.Add(CreateWeaponClassLevel("Spear"));
        WeaponClassLevelListData_.weaponClassLevelDataList.Add(CreateWeaponClassLevel("Bow"));
        SaveWeaponClassListData();
    }

    public WeaponClassLevelData CreateWeaponClassLevel(string name)
    {
        WeaponClassLevelData weaponClass = new WeaponClassLevelData();
        weaponClass.name = name;
        weaponClass.level = 0;
        weaponClass.killedCount = 0;
        return weaponClass;
    }

    public WeaponClassLevelData LoadWeaponClassLevel(string name)
    {
        foreach (WeaponClassLevelData data in WeaponClassLevelListData_.weaponClassLevelDataList)
        {
            if (data.name == name)
            {
                return data;
            }
        }
        return null;
    }

    public void SaveWeaponClassListData(WeaponClassLevelData data)
    {
        for (int i = 0; i < WeaponClassLevelListData_.weaponClassLevelDataList.Count; i++)
        {
            if (WeaponClassLevelListData_.weaponClassLevelDataList[i].name == data.name)
            {
                WeaponClassLevelListData_.weaponClassLevelDataList[i] = data;
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
        string json = JsonManager.ObjectToJson(InventoryData_);
        JsonManager.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/User", "InvectoryData", json);
    }

    //All
    public void AddItemInInventory(ItemID id, int count = 0)
    {
        SaveItemData item = new SaveItemData();
        item.id = id;
        item.currentCnt = count;
        item.name = id.GetType().Name;

        if ((int)id < 101)
        { //Weapon
            if (HaveWeapon(id)) return;
            InventoryData_.inventoryInWeaponList.Add(item);

        }
        else if ((int)id < 201)
        { //Helo
            if (HaveHalo(id)) return;
            InventoryData_.inventoryInHaloList.Add(item);
        }
        else if ((int)id < 301)
        { //UseableItem

            SaveItemData info = LoadUsableItemFromInventory(id);
            if (info == null)
            {
                info = item;
                InventoryData_.inventoryInUsableItemList.Add(item);
            }
            else
            {
                AddItemCount(item);
            }
        }
        else if ((int)item.id < 401)
        { //QuestItem
            if (HaveQuestItem(item.id)) return;
            InventoryData_.inventoryInQuestItemList.Add(item);
        }

        SaveToInventoryData();
    }
    public SaveItemData LoadItemFromInventory(ItemID id)
    {
        if ((int)id < 101)
        { //Weapon
            return LoadWeaponDataFromInventory(id);
        }
        else if ((int)id < 201)
        { //Helo
            return LoadHaloDataFromInventory(id);
        }
        else if ((int)id < 301)
        { //UseableItem
            return LoadUsableItemFromInventory(id);
        }
        else if ((int)id < 401)
        { //QuestItem
            return LoadQuestFromInventory(id);
        }

        return null;
    }

    //WeaponInventory=============================================================================
    public List<SaveItemData> LoadWeaponDataFromInventory()
    {
        return InventoryData_.inventoryInWeaponList;
    }
    public SaveItemData LoadWeaponDataFromInventory(ItemID id)
    {
        foreach (SaveItemData info in InventoryData_.inventoryInWeaponList)
        {
            if (info.id == id)
                return info;
        }
        return null;
    }
    public bool HaveWeapon(ItemID id)
    {
        foreach (SaveItemData weapon in InventoryData_.inventoryInWeaponList)
        {
            if (weapon.id == id)
            {
                return true;
            }
        }
        return false;
    }

    //HaloInventory=============================================================================
    public List<SaveItemData> LoadHaloDataFromInventory()
    {
        return InventoryData_.inventoryInHaloList;
    }
    public SaveItemData LoadHaloDataFromInventory(ItemID id)
    {
        foreach (SaveItemData info in InventoryData_.inventoryInHaloList)
        {
            if (info.id == id)
                return info;
        }
        return null;
    }
    public bool HaveHalo(ItemID id)
    {
        foreach (SaveItemData helo in InventoryData_.inventoryInHaloList)
        {
            if (helo.id == id)
            {
                return true;
            }
        }
        return false;
    }

    //UseableInventory=============================================================================
    public List<SaveItemData> LoadUsableItemFromInventory()
    {
        return InventoryData_.inventoryInUsableItemList;
    }
    public SaveItemData LoadUsableItemFromInventory(ItemID id)
    {
        foreach (SaveItemData info in InventoryData_.inventoryInUsableItemList)
        {
            if (info.id == id)
                return info;
        }
        return null;
    }
    public void ChangeItemInfo(SaveItemData data)
    {
        for (int i = 0; i < InventoryData_.inventoryInUsableItemList.Count; i++)
        {
            if (InventoryData_.inventoryInUsableItemList[i].id == data.id)
            {
                InventoryData_.inventoryInUsableItemList[i] = data;

                SaveToInventoryData();
                return;
            }
        }
    }
    public void AddItemCount(SaveItemData data)
    {
        for (int i = 0; i < InventoryData_.inventoryInUsableItemList.Count; i++)
        {
            if (InventoryData_.inventoryInUsableItemList[i].id == data.id)
            {
                SaveItemData info = InventoryData_.inventoryInUsableItemList[i];
                InventoryData_.inventoryInUsableItemList[i].currentCnt = Math.Clamp((info.currentCnt + data.currentCnt), 0, info.maxCnt);

                SaveToInventoryData();
                return;
            }
        }
    }

    //QuestItemInventory=============================================================================
    public List<SaveItemData> LoadQuestFromInventory()
    {
        return InventoryData_.inventoryInQuestItemList;
    }
    public SaveItemData LoadQuestFromInventory(ItemID id)
    {
        foreach (SaveItemData info in InventoryData_.inventoryInQuestItemList)
        {
            if (info.id == id)
                return info;
        }
        return null;
    }
    public bool HaveQuestItem(ItemID id)
    {
        foreach (SaveItemData item in InventoryData_.inventoryInQuestItemList)
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
