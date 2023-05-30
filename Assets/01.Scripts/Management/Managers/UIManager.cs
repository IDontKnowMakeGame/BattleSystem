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
        Define.GetManager<DataManager>().AddItemInInventory(ItemID.CompassOfSpace);
        Define.GetManager<DataManager>().AddItemInInventory(ItemID.HaloOfTime);
        Define.GetManager<DataManager>().AddItemInInventory(ItemID.ExecutionBlade);
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
            
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            Dialog.NextMessage();
        }

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
}
