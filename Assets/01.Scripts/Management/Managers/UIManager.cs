using Core;
using Data;
using System.Collections;
using System.Collections.Generic;
using Tool.Data.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialData
{
    public bool weapon = false;
    public bool halo = false;
    public bool useable = false;
    public bool quest = false;
    public bool slider = false;
}
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public UIDocument _document;

    public UIMenu Menu = new UIMenu();
    public UIInGame InGame = new UIInGame();
    public UIInventory Inventory = new UIInventory();
    public UIStatus Status = new UIStatus();
    public UIItemStore ItemStore = new UIItemStore();
    public UISmithy Smithy = new UISmithy();
    public UIBossBar BossBar = new UIBossBar();
    public UIDialog Dialog = new UIDialog();
    public UIFirstFloorMap UIFirstFloorMap = new UIFirstFloorMap();
    public UIPadeInOut PadeInOut = new UIPadeInOut();
    public UIDeathPanel DeathPanel = new UIDeathPanel();
    public UIQuit Quit = new UIQuit();
    public UISettingPanel SettingPanel = new UISettingPanel();
    public UIExplanation Explanation = new UIExplanation();
    public UITutorial Tutorial = new UITutorial();

    public MapNameData MapNameData;

    public WeaponInfoListSO weaponTextInfoListSO;
    public HaloTextInfoListSO haloTextInfoListSO;
    public UseableItemTextInfoListSO useableItemTextInfoListSO;
    public QuestItemTextInfoListSO questItemTextInfoListSO;

    public QuestTextInfoListSO questTextInfoListSO;

    public Dictionary<int, int> levelToAtk = new Dictionary<int, int>()
    {
        { 0,0},
        { 1,20},
        { 2,45},
        { 3,75},
        { 4,110},
        { 5,150},
        { 6,195},
        { 7,245},
        { 8,300},
        { 9,360},
        { 10,425},
        { 11,495},
        { 12,570},
    };
    public Dictionary<int, int> levelTofeather = new Dictionary<int, int>()
    {
        { 0,0},
        { 1,500},
        { 2,800},
        { 3,1000},
        { 4,2500},
        { 5,4500},
        { 6,8500},
        { 7,10000},
        { 8,12500},
        { 9,15000},
        { 10,25000},
        { 11,35000},
        { 12,45000},
    };

    public static TutorialData TutorialData_ = new TutorialData();

    public IntroData introData;

    public bool stopEsc = false;
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

        introData = JsonManager.LoadJsonFile<IntroData>(Application.streamingAssetsPath + "/SAVE/Tutorial", "IntroData");
        TutorialData_ = JsonManager.LoadJsonFile<TutorialData>(Application.streamingAssetsPath + "/SAVE/User", "TutorialData");

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
        Status.Init();
        ItemStore.Init();
        Smithy.Init();
        BossBar.Init();
        Dialog.Init();
        UIFirstFloorMap.Init();
        PadeInOut.Init();
        DeathPanel.Init();
        Quit.Init();
        SettingPanel.Init();
        Explanation.Init();
        Tutorial.Init();
    }


    private void UIStart()
    {
        //InGame.Start();
    }


    private void Update()
    {
        InGame.Update();
        UIFirstFloorMap.Update();
        SettingPanel.Update();

        if (Input.GetKeyDown(KeyCode.Z))
        {

        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Tutorial.Show(0);
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
    public void SaveToTutorialData()
    {
        string json = JsonManager.ObjectToJson(TutorialData_);
        JsonManager.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/User", "TutorialData", json);
    }


    public void Escape()
    {
        if (stopEsc) return;
        if(OpenPanels.Count <= 0)
        {
            Menu.Show();
            return;
        }

        MoveAndInputPlay();
        UIBase ui = OpenPanels.Pop();
        if(ui is UIMenu || ui is UIStatus || ui is UIQuit || ui is UIExplanation)
        {
            ui.Hide();
            return;
        }

        PadeInOut.Pade(PadeType.padeDown, () => { ui.Hide(); });
        
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

    public void ShowItemPanel()
    {
        InGame.ItemPanel();
        Explanation.Show(ItemID.HPPotion);
    }
    public void ShowExplanationUI(int pageNum)
    {
        Explanation.Show(pageNum);
    }
    
    public void ClearIntroData()
    {
        introData.isClearTutorial = true;
        string json = JsonManager.ObjectToJson(introData);
        JsonManager.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/Tutorial", "IntroData",json);
    }
}
