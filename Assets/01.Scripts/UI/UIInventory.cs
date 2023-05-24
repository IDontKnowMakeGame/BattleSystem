using Core;
using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.UIElements;

public class UIInventory : UIBase
{
    private bool isOpen;

    private VisualElement _selectBtnBox;
    private VisualElement _exitBtn;

    private VisualElement _weaponSelectBtn;
    private VisualElement _haloSelectBtn;
    private VisualElement _useableItemSelectBtn;
    private VisualElement _questItemSelectBtn;

    private VisualElement _itemPanel;
    private VisualElement _weaponPanel;
    private VisualElement _haloPanel;
    private VisualElement _useableItemPanel;
    private VisualElement _questItemPanel;

    #region WaeponPanel
    private VisualElement _weaponChacraterViewImage;
    private VisualElement _firstWeaponImage;
    private VisualElement _secondWeaponImage;
    private VisualElement _weaponScrollPanel;

    private VisualTreeAsset _weaponCardTemp;
    #endregion

    #region WeaponStatusPanel
    private VisualElement _weaponInfoPanel;
    private VisualElement _weaponStatusPanel;
    #endregion

    #region HaloPanel
    private VisualElement _haloSelectPanel;
    private VisualElement _equipHaloPanel;

    private VisualElement _selectHaloCard = null;
    private ItemID _selectHaloID = ItemID.None;
    private int _selectHaloEquipNum = 0;
    private bool _selectIsHaloCard = false;
    private bool _selectIsEquipHaloCard = false;
    float width = 100;
    float height = 100;
    #endregion

    #region UseableItemPanel
    private VisualElement _useableItemScrollPanel;
    private VisualElement _useableEquipPanel;
    private VisualElement _unmountBtn;

    private VisualTreeAsset _useableItemCardTemp;
    #endregion

    #region QuestItemPanel
    private VisualElement _questItemScrollPanel;

    private VisualTreeAsset _questItemCardTemp;
    #endregion

    
    private bool isSelectEquipBox = false;
    private bool isSelectCard = false;
    private ItemID currentItemId = ItemID.None;
    private int selectNumber = 0;
    private VisualElement selectCard = null;


