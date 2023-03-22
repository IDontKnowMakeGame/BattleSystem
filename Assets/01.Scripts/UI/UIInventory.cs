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

    #region HaloPanel

    #endregion

    #region UseableItemPanel
    private VisualElement _useableItemScrollPanel;
    private VisualElement _useableEquipPanel;

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
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_Inventory");

        _exitBtn = _root.Q<VisualElement>("ExitBtn");
        _exitBtn.RegisterCallback<ClickEvent>(e =>
        {
            HideInventory();
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

        _weaponChacraterViewImage = _weaponPanel.Q<VisualElement>("CharacterImage");
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

        _useableItemScrollPanel = _useableItemPanel.Q<VisualElement>("UseableItemScrollPanel");
        _useableEquipPanel = _useableItemPanel.Q<VisualElement>("UseableEquipPanel");
        _useableItemCardTemp = Resources.Load<VisualTreeAsset>("UIDoc/InventoryUseableItemCardTemp");

        _questItemScrollPanel = _questItemPanel.Q<VisualElement>("QuestItemScrollPanel");
        _questItemCardTemp = Resources.Load<VisualTreeAsset>("UIDoc/InventoryQuestItemCardTemp");

        UseableEquipBoxSetting();
        CreateCardList(_weaponScrollPanel, _weaponCardTemp, Define.GetManager<DataManager>().LoadWeaponDataFromInventory(), SelectCard);
        CreateCardList(_useableItemScrollPanel, _useableItemCardTemp, Define.GetManager<DataManager>().LoadUsableItemFromInventory(), SelectCard);
        CreateCardList(_questItemScrollPanel, _questItemCardTemp, Define.GetManager<DataManager>().LoadQuestFromInventory(), SelectCard);
    }
    public void ShowInventory()
    {
        _root.style.display = DisplayStyle.Flex;
        EquipWeaponBoxImage();
    }
    public void HideInventory()
    {
        _root.style.display = DisplayStyle.None;
    }

    public void EquipWeaponBoxImage()
    {
        ItemID id = DataManager.UserData_.firstWeapon;
        _firstWeaponImage.style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManager>().Load<Sprite>($"Image/{id}"));
        id = DataManager.UserData_.secondWeapon;
        _secondWeaponImage.style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManager>().Load<Sprite>($"Image/{id}"));
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
        foreach(SaveItemData item in list)
        {
            VisualElement card = temp.Instantiate().Q<VisualElement>("card");
            card.style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManager>().Load<Sprite>($"Image/{item.name}"));

            card.RegisterCallback<ClickEvent>(e =>
            {
                action(card,item.id);
            });
            parent.Add(card);
        }
    }
    public void UseableEquipBoxSetting()
    {
        int index = 1;
        foreach(VisualElement card in _useableEquipPanel.Children())
        {
            card.RegisterCallback<ClickEvent>(e =>
            {
                EquipItemBox(card,index);
            });
            index++;
        }
    }
    public void EquipItem(ItemID id,int equipNum)
    {
        if((int)id < 100)
            Define.GetManager<DataManager>().ChangeUserWeaponData(id, equipNum);
        else
            Define.GetManager<DataManager>().EquipUsableItem(id, equipNum);

        EquipWeaponBoxImage();
        Define.GetManager<EventManager>().TriggerEvent(EventFlag.WeaponEquip, new EventParam());
    }
    public void EquipItemBox(VisualElement card,int equipNum)
    {
        if(isSelectCard)
        {
            if (selectCard != null)
                CardBorderWidth(selectCard, 0, Color.white);
            EquipItem(currentItemId, equipNum);

            SelectOptionInit();
        }
        else
        {
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
            if (selectCard != null)
                CardBorderWidth(selectCard, 0, Color.white);
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
    }
    public void CardBorderWidth(VisualElement card,float value,Color color)
    {
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
