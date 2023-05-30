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

    private VisualElement _exitBtn;

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

        _exitBtn = _root.Q<VisualElement>("ExitBtn");
        _exitBtn.RegisterCallback<ClickEvent>(e =>
        {
            HideSmithy();
        });

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
        _purchaseBtn.RegisterCallback<ClickEvent>(e =>
        {
            PurchaseBtn();
        });

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
    public void HideSmithy()
    {
        _root.style.display = DisplayStyle.None;
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
            VisualElement card = _weaponCardTemp.Instantiate().Q<VisualElement>("card");
            card.style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManager>().Load<Sprite>($"Item/{(int)item.id}"));
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
        _weaponSelectBtn.style.backgroundImage = card.style.backgroundImage;

        currentItem = id;
        nowLevel = Define.GetManager<DataManager>().LoadWeaponLevelData(currentItem);
        afterLevel = nowLevel + 1;

        int myAtk = (int)Define.GetManager<DataManager>().GetWeaponData(id).Atk;
        nowAtk = myAtk + UIManager.Instance.LevelToAtk(nowLevel);
        afterAtk = myAtk + UIManager.Instance.LevelToAtk(afterLevel);

        price = UIManager.Instance.LevelToFeather(afterLevel);

        UpdateSmithyUI();
    }
    public void PurchaseBtn()
    {
        if (currentItem == ItemID.None) return;
        if (nowLevel >= 12) return;

        Define.GetManager<DataManager>().AddFeahter(-price);
        UIManager.Instance.InGame.WriteFeatherValue();

        nowLevel++;
        afterLevel++;
        int myAtk = (int)Define.GetManager<DataManager>().GetWeaponData(currentItem).Atk;
        nowAtk = myAtk + UIManager.Instance.LevelToAtk(nowLevel);
        afterAtk = myAtk + UIManager.Instance.LevelToAtk(afterLevel);

        nowFeather = afterFeather;
        price = UIManager.Instance.LevelToFeather(afterLevel);

        Define.GetManager<DataManager>().SaveUpGradeWeaponLevelData(currentItem); 
        

        
        UpdateSmithyUI();
    }
    
    public void UpdateSmithyUI()
    {
        _levelText.text = string.Format("Level {0} -> {1}", nowLevel, afterLevel);
        _atkText.text = string.Format("Atk {0} -> {1}", nowAtk, afterAtk);
        nowFeather = Define.GetManager<DataManager>().GetFeather();

        _smithyNowFeatherText.text = nowFeather.ToString();
        afterFeather = nowFeather - price;
        _smithyAfterFeatherText.text = afterFeather.ToString();
    }
}
