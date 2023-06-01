using Data;
using Managements.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using Tool.Data.Json;
using Tool.Data.Json.ParsingList;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class DataManager : Manager
{
    public static User UserData_;
    public static InventoryData InventoryData_;
    public static MapData MapData_;
    public static WeaponClassLevelDataList WeaponClassLevelListData_;
    public static WeaponLevelDataList WeaponLevelListData_;
    public static ItemTable ItemTableData;
    public static PlayerQuestData PlayerOpenQuestData_;
    
    public Dictionary<ItemID, ItemInfo> weaponDictionary = new Dictionary<ItemID, ItemInfo>();

    public override void Awake()
    {
        InventoryData_ = JsonManager.LoadJsonFile<InventoryData>(Application.streamingAssetsPath + "/SAVE/User", "InvectoryData");
        UserData_ = JsonManager.LoadJsonFile<User>(Application.streamingAssetsPath + "/SAVE/User", "UserData");
        MapData_ = JsonManager.LoadJsonFile<MapData>(Application.streamingAssetsPath + "/SAVE/User", "MapData");
        WeaponClassLevelListData_ = JsonManager.LoadJsonFile<WeaponClassLevelDataList>(Application.streamingAssetsPath + "/SAVE/Weapon", "ClassLevelData");
        WeaponLevelListData_ = JsonManager.LoadJsonFile<WeaponLevelDataList>(Application.streamingAssetsPath + "/SAVE/Weapon", "WeaponLevelData");
        ItemTableData = JsonManager.LoadJsonFile<ItemTable>(Application.streamingAssetsPath + "/Save/Json/" + typeof(ItemTable), typeof(ItemTable).ToString());
        PlayerOpenQuestData_ = JsonManager.LoadJsonFile<PlayerQuestData>(Application.streamingAssetsPath + "/SAVE/User", "OpenQuest");
        //Debug.Log($"QuestData Load : {PlayerOpenQuestData_.openQuestList[0]}");

        if(MapData_.mapData.Count == 0)
            InitMapData();

        if (WeaponClassLevelListData_.weaponClassLevelDataList.Count == 0)
            CreateWeaponClassListData();

        WeaponInfoSerialize();
    }
    public void WeaponInfoSerialize()
    {
        foreach(ItemInfo item in ItemTableData.ItemList)
        {
            weaponDictionary[item.Id] = item;
        }
    }
    public ItemInfo GetWeaponData(ItemID id)
    {
        return weaponDictionary[id];
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
        ItemID temp = UserData_.firstWeapon;
        UserData_.firstWeapon = UserData_.secondWeapon;
        UserData_.secondWeapon = temp;

        SaveToUserData();
    }
    public void ChangeUserWeaponData(ItemID item,int num = 1 )
    {
        if (num == 1)
            UserData_.firstWeapon = item;
        else if (num == 2) 
            UserData_.secondWeapon = item;

        SaveToUserData();
    }
    public void ChangeUserMaxHp(int value)
    {
        UserData_.maxHp = Math.Clamp(value, 100, 1200);

        SaveToUserData();
    }
    public void EquipUsableItem(ItemID id, int equipnumber)
    {
        if (!HaveUseableItem(id))
        {
            Debug.LogError($"Not  Have Item : {id} + {(int)id}");
            return;
        }
        
        switch(equipnumber)
        {
            case 1:
                UserData_.equipUseableItem.first = id;
                break;
            case 2:
                UserData_.equipUseableItem.second = id;
                break;
            case 3:
                UserData_.equipUseableItem.third = id;
                break;
            case 4:
                UserData_.equipUseableItem.fourth = id;
                break;
            case 5:
                UserData_.equipUseableItem.fifth = id;
                break;
            default:
                Debug.LogError($"Over Input Equip Number : {equipnumber} -> 1 ~ 5 ");
                break;
        }

        SaveToUserData();
    }
    public void UnmountUseableItem(int number)
    {
        switch (number)
        {
            case 1:
                UserData_.equipUseableItem.first = ItemID.None;
                break;
            case 2:
                UserData_.equipUseableItem.second = ItemID.None;
                break;
            case 3:
                UserData_.equipUseableItem.third = ItemID.None;
                break;
            case 4:
                UserData_.equipUseableItem.fourth = ItemID.None;
                break;
            case 5:
                UserData_.equipUseableItem.fifth = ItemID.None;
                break;
            default:
                Debug.LogError($"Over Input Equip Number : {number} -> 1 ~ 9 ");
                break;
        }

        SaveToUserData();
    }
    public EquipUesableItemSetting LoadUsableItem()
    {
        return UserData_.equipUseableItem;
    }
    public List<ItemID> LoadUsableItemList()
    {
        List<ItemID> list = new List<ItemID>();
        list.Add(UserData_.equipUseableItem.first);
        list.Add(UserData_.equipUseableItem.second);
        list.Add(UserData_.equipUseableItem.third);
        list.Add(UserData_.equipUseableItem.fourth);
        list.Add(UserData_.equipUseableItem.fifth);
        return list;
    }
    public void EquipHalo(ItemID id,int equipNum)
    {
        if (!HaveHalo(id) && id != ItemID.None)
        {
            Debug.LogError($"Not  Have Halo : {id} + {(int)id}");
            return;
        }

        switch (equipNum)
        {
            case 1:
                UserData_.firstHalo = id;
                break;
            case 2:
                UserData_.secondHalo = id;
                break;
            case 3:
                UserData_.thirdHalo = id;
                break;
            default:
                Debug.LogError($"Not Equip Halo is Number : {equipNum}");
                break;
        }

        UIManager.Instance.InGame.ChangeHalo(id);

        SaveToUserData();
    }

    public List<ItemID> LoadHaloListInUserData()
    {
        List<ItemID> list = new List<ItemID>();
        list.Add(UserData_.firstHalo);
        list.Add(UserData_.secondHalo);
        list.Add(UserData_.thirdHalo);
        return list;
    }
    #endregion

    #region MapData
    public void SaveToMapData()
    {
        string json = JsonManager.ObjectToJson(MapData_);
        JsonManager.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/User", "MapData", json);
    }
    public void InitMapData()
    {
        MapData_.mapData.Clear();
        MapData_.mapData.Add(new MapInfo() { floor = Floor.Lobby});
        MapData_.mapData.Add(new MapInfo() { floor = Floor.First });
        MapData_.mapData.Add(new MapInfo() { floor = Floor.Second });
        MapData_.mapData.Add(new MapInfo() { floor = Floor.Third });

        SaveToMapData();
    }
    public MapInfo CurrentMapData()
    {
        return MapData_.mapData[(int)MapData_.currentFloor];
    }

    public List<int> LoadOnCristalData(Floor floor)
    {
        MapInfo map = MapData_.mapData[(int)floor];
        return map.onCristalList;
    }
    public void OnCrital(int num)
    {
        MapData_.mapData[(int)MapData_.currentFloor].onCristalList.Add(num);

        UIManager.Instance.InGame.UpdateCristalText();

        SaveToMapData();
    }
    public bool IsOnCrital(int num,Floor floor)
    {

        foreach(int number in LoadOnCristalData(floor))
        {
            if(number == num) return true;
        }
        return false;
    }

    public List<int> LoadOpenChestData(Floor floor)
    {
        MapInfo map = MapData_.mapData[(int)floor];
        return map.openChestList;
    }
    public void OpenChest(int num)
    {
        if (IsOpenChest(num, MapData_.currentFloor)) return;

        MapData_.mapData[(int)MapData_.currentFloor].openChestList.Add(num);

        SaveToMapData();
    }
    public bool IsOpenChest(int num, Floor floor)
    {

        foreach (int number in LoadOpenChestData(floor))
        {
            if (number == num) return true;
        }
        return false;
    }

    public List<int> LoadOpenDoorList(Floor floor)
    {
        MapInfo map = MapData_.mapData[(int)floor];
        return map.openDoorList;
    }
    public void OpenDoor(int num)
    {
        if (IsOpenDoorl(num, MapData_.currentFloor)) return;

        MapData_.mapData[(int)MapData_.currentFloor].openDoorList.Add(num);

        SaveToMapData();
    }
    public bool IsOpenDoorl(int num, Floor floor)
    {

        foreach (int number in LoadOpenDoorList(floor))
        {
            if (number == num) return true;
        }
        return false;
    }

    public List<int> LoadBrokenWallList(Floor floor)
    {
        MapInfo map = MapData_.mapData[(int)floor];
        return map.brokenWallList;
    }
    public void BrokenWall(int num)
    {
        if (IsBrokenWall(num, MapData_.currentFloor)) return;

        MapData_.mapData[(int)MapData_.currentFloor].brokenWallList.Add(num);

        SaveToMapData();
    }
    public bool IsBrokenWall(int num, Floor floor)
    {

        foreach (int number in LoadBrokenWallList(floor))
        {
            if (number == num) return true;
        }
        return false;
    }

    public void EnterFloor(Floor floor)
    {
        MapData_.currentFloor = floor;
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
        WeaponClassLevelListData_.weaponClassLevelDataList.Add(CreateWeaponClassLevel("StraightSword"));
        WeaponClassLevelListData_.weaponClassLevelDataList.Add(CreateWeaponClassLevel("GreatSword"));
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
    public void AddWeaponClassKillData(string className,int addKillcount = 1, int addlevel = 0)
    {
        for (int i = 0; i < WeaponClassLevelListData_.weaponClassLevelDataList.Count; i++)
        {
            if (WeaponClassLevelListData_.weaponClassLevelDataList[i].name == className)
            {
                WeaponClassLevelListData_.weaponClassLevelDataList[i].killedCount += addKillcount;
                WeaponClassLevelListData_.weaponClassLevelDataList[i].level += addlevel;
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
    public void AddItemInInventory(ItemID id, int count = 1)
    {
        SaveItemData item = new SaveItemData();
        item.id = id;
        item.currentCnt = count;
        item.maxCnt = 99;
        item.name = id.ToString();

        if ((int)id < 100)
        { //Weapon
            if (HaveWeapon(id)) return;
            InventoryData_.inventoryInWeaponList.Add(item);

        }
        else if ((int)id < 200)
        { //Helo
            if (HaveHalo(id)) return;
            InventoryData_.inventoryInHaloList.Add(item);
        }
        else if ((int)id < 300)
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
        else if ((int)item.id < 400)
        { //QuestItem
            Debug.Log(InventoryData_.inventoryInQuestItemList.Count);
            if (HaveQuestItem(item.id)) return;
            InventoryData_.inventoryInQuestItemList.Add(item);
            Debug.Log(item.id);
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.InGame.AddShowItemPanel(id, count);
            UIManager.Instance.InGame.ChangeItemPanelImage();
        }
            

        SaveToInventoryData();
    }
    public SaveItemData LoadItemFromInventory(ItemID id)
    {
        if(id == ItemID.None) return null;

        if ((int)id < 100)
        { //Weapon
            return LoadWeaponDataFromInventory(id);
        }
        else if ((int)id < 200)
        { //Helo
            return LoadHaloDataFromInventory(id);
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
    public bool HaveUseableItem(ItemID id)
    {
        foreach (SaveItemData item in InventoryData_.inventoryInUsableItemList)
        {
            Debug.Log($"{item.name} + {(int)item.id} : {id} + {(int)id}");
            if (item.id == id)
            {
                return true;
            }
        }
        return false;
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

    public static bool HaveQuestItem(ItemID id)
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

    #region PlayerOpenQuest
    public void SaveToOpenQuestData()
    {
        string json = JsonManager.ObjectToJson(PlayerOpenQuestData_);
        JsonManager.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/User", "OpenQuest", json);
    }

    public void ReadyQuest(QuestName name)
    {
        if (IsReadyQuest(name))
        {
            Debug.LogError($"Already Ready Quest Number : {(int)name} Name : {name}");
            return;
        }
        Debug.Log($"Ready Quest Number : {(int)name} Name : {name}");
        PlayerOpenQuestData_.readyQuestList.Add(name);

        SaveToOpenQuestData();
    }
    public void OpenQuest(QuestName name)
    {
        if(IsOpenQuest(name))
        {
            Debug.LogError($"Already Open Quest Number : {(int)name} Name : {name}");
            return;
        }
        Debug.Log($"Open Quest Number : {(int)name} Name : {name}");
        PlayerOpenQuestData_.openQuestList.Add(name);
        PlayerOpenQuestData_.readyQuestList.Remove(name);

        UIManager.Instance.InGame.AddQuestPanel(name);
        QuestManager.Instance.AddQuestCheck();

        SaveToOpenQuestData();
    }
    public void ReadyClearQuest(QuestName name)
    {
        if (IsReadyClearQuest(name))
        {
            Debug.LogError($"Already IsReadyClear Quest Number : {(int)name} Name : {name}");
        }
        Debug.Log($"IsReadyClear Quest Number : {(int)name} Name : {name}");
        PlayerOpenQuestData_.readyClearQuestList.Add(name);
        PlayerOpenQuestData_.openQuestList.Remove(name);

        UIManager.Instance.InGame.ClearQuestPanel(name);

        SaveToOpenQuestData();
    }
    public void ClearQuest(QuestName name)
    {
        if (IsClearQuest(name))
        {
            Debug.LogError($"Already Clear Quest Number : {(int)name} Name : {name}");
        }
        Debug.Log($"Clear Quest Number : {(int)name} Name : {name}");

        PlayerOpenQuestData_.clearQuestList.Add(name);
        PlayerOpenQuestData_.readyClearQuestList.Remove(name);

        UIManager.Instance.InGame.CloseQuestPanel(name);
        SaveToOpenQuestData();
    }
    public bool IsReadyQuest(QuestName name)
    {
        foreach (QuestName quest in PlayerOpenQuestData_.readyQuestList)
        {
            if (quest == name)
                return true;
        }
        return false;
    }
    public bool IsOpenQuest(QuestName name)
    {
        foreach(QuestName quest in PlayerOpenQuestData_.openQuestList)
        {
            if(quest == name)
                return true;
        }
        return false;
    }
    public bool IsReadyClearQuest(QuestName name)
    {
        foreach (QuestName quest in PlayerOpenQuestData_.readyClearQuestList)
        {
            if (quest == name)
                return true;
        }
        return false;
    }
    public bool IsClearQuest(QuestName name)
    {
        foreach (QuestName quest in PlayerOpenQuestData_.clearQuestList)
        {
            if (quest == name)
                return true;
        }
        return false;
    }
    #endregion
}