    public override void Init()
    {
        isOpen = false;
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_Inventory");

        _exitBtn = _root.Q<VisualElement>("ExitBtn");
        _exitBtn.RegisterCallback<ClickEvent>(e =>
        {
            HideInventory();
            UIManager.Instance.UpdateInGameUI();
        });

        VisualElement selectType = _root.Q<VisualElement>("SelectItemType");
        _weaponSelectBtn = selectType.Q<VisualElement>("Weapon");
        _weaponSelectBtn.RegisterCallback<ClickEvent>(e =>
        {
            SelectItemBtn(0, _weaponSelectBtn);
        });
        _haloSelectBtn = selectType.Q<VisualElement>("Halo");
        _haloSelectBtn.RegisterCallback<ClickEvent>(e =>
        {
            SelectItemBtn(1, _haloSelectBtn);
        });
        _useableItemSelectBtn = selectType.Q<VisualElement>("UseableItem");
        _useableItemSelectBtn.RegisterCallback<ClickEvent>(e =>
        {
            SelectItemBtn(2, _useableItemSelectBtn);
        });
        _questItemSelectBtn = selectType.Q<VisualElement>("QuestItem");
        _questItemSelectBtn.RegisterCallback<ClickEvent>(e =>
        {
            SelectItemBtn(3, _questItemSelectBtn);
        });

        _selectBtnBox = _weaponSelectBtn;

        _itemPanel = _root.Q<VisualElement>("ItemPanel");
        _weaponPanel = _itemPanel.Q<VisualElement>("WeaponPanel");
        _haloPanel = _itemPanel.Q<VisualElement>("HaloPanel");
        _useableItemPanel = _itemPanel.Q<VisualElement>("UseblePanel");
        _questItemPanel = _itemPanel.Q<VisualElement>("QuestPanel");

        //WeaponPanel===================================================================================
        _weaponChacraterViewImage = _weaponPanel.Q<VisualElement>("Character");
        _weaponInfoPanel = _weaponPanel.Q<VisualElement>("WeaponInfoPanel");
        _weaponStatusPanel = _weaponInfoPanel.Q<VisualElement>("StatusPanel");
        _firstWeaponImage = _weaponPanel.Q<VisualElement>("FirstWeapon");
        _firstWeaponImage.RegisterCallback<ClickEvent>(e =>
        {
            EquipItemBox(_firstWeaponImage, 1);
        });
        _secondWeaponImage = _weaponPanel.Q<VisualElement>("SecondWeapon");
        _secondWeaponImage.RegisterCallback<ClickEvent>(e =>
        {
            EquipItemBox(_secondWeaponImage, 2);
        });
        _weaponScrollPanel = _weaponPanel.Q<VisualElement>("WeaponScrollPanel");
        _weaponCardTemp = Resources.Load<VisualTreeAsset>("UIDoc/InventoryWeaponItemCard");
        //HaloPanel===================================================================================
        _haloSelectPanel = _haloPanel.Q<VisualElement>("HaloSelectPanel");
        _equipHaloPanel = _haloPanel.Q<VisualElement>("EquipHalo");
        //UseablePanel===================================================================================
        _useableItemScrollPanel = _useableItemPanel.Q<VisualElement>("UseableItemScrollPanel");
        _useableEquipPanel = _useableItemPanel.Q<VisualElement>("UseableEquipPanel");
        _unmountBtn = _useableEquipPanel.Q<VisualElement>("UnmountBtn");
        _unmountBtn.RegisterCallback<ClickEvent>(e =>
        {
            if (selectCard == null) return;

            UnmountItem(selectNumber);
            UIManager.Instance.InGame.ChangeItemPanelImage();
        });
        _useableEquipPanel = _useableItemPanel.Q<VisualElement>("UseableEquipPanel").Q<VisualElement>("ItemBoxs");
        _useableItemCardTemp = Resources.Load<VisualTreeAsset>("UIDoc/InventoryUseableItemCardTemp");

        _questItemScrollPanel = _questItemPanel.Q<VisualElement>("QuestItemScrollPanel");
        _questItemCardTemp = Resources.Load<VisualTreeAsset>("UIDoc/InventoryQuestItemCardTemp");

        UseableEquipBoxSetting();
        CreateCardList(_weaponScrollPanel, _weaponCardTemp, Define.GetManager<DataManager>().LoadWeaponDataFromInventory(), SelectCard);
        CreateCardList(_useableItemScrollPanel, _useableItemCardTemp, Define.GetManager<DataManager>().LoadUsableItemFromInventory(), SelectCard);
        CreateCardList(_questItemScrollPanel, _questItemCardTemp, Define.GetManager<DataManager>().LoadQuestFromInventory(), SelectCard);
        InitHaloSelectCard();
    }
    public void ShowInventory()
    {
        if(isOpen)
        {
            InGame.Player.GetAct<PlayerEquipment>().WeaponOnOff(false);
            HideInventory();
            return;
        }

        InGame.Player.GetAct<PlayerEquipment>().WeaponOnOff(true);
        isOpen = true;
        _root.style.display = DisplayStyle.Flex;
        EquipWeaponBoxImage();
    }
    public void HideInventory()
    {
        isOpen = false;
        _root.style.display = DisplayStyle.None;
        HideWeaponInfoPanel();
        InitSelectHaloSetting();
        SelectOptionInit(true);
    }

