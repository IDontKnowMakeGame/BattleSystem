using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.UIElements;

public class UIInventory : UIBase
{
    private VisualElement _selectBtnBox;

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


    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_Inventory");

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
        _secondWeaponImage = _weaponPanel.Q<VisualElement>("SecondWeapon");
        _weaponScrollPanel = _weaponPanel.Q<VisualElement>("WeaponScrollPanel");
        _weaponCardTemp = Resources.Load<VisualTreeAsset>("UIDoc/InventoryWeaponItemCard");

        _useableItemScrollPanel = _useableItemPanel.Q<VisualElement>("UseableItemScrollPanel");
        _useableEquipPanel = _useableItemPanel.Q<VisualElement>("UseableEquipPanel");
        _useableItemCardTemp = Resources.Load<VisualTreeAsset>("UIDoc/InventoryUseableItemCardTemp");

        _questItemScrollPanel = _questItemPanel.Q<VisualElement>("QuestItemScrollPanel");
        _questItemCardTemp = Resources.Load<VisualTreeAsset>("UIDoc/InventoryQuestItemCardTemp");

        CreateCardList(_weaponScrollPanel, _weaponCardTemp, Define.GetManager<DataManager>().LoadWeaponDataFromInventory(), () => { });
        CreateCardList(_useableItemScrollPanel, _useableItemCardTemp, Define.GetManager<DataManager>().LoadUsableItemFromInventory(), () => { });
        CreateCardList(_questItemScrollPanel, _questItemCardTemp, Define.GetManager<DataManager>().LoadQuestFromInventory(), () => { });
    }
    public void ShowInventory()
    {
        _root.style.display = DisplayStyle.Flex;
    }
    public void HideInventory()
    {
        _root.style.display = DisplayStyle.None;
    }

    public void SelectItemBtn(int pageNum,VisualElement chageBox)
    {
        _selectBtnBox.style.height = new Length(80, LengthUnit.Percent);
        _selectBtnBox = chageBox;

        ChangeShowInventoryPanel(pageNum);
        _selectBtnBox.style.height = new Length(100, LengthUnit.Percent);
    }

    public void ChangeShowInventoryPanel(int pageNum)
    {
        _itemPanel.style.translate = new StyleTranslate(new Translate(new Length(-100 * pageNum, LengthUnit.Percent), new Length(0, LengthUnit.Pixel)));
    }
    public void CreateCardList(VisualElement parent, VisualTreeAsset temp ,List<SaveItemData> list,Action action)
    {

        foreach(SaveItemData item in list)
        {
            VisualElement card = temp.Instantiate();
            card.style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManager>().Load<Sprite>(item.name));
            card.RegisterCallback<ClickEvent>(e =>
            {
                action();
            });
            

            parent.Add(card);
        }
    }
}
