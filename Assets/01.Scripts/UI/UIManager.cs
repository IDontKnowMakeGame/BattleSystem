using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Managements.Managers.Base;
using Core;
using System;

public class UIManager : Manager
{
    static GameObject ob;
    private UIDocument _document;

    private enum MyUI
    {
        InGame,
        Dialog,
        Inventory,
        WeaponStore,
        ItemStore,
        None
    }

    private static MyUI currentUI = MyUI.None;

    private VisualElement _exitBtn;

    #region InGame
    private bool _onOtherPanel = false;
    private VisualElement _hpBar;
    private VisualElement _angerBar;
    private VisualElement _adranalineBar;

    private VisualElement _firstWeaponIamge;
    private VisualElement _secondWeaponImage;

    private VisualElement _potionImage;
    private VisualElement _itemImage;

    private VisualElement _featherPanel;
    #endregion

    #region Dialog
    private VisualElement _sentencePanel;
    private VisualElement _choicePanel;
    #endregion

    #region WeaponStore
    private string _selectWeaponName;
    private VisualTreeAsset _weaponCardTemp;
    private VisualElement _weaponImage;
    private Label _weaponName;

    private Label _atkText;
    private Label _atsText;
    private Label _afsText;
    private Label _weightText;

    private Label _needMoneyText;
    private Label _needItemText;

    private ScrollView _weaponScroll;
    private VisualElement _weaponUpgradeBtn;
    #endregion

    #region ItemStore
    private int _currentHaveMoney;
    private int _currentSelectItemCode = 0;
    private int _itemPurchaseMoney;
    private int _itempurchaseCnt;

    private ItemStoreTableSO _currentItemTable;

    private VisualTreeAsset _itemCardTemp;

    private VisualElement _itemSave;

    private Label _currenHavetMoneyText;
    private Label _beforeMoneyText;

    private VisualElement _minusBtn;
    private VisualElement _addBtn;
    private Label _cntText;

    private VisualElement _purchaseBtn;
    #endregion

    #region Inventory
    private enum EquipType
    {
        None,
        Helo,
        Weapon,
        UseableItem
    }

    private EquipType boxType = EquipType.None;
    private int _selectNum;
    private VisualElement _selectVisual;
    

    private VisualTreeAsset _itemInfoCardTemp;

    private VisualElement _heloPanel;
    private VisualElement _firstWeaponPanel;
    private VisualElement _secondWeaponPanel;
    private VisualElement _itemPanel;

    private VisualElement _weaponBtn;
    private VisualElement _useableItemBtn;
    private VisualElement _questItemBtn;
    private VisualElement _heloBtn;

    private VisualElement _scrolls;

    private VisualElement _weaponListScroll;
    private VisualElement _heloListScroll;
    private VisualElement _useableListScroll;
    private VisualElement _questItemListScroll;
    #endregion

