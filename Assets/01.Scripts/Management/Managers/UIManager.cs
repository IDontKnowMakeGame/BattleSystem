using Core;
using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public UIDocument _document;

    public UIInGame InGame = new UIInGame();
    public UIInventory Inventory = new UIInventory();
    public UIItemStore ItemStore = new UIItemStore();
    public UISmithy Smithy = new UISmithy();
    public UIBossBar BossBar = new UIBossBar();
    public UIDialog Dialog = new UIDialog();
    public UIFirstFloorMap UIFirstFloorMap = new UIFirstFloorMap();

    public MapNameData MapNameData;
    public HaloTextInfoListSO haloTextInfoListSO;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Mutiple UIManager");
        }
        Instance = this;

        _document = GetComponent<UIDocument>();

        Init();
    }

    private void Start()
    {
        //Define.GetManager<DataManager>().AddItemInInventory(ItemID.CompassOfSpace);
        //Define.GetManager<DataManager>().AddItemInInventory(ItemID.HaloOfTime);
        //Define.GetManager<DataManager>().AddItemInInventory(ItemID.ExecutionBlade);
        //Define.GetManager<DataManager>().AddItemInInventory(ItemID.Ascalon);
        //Define.GetManager<DataManager>().AddItemInInventory(ItemID.SecondMap);
        //Define.GetManager<DataManager>().AddItemInInventory(ItemID.Pick,1);
        UIStart();
    }

    private void Init()
    {
        InGame.Init();
        Inventory.Init();
        ItemStore.Init();
        Smithy.Init();
        BossBar.Init();
        Dialog.Init();
        UIFirstFloorMap.Init();
    }

    private void UIStart()
    {
        //InGame.Start();
    }


    private void Update()
    {
        InGame.Update();
        UIFirstFloorMap.Update();

        if (Input.GetKeyDown(KeyCode.Z))
        {
           InGame.AddAbnormalStatus();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Inventory.ShowInventory();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log($" open QuestList Cnt : {DataManager.PlayerOpenQuestData_.openQuestList[0]}");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            Dialog.NextMessage();
        }

    }
    public void MoveAndInputPlay()
    {
        Core.InGame.Player.GetAct<PlayerEquipment>().WeaponOnOff(false);
    }
    public void MoveAndInputStop()
    {
        Core.InGame.Player.GetAct<PlayerEquipment>().WeaponOnOff(true);
    }
    public void UpdateInGameUI()
    {
        InGame.ChangeFirstWeaponImage(DataManager.UserData_.firstWeapon);
        InGame.ChangeSecondWeaponImage(DataManager.UserData_.secondWeapon);
    }
    public void RoomInCristal(int num)
    {
        InGame.CristalInfoInRoom(num);
    }
    public  void show(string name)
    {
        BossBar.ShowBossBar(name);
    }
    public int LevelToAtk(int level)
    {
        int value = 0;
        switch (level)
        {
            case 0:
                value = 0;
                break;
            case 1:
                value = 20;
                break;
            case 2:
                value = 45;
                break;
            case 3:
                value = 75;
                break;
            case 4:
                value = 110;
                break;
            case 5:
                value = 150;
                break;
            case 6:
                value = 195;
                break;
            case 7:
                value = 245;
                break;
            case 8:
                value = 300;
                break;
            case 9:
                value = 360;
                break;
            case 10:
                value = 425;
                break;
            case 11:
                value = 495;
                break;
            case 12:
                value = 570;
                break;
            default:
                break;
        }

        return value;
    }
    public int LevelToFeather(int level)
    {
        int value = 0;
        switch (level)
        {
            case 0:
                value = 0;
                break;
            case 1:
                value = 500;
                break;
            case 2:
                value = 800;
                break;
            case 3:
                value = 1000;
                break;
            case 4:
                value = 2500;
                break;
            case 5:
                value = 4500;
                break;
            case 6:
                value = 8500;
                break;
            case 7:
                value = 10000;
                break;
            case 8:
                value = 12500;
                break;
            case 9:
                value = 15000;
                break;
            case 10:
                value = 25000;
                break;
            case 11:
                value = 35000;
                break;
            case 12:
                value = 45000;
                break;
            default:
                break;
        }
        return value;
    }
}
