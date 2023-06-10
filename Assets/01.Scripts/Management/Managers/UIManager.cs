using Core;
using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public UIDocument _document;

    public UIMenu Menu = new UIMenu();
    public UIInGame InGame = new UIInGame();
    public UIInventory Inventory = new UIInventory();
    public UIItemStore ItemStore = new UIItemStore();
    public UISmithy Smithy = new UISmithy();
    public UIBossBar BossBar = new UIBossBar();
    public UIDialog Dialog = new UIDialog();
    public UIFirstFloorMap UIFirstFloorMap = new UIFirstFloorMap();
    public UIPadeInOut PadeInOut = new UIPadeInOut();

    public MapNameData MapNameData;

    public WeaponInfoListSO weaponTextInfoListSO;
    public HaloTextInfoListSO haloTextInfoListSO;
    public UseableItemTextInfoListSO useableItemTextInfoListSO;
    public QuestItemTextInfoListSO questItemTextInfoListSO;

    #region Escape
    public static Stack<UIBase> OpenPanels = new Stack<UIBase>();
    #endregion


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
        //Define.GetManager<DataManager>().AddItemInInventory(ItemID.AngelWingFragment);
        //Define.GetManager<DataManager>().AddItemInInventory(ItemID.AngelEyes);
        //Define.GetManager<DataManager>().AddItemInInventory(ItemID.ExecutionBlade);
        //Define.GetManager<DataManager>().AddItemInInventory(ItemID.Ascalon);
        //Define.GetManager<DataManager>().AddItemInInventory(ItemID.SecondMap);
        
        UIStart();
    }

    private void Init()
    {
        Menu.Init();
        InGame.Init();
        Inventory.Init();
        ItemStore.Init();
        Smithy.Init();
        BossBar.Init();
        Dialog.Init();
        UIFirstFloorMap.Init();
        PadeInOut.Init();
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
    
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Define.GetManager<DataManager>().AddItemInInventory(ItemID.Pick, 3);
            Define.GetManager<DataManager>().AddItemInInventory(ItemID.Torch, 2);
            Define.GetManager<DataManager>().AddItemInInventory(ItemID.Shield, 2);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Escape();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            Dialog.NextMessage();
        }
    }
    private void Escape()
    {
        if(OpenPanels.Count <= 0)
        {
            Menu.Show();
            return;
        }

        UIBase ui = OpenPanels.Pop();
        if(ui is UIMenu)
        {
            ui.Hide();
            return;
        }

        PadeInOut.Pade(1, () => { ui.Hide(); });
        
    }
    public void HideAllUI()
    {
        InGame.Hide();
    }
    public void ShowAllUI()
    {
        InGame.Show();
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