    public override void Awake()
    {
        if(ob == null)
        {
            ob = new GameObject();
            ob.AddComponent<UIDocument>();
        }
        _document = ob.GetComponent<UIDocument>();
        _document.panelSettings = Define.GetManager<ResourceManagers>().Load<PanelSettings>("UI Toolkit/PanelSettings");

        InGameInit();
    }
    public void InGameInit()
    {
        if (currentUI == MyUI.InGame) return;


        _onOtherPanel = false;
        currentUI = MyUI.InGame;
        _document.visualTreeAsset = Define.GetManager<ResourceManagers>().Load<VisualTreeAsset>("UIDoc/InGame");
        VisualElement root = _document.rootVisualElement;

        _hpBar = root.Q<VisualElement>("HpBar");
        _angerBar = root.Q<VisualElement>("AngerBar");
        _adranalineBar = root.Q<VisualElement>("AdrenalineBar");

       // _firstWeaponIamge = root.Q<VisualElement>("AdrenalineBar");
        //_secondWeaponImage = root.Q<VisualElement>("AdrenalineBar");
    }
    public void DialogInit()
    {
        if (currentUI == MyUI.Dialog) return;

        currentUI = MyUI.Dialog;
        _document.visualTreeAsset = Define.GetManager<ResourceManagers>().Load<VisualTreeAsset>("UIDoc/Dialog");
        VisualElement root = _document.rootVisualElement;

        _sentencePanel = root.Q<VisualElement>("TextBox");
        _choicePanel = root.Q<VisualElement>("ChoicePanel");
    }
    public void WeaponStoreInit()
    {
        if (currentUI == MyUI.WeaponStore) return;

        _onOtherPanel = false;

        currentUI = MyUI.WeaponStore;
        _document.visualTreeAsset = Define.GetManager<ResourceManagers>().Load<VisualTreeAsset>("UIDoc/WeaponStore");
        VisualElement root = _document.rootVisualElement;

        _weaponCardTemp = Define.GetManager<ResourceManagers>().Load<VisualTreeAsset>("UIDoc/WeaponCardTemp");

        _weaponScroll = root.Q<ScrollView>("WeaponScroll");
        _weaponName = root.Q<Label>("WeaponName");

        _exitBtn = root.Q<VisualElement>("ExitBtn");
        _exitBtn.RegisterCallback<ClickEvent>(e =>
        {
            InGameInit();
        });

        _weaponUpgradeBtn = root.Q<VisualElement>("Btn");
        _weaponUpgradeBtn.RegisterCallback<ClickEvent>(e =>
        {
            WeaponUpGradeBtn();
        });

        VisualElement rightWindow = root.Q<VisualElement>("RightWindow");

        _atkText = rightWindow.Q<Label>("AtkBoxLabel");
        _atsText = rightWindow.Q<Label>("AtsBoxLabel");
        _afsText = rightWindow.Q<Label>("AfsBoxLabel");
        _weightText = rightWindow.Q<Label>("WeightBoxLabel");

        _needMoneyText = rightWindow.Q<Label>("NeedMoneyBox/Label");
        _needItemText = rightWindow.Q<Label>("NeedItemBox/Label");
    }
    public void ItemStoreInit()
    {
        if (currentUI == MyUI.ItemStore) return;

        _onOtherPanel = false;
        currentUI = MyUI.ItemStore;
        _document.visualTreeAsset = Define.GetManager<ResourceManagers>().Load<VisualTreeAsset>("UIDoc/ItemStore");
        VisualElement root = _document.rootVisualElement;

        _itemCardTemp = Define.GetManager<ResourceManagers>().Load<VisualTreeAsset>("UIDoc/ItemCardTemp");

        _itemSave = root.Q<VisualElement>("ItemCardSavePanel");

        _exitBtn = root.Q<VisualElement>("ExitBtn");
        _exitBtn.RegisterCallback<ClickEvent>(e =>
        {
            InGameInit();
        });

        _currenHavetMoneyText = root.Q<Label>("CurrentMoneyText");
        _beforeMoneyText = root.Q<Label>("BeforeMoneyText");

        _minusBtn = root.Q<VisualElement>("MinusBtn");
        _minusBtn.RegisterCallback<ClickEvent>(e =>
        {
            MinusItemBtn();
        });
        _addBtn = root.Q<VisualElement>("AddBtn");
        _addBtn.RegisterCallback<ClickEvent>(e =>
        {
            AddItemBtn();
        });
        _cntText = root.Q<Label>("CntText");

        _purchaseBtn = root.Q<VisualElement>("PurchaseBtn");
        _purchaseBtn.RegisterCallback<ClickEvent>(e =>
        {
            PurchaseBtn();
        });
    }
    public void InventoryInit()
    {
        if (currentUI == MyUI.Inventory) return;

        _onOtherPanel = false;
        currentUI = MyUI.Inventory;
        _document.visualTreeAsset = Define.GetManager<ResourceManagers>().Load<VisualTreeAsset>("UIDoc/Inventory");
        VisualElement root = _document.rootVisualElement;

        _exitBtn = root.Q<VisualElement>("ExitBtn");
        _exitBtn.RegisterCallback<ClickEvent>(e =>
        {
            InGameInit();
        });

        _itemInfoCardTemp = Define.GetManager<ResourceManagers>().Load<VisualTreeAsset>("UIDoc/ItemInfoCardTemp");

        _heloPanel = root.Q<VisualElement>("HeloPanel");
        _firstWeaponPanel = root.Q<VisualElement>("FirstWeaponPanel");
        _secondWeaponPanel = root.Q<VisualElement>("SecondWeaponPanel");
        _itemPanel = root.Q<VisualElement>("ItemPanel");

        _weaponBtn = root.Q<VisualElement>("WeaponBtn");
        _weaponBtn.RegisterCallback<ClickEvent>(e =>
        {
            ChangeShowInventoryPanel(0);
        });
        _heloBtn = root.Q<VisualElement>("HeloBtn");
        _heloBtn.RegisterCallback<ClickEvent>(e =>
        {
            ChangeShowInventoryPanel(1);
        });
        _useableItemBtn = root.Q<VisualElement>("UseableItemBtn");
        _useableItemBtn.RegisterCallback<ClickEvent>(e =>
        {
            ChangeShowInventoryPanel(2);
        });
        _questItemBtn = root.Q<VisualElement>("QuestItemBtn");
        _questItemBtn.RegisterCallback<ClickEvent>(e =>
        {
            ChangeShowInventoryPanel(3);
        });

        _scrolls = root.Q<VisualElement>("Scrolls");

        _weaponListScroll = _scrolls.Q<VisualElement>("WeaponList");
        _heloListScroll = _scrolls.Q<VisualElement>("HeloList");
        _useableListScroll = _scrolls.Q<VisualElement>("UseableItemList");
        _questItemListScroll = _scrolls.Q<VisualElement>("QuestItemList");
    }

