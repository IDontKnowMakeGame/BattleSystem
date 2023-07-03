using Core;
using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
//using static UnityEditor.Progress;

public class UISmithy : UIBase
{

    private VisualTreeAsset _weaponCardTemp;

    private VisualElement _weaponImage;
    private VisualElement _weaponPanel;
    private VisualElement _leftBtn;
    private VisualElement _rightBtn;
    private VisualElement _purchaseBtn;

    private Label _weaponName;
    private Label _levelLabel;
    private string _leveltext;
    private Label _atkLabel;
    private string _atktext;
    private Label _needFeatherLabel;
    private string _needFeatherText;
    private Label _featherLabel;

    private List<SaveItemData> _weaponList = new List<SaveItemData>();

    private int currentFeather = 0;
    private ItemID currentWeaponID;
    private float moveSclae = 165f;
    private int index = 0;
    private int maxIndex = 0;
    

    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_Smithy");

        _weaponCardTemp = Define.GetManager<ResourceManager>().Load<VisualTreeAsset>("UIDoc/InventoryWeaponItemCard");
        _weaponPanel = _root.Q<VisualElement>("panel-weapon");
        _weaponImage = _root.Q<VisualElement>("weaponImage");
        _weaponName = _weaponImage.Q<Label>("label-weaponName");
        _levelLabel = _root.Q<Label>("label-level");
        _leveltext = _levelLabel.text; 
        _atkLabel = _root.Q<Label>("label-atk");
        _atktext = _atkLabel.text;
        _needFeatherLabel = _root.Q<Label>("label-needfeather");
        _needFeatherText = _needFeatherLabel.text;
        _featherLabel = _root.Q<Label>("lable-feather");
        _purchaseBtn = _root.Q<VisualElement>("btn-purchase");
        _purchaseBtn.RegisterCallback<ClickEvent>(e =>
        {
            Purchase();
        });

        _leftBtn = _root.Q<VisualElement>("Btn-left");
        _leftBtn.RegisterCallback<ClickEvent>(e =>
        {
            if (index <= 0) return;
            CancelCard(_weaponPanel[index]);
            index--;
            _weaponPanel.style.translate = new StyleTranslate(new Translate((-index + 2) * moveSclae,0,0));
            UpdateWeaponCard(index);
        });
        _rightBtn = _root.Q<VisualElement>("Btn-right");
        _rightBtn.RegisterCallback<ClickEvent>(e =>
        {
            if (index >= maxIndex) return;
            CancelCard(_weaponPanel[index]);
            index++;
            _weaponPanel.style.translate = new StyleTranslate(new Translate((-index + 2) * moveSclae, 0, 0));
            UpdateWeaponCard(index);
        });
        _purchaseBtn = _root.Q<VisualElement>("");


        currentFeather = Define.GetManager<DataManager>().GetFeather();
        CreateCard();
    }
    public override void Show()
    {
        UIManager.Instance.MoveAndInputStop();
        UIManager.Instance.PadeInOut.Pade(PadeType.padeUp, () =>
        {
            _root.style.display = DisplayStyle.Flex;
            CreateCard();
            UIManager.OpenPanels.Push(this);
        });
        currentFeather = Define.GetManager<DataManager>().GetFeather();
    }
    public override void Hide()
    {
        UIManager.Instance.MoveAndInputPlay();
        _root.style.display = DisplayStyle.None;
    }

    public void CreateCard()
    {
        _weaponPanel.Clear();
        _weaponList = Define.GetManager<DataManager>().LoadWeaponDataFromInventory();
        int cnt = 0;
        foreach (SaveItemData item in _weaponList)
        {
            if (item.id == ItemID.None) continue;
            VisualElement card = _weaponCardTemp.Instantiate();
            
            card.Q<VisualElement>("card").style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManager>().Load<Sprite>($"Item/{(int)item.id}"));
            _weaponPanel.Add(card);
            cnt++;
        }
        maxIndex = cnt-1;
        UpdateStatus(_weaponList[index].id);
        UpdateWeaponCard(index);
        UpdateCurrentFeather();
    }

    public void UpdateWeaponCard(int index)
    {
        currentWeaponID = _weaponList[index].id;
        _weaponImage.style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManager>().Load<Sprite>($"Item/{(int)currentWeaponID}"));
        _weaponName.text = UIManager.Instance.weaponTextInfoListSO.weapons[(int)currentWeaponID - 1].weaponNameText;
        SelectCard(_weaponPanel[index]);
        UpdateStatus(currentWeaponID);
    }
    public void UpdateStatus(ItemID id)
    {
        int level = Define.GetManager<DataManager>().LoadWeaponLevelData(id);
        if(level >= 3)
        {
            _levelLabel.text = "레벨 : 3(Max)";
            _atkLabel.text = $"공격력 : {UIManager.Instance.levelToAtk[level]}";
            _needFeatherLabel.text = "";
            return;
        }

        _levelLabel.text = _leveltext.Replace("x", level.ToString()).Replace("y", (level+1).ToString());
        _atkLabel.text = _atktext.Replace("x", UIManager.Instance.levelToAtk[level].ToString()).Replace("y", UIManager.Instance.levelToAtk[level+1].ToString());
        _needFeatherLabel.text = _needFeatherText.Replace("x", UIManager.Instance.levelTofeather[level + 1].ToString());
    }
    public void Purchase()
    {
        int level = Define.GetManager<DataManager>().LoadWeaponLevelData(currentWeaponID);
        int value = currentFeather - UIManager.Instance.levelTofeather[level+1];
        if (level >= 3 || value < 0)
        {
            Define.GetManager<SoundManager>().Play("UI/Faield", Define.Sound.Effect);
            return;
        }
        level++;
        Define.GetManager<DataManager>().SaveUpGradeWeaponLevelData(currentWeaponID);
        Define.GetManager<DataManager>().SetFeahter(value);
        Define.GetManager<SoundManager>().Play("UI/UpgradeSound", Define.Sound.Effect);

        currentFeather = value;

        UpdateStatus(currentWeaponID);
        UpdateCurrentFeather();
    }
    public void UpdateCurrentFeather()
    {
        _featherLabel.text = currentFeather.ToString();
    }
    public void SelectCard(VisualElement card)
    {
        //card.style.translate = new StyleTranslate(new Translate(0, -10));
        SetBoderScale(_weaponPanel[index], 5, Color.white);
    }
    public void CancelCard(VisualElement card)
    {
        //card.style.translate = new StyleTranslate(new Translate(0, 0));
        SetBoderScale(_weaponPanel[index], 0, Color.white);
    }
    public void SetBoderScale(VisualElement card,float value,Color color)
    {
        StyleFloat scale = new StyleFloat(value);
        card.style.borderBottomWidth = scale;
        card.style.borderTopWidth = scale;
        card.style.borderLeftWidth = scale;
        card.style.borderRightWidth = scale;

        card.style.borderBottomColor = color;
        card.style.borderTopColor = color;
        card.style.borderLeftColor = color;
        card.style.borderRightColor = color;
    }
}
