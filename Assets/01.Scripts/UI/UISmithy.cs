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
    private Label _smithyNowFeatherText;
    private Label _smithyAfterFeatherText;
    private VisualElement _purchaseBtn;

    private VisualElement _feaderBox;

    private ItemID currentItem;
    private int price = 0;

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
        _weaponSelectBtn.RegisterCallback<ClickEvent>(e =>
        {
            OpenWeaponSelectPanel();
        });

        _weaponSelectPanel = _root.Q<VisualElement>("WeaponSelectPanel");
        _weaponScrollPanel = _weaponSelectPanel.Q<VisualElement>("WeaponScrollPanel");

        _weaponUpgradePanel = _root.Q<VisualElement>("WeaponUpgradePanel");
        _levelText = _weaponUpgradePanel.Q<Label>("LevelText");
        _atkText = _weaponUpgradePanel.Q<Label>("AtkText");
        _smithyNowFeatherText = _weaponUpgradePanel.Q<Label>("NowFeatherText");
        _smithyAfterFeatherText = _weaponUpgradePanel.Q<Label>("BeforeFeatherText");
        _purchaseBtn = _weaponUpgradePanel.Q<VisualElement>("PurchaseBtn");

        _feaderBox = _root.Q<VisualElement>("FeatherBox");

        _weaponCardTemp = Define.GetManager<ResourceManager>().Load<VisualTreeAsset>("UIDoc/InventoryWeaponItemCard");
        nowFeather = Define.GetManager<DataManager>().GetFeather();
    }

    public void ShowSmithy()
    {
        _root.style.display = DisplayStyle.Flex;
        _weaponSelectPanel.style.display = DisplayStyle.Flex;
        CreateWeaponCardList();
        UpdateSmithyUI();
    }
    public void CreateWeaponCardList()
    {
        CreateWeaponCardList(Define.GetManager<DataManager>().LoadWeaponDataFromInventory());
    }
    public void CreateWeaponCardList(List<SaveItemData> list)
    {
        _weaponScrollPanel.Clear();
        foreach (SaveItemData item in list)
        {
            VisualElement card = _weaponCardTemp.Instantiate();
            card.RegisterCallback<ClickEvent>(e =>
            {
                SelectWeaponCardOnClick(card,item.id);
            });

            _weaponScrollPanel.Add(card);
        }
    }
    public void OpenWeaponSelectPanel()
    {
        _weaponSelectPanel.style.display = DisplayStyle.Flex;
    }
    public void SelectWeaponCardOnClick(VisualElement card,ItemID id)
    {
        _weaponSelectPanel.style.display = DisplayStyle.None;

        currentItem = id;
        nowLevel = Define.GetManager<DataManager>().LoadWeaponLevelData(currentItem);
        afterLevel = nowLevel + 1;

        int myAtk = (int)Define.GetManager<DataManager>().GetWeaponData(id).Atk;
        nowAtk = myAtk + LevelToAtk(nowLevel);
        afterLevel = myAtk + LevelToAtk(afterLevel);

        UpdateSmithyUI();
    }

    public int LevelToAtk(int level)
    {
        int value = 0;
        switch (level)
        {
            case 0:
                value = 0;
                break;
                case 1: value = 20; 
                break;
                case 2: value = 45;  
                break;
                case 3: value = 75;
                break;
                case 4: value = 110; 
                break;
                case 5: value = 150;
                break;
                case 6: value = 195;
                break;
                case 7: value = 245;
                break;
                case 8: value = 300;
                break;
                case 9: value = 360;
                break;
                case 10: value = 425;
                break;
                case 11: value = 495;
                break;
                case 12: value = 570;
                break;
            default:
                break;
        }

        return value;
    }
    public void UpdateSmithyUI()
    {
        _levelText.text = string.Format("Level {0} -> {1}", nowLevel, afterLevel);
        _atkText.text = string.Format("Atk {0} -> {1}", nowAtk, afterAtk);

        _smithyNowFeatherText.text = nowFeather.ToString();
        afterLevel = nowFeather - price;
        _smithyAfterFeatherText.text = afterLevel.ToString();
    }
}
