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

    }

    private void Init()
    {
        InGame.Init();
        Inventory.Init();
        ItemStore.Init();
        Smithy.Init();
        BossBar.Init();
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.X))
        //{
        //    ItemStore.ShowItemStore(itemStoreTable);
        //}
        if (Input.GetKeyDown(KeyCode.C))
        {
            Inventory.ShowInventory();
        }
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    Smithy.ShowSmithy();
        //}
    }

    public  void show(string name)
    {
        BossBar.ShowBossBar(name);
    }
}
