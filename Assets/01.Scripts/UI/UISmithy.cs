using Core;
using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UISmithy : UIBase
{
    private VisualElement _characterImage;
    private VisualElement _weaponSelectBtn;

    private VisualElement _weaponSelectPanel;
    private VisualElement _weaponScrollPanel;

    private VisualTreeAsset _weaponCardTemp;
    private VisualElement _weaponUpgradePanel;
    private Label _levelText;
    private Label _atkText;
    private Label _smithyNowFeather;
    private Label _smithyAfterFeather;
    private VisualElement _purchaseBtn;

    private VisualElement _feaderBox;

    private ItemID currentItem;

    private int nowLevel = 0;
    private int afterLevel = 0;
    private int nowAtk = 0;
    private int afterAtk = 0;
    private int nowFeather = 0;
    private int afterFeather = 0;
    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_Smithy");

        _characterImage = _root.Q<VisualElement>("CharacterImage");
        _weaponSelectBtn = _characterImage.Q<VisualElement>("WeaponSelect");

        _weaponSelectPanel = _root.Q<VisualElement>("WeaponSelectPanel");
        _weaponScrollPanel = _weaponSelectPanel.Q<VisualElement>("WeaponScrollPanel");

        _weaponUpgradePanel = _root.Q<VisualElement>("WeaponUpgradePanel");
        _levelText = _weaponUpgradePanel.Q<Label>("LevelText");
        _atkText = _weaponUpgradePanel.Q<Label>("AtkText");
        _smithyNowFeather = _weaponUpgradePanel.Q<Label>("NowFeatherText");
        _smithyAfterFeather = _weaponUpgradePanel.Q<Label>("BeforeFeatherText");
        _purchaseBtn = _weaponUpgradePanel.Q<VisualElement>("PurchaseBtn");

        _feaderBox = _root.Q<VisualElement>("FeatherBox");

        _weaponCardTemp = Define.GetManager<ResourceManager>().Load<VisualTreeAsset>("UIDoc/InventoryWeaponItemCard");
        nowFeather = Define.GetManager<DataManager>().GetFeather();
    }

    public void ShowSmithy()
    {
        _root.style.display = DisplayStyle.Flex;

    }
    public void CreateWeaponCardList(List<SaveItemData> list)
    {

        foreach(SaveItemData item in list)
        {

        }
    }

}