    public void EquipWeaponBoxImage()
    {
        ItemID id = DataManager.UserData_.firstWeapon;
        ItemInfo data = Define.GetManager<DataManager>().weaponDictionary[id];
        _firstWeaponImage.style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManager>().Load<Sprite>($"Item/{(int)id}"));
        _weaponChacraterViewImage.style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManager>().Load<Sprite>($"Image/{data.Class}"));

        id = DataManager.UserData_.secondWeapon;
        _secondWeaponImage.style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManager>().Load<Sprite>($"Item/{(int)id}"));
        
    }
    public void EquipUseableItemBoxImage(VisualElement card, int i)
    {
        List<ItemID> list = Define.GetManager<DataManager>().LoadUsableItemList();
        card.style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManager>().Load<Sprite>($"Item/{(int)list[i-1]}"));

    }
    public void UpdateWeaponIcon()
    { 

    }
    public void SelectItemBtn(int pageNum,VisualElement chageBox)
    {
        _selectBtnBox.style.height = new Length(80, LengthUnit.Percent);
        _selectBtnBox = chageBox;
        _selectBtnBox.style.height = new Length(100, LengthUnit.Percent);

        ChangeShowInventoryPanel(pageNum);
        SelectOptionInit(true);
        
    }
    public void ChangeShowInventoryPanel(int pageNum)
    {
        _itemPanel.style.translate = new StyleTranslate(new Translate(new Length(-100 * pageNum, LengthUnit.Percent), new Length(0, LengthUnit.Pixel)));
    }
    public void CreateCardList(VisualElement parent, VisualTreeAsset temp ,List<SaveItemData> list,Action<VisualElement,ItemID> action)
    {
        parent.Clear();
        foreach (SaveItemData item in list)
        {
            if (EquipToItem(item.id)) continue;
            if (item.id == 0) continue;

            VisualElement card = temp.Instantiate().Q<VisualElement>("card");
            card.style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManager>().Load<Sprite>($"Item/{(int)item.id}"));

            card.RegisterCallback<ClickEvent>(e =>
            {
                action(card,item.id);
            });
            parent.Add(card);
        }
    }
    public bool EquipToItem(ItemID id)
    {
        EquipUesableItemSetting setting = DataManager.UserData_.equipUseableItem;
        if (DataManager.UserData_.firstWeapon == id || DataManager.UserData_.secondWeapon == id) return true;
        if (setting.first == id) return true;
        if (setting.second == id) return true;
        if (setting.third == id) return true;
        if (setting.fourth == id) return true;
        if (setting.fifth == id) return true;

        return false;
    }
    public void ShowWeaponInfoPanel(ItemID itemID)
    {
        ItemInfo data = Define.GetManager<DataManager>().weaponDictionary[itemID];
        int weaponLevel = Define.GetManager<DataManager>().LoadWeaponLevelData(itemID);

        _weaponInfoPanel.style.display = DisplayStyle.Flex;
        _weaponStatusPanel.Q<VisualElement>("Image").style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManager>().Load<Sprite>($"Item/{(int)itemID}"));

        VisualElement status = _weaponInfoPanel.Q<VisualElement>("Status");
        status.Q<Label>("Atk").text = string.Format("Level : {0}", weaponLevel);
        status.Q<Label>("Atk").text = string.Format("공격력 : {0} + {1}", data.Atk, weaponLevel);
        status.Q<Label>("Ats").text = string.Format("공격속도 : {0}", data.Ats);
        status.Q<Label>("Afs").text = string.Format("후 딜레이 : {0}", data.Afs);
        status.Q<Label>("Wei").text = string.Format("무게 : {0}", data.Weight);
    }
    public void HideWeaponInfoPanel()
    {
        _weaponInfoPanel.style.display = DisplayStyle.None;
    }
    public void UseableEquipBoxSetting()
    {
        foreach(VisualElement card in _useableEquipPanel.Children())
        {
            EquipUseableItemBoxImage(card, Int32.Parse(card.name));
            card.RegisterCallback<ClickEvent>(e =>
            {
                int i = Int32.Parse(card.name);
                EquipItemBox(card, i);
                EquipUseableItemBoxImage(card,i);
            });
        }
    }
    public void EquipItem(ItemID id,int equipNum)
    {
        if((int)id < 100)
        {
            Define.GetManager<DataManager>().ChangeUserWeaponData(id, equipNum);
            CreateCardList(_weaponScrollPanel, _weaponCardTemp, Define.GetManager<DataManager>().LoadWeaponDataFromInventory(), SelectCard);
            EquipWeaponBoxImage();
            Define.GetManager<EventManager>().TriggerEvent(EventFlag.WeaponEquip, new EventParam());
        }
        else
        {
            Define.GetManager<DataManager>().EquipUsableItem(id, equipNum);
            EquipUseableItemBoxImage(selectCard, equipNum);
            CreateCardList(_useableItemScrollPanel, _useableItemCardTemp, Define.GetManager<DataManager>().LoadUsableItemFromInventory(), SelectCard);
            UIManager.Instance.InGame.ChangeItemPanelImage();
        } 
    }
    public void UnmountItem(int equipNum)
    {
        Define.GetManager<DataManager>().UnmountUseableItem(equipNum);
        EquipUseableItemBoxImage(selectCard, int.Parse(selectCard.name));
        CreateCardList(_useableItemScrollPanel, _useableItemCardTemp, Define.GetManager<DataManager>().LoadUsableItemFromInventory(), SelectCard);
    }
    public void EquipItemBox(VisualElement card,int equipNum)
    {
        Debug.Log($"EquipItemBox {card.name} + {equipNum}");
        if(isSelectCard)
        {
            if (selectCard != null)
                CardBorderWidth(selectCard, 0, Color.white);
            EquipItem(currentItemId, equipNum);

            SelectOptionInit();
        }
        else
        {
            if (selectCard == card)
            {
                SelectOptionInit(true);
                return;
            }
            if (selectCard != null)
                CardBorderWidth(selectCard, 0, Color.white);
            selectCard = card;
            CardBorderWidth(selectCard, 3, Color.red);
            selectNumber = equipNum;
            isSelectEquipBox = true;
        }
    }
    public void SelectCard(VisualElement card,ItemID id)
    {
        if(isSelectEquipBox)
        {
            if(selectCard != null)
                CardBorderWidth(selectCard, 0, Color.white);
            EquipItem(id, selectNumber);

            SelectOptionInit();
        }
        else
        {
            if(selectCard == card)
            {
                SelectOptionInit(true);
                return;
            }
                
            if (selectCard != null)
                CardBorderWidth(selectCard, 0, Color.white);

            if((int)id < 101&&(int)id !=0 )
                ShowWeaponInfoPanel(id);

            selectCard = card;
            CardBorderWidth(selectCard, 3, Color.red);
            currentItemId = id;
            isSelectCard = true;
        }
 
    }
    public void SelectOptionInit(bool isCardInit = false)
    {
        if(isCardInit&& selectCard != null)
            CardBorderWidth(selectCard, 0, Color.white);

        selectCard = null;
        selectNumber = 0;
        currentItemId = ItemID.None;
        isSelectCard = false;
        isSelectEquipBox = false;

        HideWeaponInfoPanel();
    }
    public void InitHaloSelectCard()
    {
        foreach(VisualElement card in _haloSelectPanel.Children())
        {
            card.RegisterCallback<ClickEvent>(e =>
            {
                HaloSelectCard(card, Int32.Parse(card.name));
            });
        }

        List<ItemID> list = Define.GetManager<DataManager>().LoadHaloListInUserData();
        int index = 0;
        foreach(VisualElement card in _equipHaloPanel.Children())
        {
            ChangeHaloImage(card,list[index++]);
            card.RegisterCallback<ClickEvent>(e =>
            {
                HaloEquipSelectCard(card,Int32.Parse(card.name));
            });
        }
        

    }
    public void HaloSelectCard(VisualElement card, int id)
    {
        if (card == null) return;
        if (_selectIsHaloCard)
        {
            InitSelectHaloSetting();
            return;
        }

        if (_selectIsEquipHaloCard)
        {
            EquipHalo(_selectHaloCard, (ItemID)id, _selectHaloEquipNum);
        }
        else
        {
            _selectIsHaloCard = true;
            _selectHaloCard = card;
            _selectHaloID = (ItemID)id;

            CardBorderWidth(card, 1, Color.green);
        }
        
    }
    public void HaloEquipSelectCard(VisualElement card,int equipNum)
    {
        if (card == null) return;
        if(_selectIsEquipHaloCard)
        {
            EquipHalo(card, ItemID.None, equipNum);
            return;
        }

        if(_selectIsHaloCard)
        {
            EquipHalo(card,_selectHaloID, equipNum);
        }
        else
        {
            _selectIsEquipHaloCard = true;
            _selectHaloCard = card;
            _selectHaloEquipNum = equipNum;

            CardBorderWidth(card, 1, Color.green);
        }
    }
    public void EquipHalo(VisualElement card,ItemID id, int equipNum)
    {
        List<ItemID> list = Define.GetManager<DataManager>().LoadHaloListInUserData();
        if (list.Contains(id)&&id != ItemID.None)
            return;

        Define.GetManager<DataManager>().EquipHalo(id, equipNum);

        EventParam eventParam = new EventParam();
        if(id != ItemID.None)
        {
            eventParam.floatParam = (float)id;
            eventParam.intParam = equipNum;
            Define.GetManager<EventManager>().TriggerEvent(EventFlag.HaloAdd, eventParam);
        }
        else
        {
            eventParam.intParam = equipNum;
            Define.GetManager<EventManager>().TriggerEvent(EventFlag.HaloDel, eventParam);
        }
            
        ChangeHaloImage(card, id);
        InitSelectHaloSetting();
    }
    public void InitSelectHaloSetting()
    {
        CardBorderWidth(_selectHaloCard,0,Color.green);

        _selectHaloCard = null;
        _selectHaloID = ItemID.None;
        _selectHaloEquipNum = 0;

        _selectIsHaloCard = false;
        _selectIsEquipHaloCard = false;
    }
    public void ChangeHaloImage(VisualElement card,ItemID id)
    {
        card.style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManager>().Load<Sprite>($"Item/{(int)id}"));
    }
    public void CardBorderWidth(VisualElement card,float value,Color color)
    {
        if (card == null) return;

        card.style.borderLeftWidth = new StyleFloat(value);
        card.style.borderRightWidth = new StyleFloat(value);
        card.style.borderTopWidth = new StyleFloat(value);
        card.style.borderBottomWidth = new StyleFloat(value);

        card.style.borderLeftColor = new StyleColor(color);
        card.style.borderRightColor = new StyleColor(color);
        card.style.borderTopColor = new StyleColor(color);
        card.style.borderBottomColor = new StyleColor(color);
    }
}