    #region HpSlider
    public void SetMaxHpValue(int value)
    {
        
        if (_onOtherPanel) return;
        InGameInit();

        VisualElement fill = _hpBar.Q<VisualElement>("BackGround");
        fill.style.width = new Length(value, LengthUnit.Percent);
    }
    public void SetHpValue(int value)
    {
        if (_onOtherPanel) return;
        InGameInit();

        VisualElement fill = _hpBar.Q<VisualElement>("Fill");
        fill.style.width = new Length(value, LengthUnit.Percent);
    }
    #endregion

    #region AngerSlider
    public void SetAngerValue(float value)
    {
        if (_onOtherPanel) return;
        InGameInit();

        VisualElement fill = _angerBar.Q<VisualElement>("Fill");
        fill.style.width = new Length(value, LengthUnit.Percent);
    }
    #endregion

    #region AdranalineSlider
    public void SetAdranalineValue(float value)
    {
        if (_onOtherPanel) return;
        InGameInit();

        VisualElement fill = _adranalineBar.Q<VisualElement>("Fill");
        fill.style.width = new Length(value, LengthUnit.Percent);
    }
    #endregion

    #region Dialog
    public void CreateDialog(string text)
    {
        DialogInit();

        Label labeltext = _sentencePanel.Q<Label>("Label");
        labeltext.text = text;
    }

    public void CreateChoiceBox(string text,Action action)
    {
        DialogInit();

        VisualTreeAsset temple = Define.GetManager<ResourceManagers>().Load<VisualTreeAsset>("UIDoc/ChoiceBox");
        VisualElement choiceBox = temple.Instantiate();
        Label label = choiceBox.Q<Label>("Text");
        label.text = text;

        choiceBox.RegisterCallback<ClickEvent>(e =>
        {
            action();
        });

        _choicePanel.Add(choiceBox);
    }
    #endregion

    #region WeaponStore
    public void ShowWeaponStore()
    {
        WeaponStoreInit();
        if (_onOtherPanel) return;
        _onOtherPanel = true;

        List<string> dataList = Define.GetManager<DataManager>().LoadWeaponData();
        
        foreach (string name in dataList)
        {
            VisualElement box = _weaponCardTemp.Instantiate();
            box.RegisterCallback<ClickEvent>(e => {
                SelectWeaponBtn(name);
            });
            _weaponScroll.Add(box);
        }
    }

    public void SelectWeaponBtn(string name)
    {
        _selectWeaponName = name;
        _weaponName.text = _selectWeaponName;

        WeaponStateData data = Define.GetManager<DataManager>().LoadWeaponStateData(name);
        Debug.Log(data.damage);
        _atkText.text = string.Format("{0}", data.damage);
        _atsText.text = string.Format("{0}", data.attackSpeed);
        _afsText.text = string.Format("{0}", data.attackAfterDelay);
        _weightText.text = string.Format("{0}", data.weaponWeight);
    }

    public void WeaponUpGradeBtn()
    {
        Define.GetManager<DataManager>().SaveUpGradeWeaponLevelData(_selectWeaponName);
        //Define.GetManager<EventManager>().TriggerEvent(EventFlag.weapon)
    }
    #endregion

    #region ItemStore
    public void ShowItemStore(ItemStoreTableSO table)
    {
        ItemStoreInit();
        if (_onOtherPanel) return;
        _onOtherPanel = true;

        _currentItemTable = table;

        foreach(ItemPrice data in _currentItemTable.table)
        {
            VisualElement card = _itemCardTemp.Instantiate();
            card.RegisterCallback<ClickEvent>(e =>
            {
                ChangeItemCard(data);
            });
            _itemSave.Add(card);
        }
        
        _currentHaveMoney = Define.GetManager<DataManager>().GetFeather();
        _itemPurchaseMoney = 0;
        _itempurchaseCnt = 0;

        UpdateUI();
    }
    public void AddItemBtn()
    {
        if(_currentSelectItemCode == 0)
        {
            return;
        }

        _itempurchaseCnt++;

        UpdateUI();
    }
    public void MinusItemBtn()
    {
        if (_currentSelectItemCode == 0 && _itempurchaseCnt == 0)
        {
            return;
        }

        _itempurchaseCnt--;

        UpdateUI();
    }
    public void PurchaseBtn()
    {
        int value = _currentHaveMoney - (_itemPurchaseMoney * _itempurchaseCnt);
        if (value < 0)
        {
            return;
        }

        _currentHaveMoney = value;
        Define.GetManager<DataManager>().SetFeahter(_currentHaveMoney);
        ItemInfo info = Define.GetManager<DataManager>().LoadUsableItemFromInventory(_currentSelectItemCode);
        if(info == null)
        {
            info = new ItemInfo();
            info.id = _currentSelectItemCode;
            info.maxCnt = 0;
        }    
        info.currentCnt += _itempurchaseCnt;

        Define.GetManager<DataManager>().AddUsableItemToInventory(info);

        UpdateUI();
    }
    public void ChangeItemCard(ItemPrice data)
    {
        _currentSelectItemCode = data.itemID;
        _itemPurchaseMoney = data.price;

        _itempurchaseCnt = 1;

        UpdateUI();
    }

