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

    public ItemStoreTableSO itemStoreTable;


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
        //Define.GetManager<DataManager>().AddItemInInventory(ItemID.FirstMap);
        //Define.GetManager<DataManager>().AddItemInInventory(ItemID.SecondMap);
        //Define.GetManager<DataManager>().AddItemInInventory(ItemID.ThirdMap);
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ItemStore.ShowItemStore(itemStoreTable);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Inventory.ShowInventory();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Smithy.ShowSmithy();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            UIFirstFloorMap.ShowFirstFloorMap();
        }
    }
    public void UpdateInGameUI()
    {
        InGame.ChangeFirstWeaponImage(DataManager.UserData_.firstWeapon);
        InGame.ChangeSecondWeaponImage(DataManager.UserData_.secondWeapon);
    }
    public  void show(string name)
    {
        BossBar.ShowBossBar(name);
    }
}