    public void UpdateUI()
    {
        _cntText.text = string.Format("{0}", _itempurchaseCnt);
        _currenHavetMoneyText.text = string.Format("{0}", _currentHaveMoney);
        _beforeMoneyText.text = string.Format("{0}", Math.Clamp(_currentHaveMoney - (_itemPurchaseMoney * _itempurchaseCnt), 0, int.MaxValue));
    }
    #endregion

    #region Inventory
    public void ShowInventory()
    {
        InventoryInit();
        _onOtherPanel = true;

        CreateCardList<string>(_weaponListScroll, Define.GetManager<DataManager>().LoadWeaponData());
        //CreateCardList<string>(_heloListScroll, Define.GetManager<DataManager>().L());
        CreateCardList<ItemInfo>(_useableListScroll, Define.GetManager<DataManager>().LoadUsableItemFromInventory());
        //CreateCardList<string>(_questItemListScroll, Define.GetManager<DataManager>().LoadWeaponData());

        VisualElement box = _firstWeaponPanel.Q<VisualElement>("1");
        string firstWeaponName = DataManager.UserData.firstWeapon;
        box.style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManagers>().Load<Sprite>(firstWeaponName));
        box.RegisterCallback<ClickEvent>(e => {
            SelectEquipWeaponBoxBtn(int.Parse(box.name), box);
        });

        VisualElement box2 = _secondWeaponPanel.Q<VisualElement>("2");
        string secondWeaponName = DataManager.UserData.secondWeapon;
        box2.style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManagers>().Load<Sprite>(secondWeaponName));
        box2.RegisterCallback<ClickEvent>(e => {
            SelectEquipWeaponBoxBtn(int.Parse(box2.name), box2);
        });

    }

    public void SelectEquipWeaponBoxBtn(int num, VisualElement box)
    {
        if(_selectVisual != null)
            _selectVisual.style.backgroundColor = new StyleColor(Color.white);

        ChangeShowInventoryPanel(0);
        _selectVisual = box;
        _selectNum = num;
        boxType = EquipType.Weapon;

        _selectVisual.style.backgroundColor = new StyleColor(Color.blue);

        
    }

    //public void SelectEquipHeloBoxBtn(int num, VisualElement box)
    //{
    //    ChangeShowInventoryPanel(1);
    //    _selectVisual = box;
    //    _selectNum = num;
    //    boxType = EquipType.Helo;
    //}
    //public void SelectEquipUseableItemBoxBtn(int num, VisualElement box)
    //{
    //    ChangeShowInventoryPanel(2);
    //    _selectVisual = box;
    //    _selectNum = num;
    //    boxType = EquipType.UseableItem;
    //}

    public void CreateCardList<T>(VisualElement parent,List<T> list)
    {
        foreach(T data in list)
        {
            Debug.Log("Create Card");
            VisualElement card = _itemInfoCardTemp.Instantiate().Q<VisualElement>("ItemInfoCardTemp");
            card.style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManagers>().Load<Sprite>(data as string));
            card.RegisterCallback<ClickEvent>(e =>
            {
                Debug.Log("WeaponCardClick");
                if(_selectVisual != null && boxType == EquipType.Weapon)
                {
                    Debug.Log($"{_selectVisual} : Quipt");
                    _selectVisual.style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManagers>().Load<Sprite>(data as string));
                    Define.GetManager<DataManager>().ChangeUserWeaponData(data as string, _selectNum == 1);

                    _selectVisual = null;
                    boxType = EquipType.None;

                    Define.GetManager<EventManager>().TriggerEvent(EventFlag.SetWeapon, new EventParam());
                }
                
            });
            parent.Add(card);
        }    
    }

    public void ChangeShowInventoryPanel(int pageNum)
    {
        _scrolls.style.translate = new StyleTranslate(new Translate(new Length(-25 * pageNum, LengthUnit.Percent), new Length(0, LengthUnit.Pixel)));
    }
    #endregion

    #region GameOption
    public void StartGame()
    { 
        SceneManager.LoadScene("BossBattle");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
    #endregion
}
