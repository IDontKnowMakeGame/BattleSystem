using Core;
using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIItemStore : UIBase
{
    private VisualElement _characterImage;
    private VisualElement _itemScrollPanel;
    private Label _currentfeatherText;
    private Label _beforefeatherText;
    private Label _purchaseCntText;
    private VisualElement _addCntBtn;
    private VisualElement _minusCntBtn;
    private VisualElement _purchaseBtn;

    private VisualTreeAsset _itemCardTemp;

    private int _currentFeather;
    private int _beforefeather;

    private VisualElement _currentItem;
    private ItemID _currentItemID;
    private int _currentItemPrice = 0;

    private int _currentPurchaseCnt = 0;
    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_ItemStore");

        _characterImage = _root.Q<VisualElement>("area_characterImage");
        _itemScrollPanel = _root.Q<VisualElement>("ItemScrollPanel");

        VisualElement infoPanel = _root.Q<VisualElement>("InfoPanel");
        _currentfeatherText = infoPanel.Q<Label>("CurrentMoneyText");
        _beforefeatherText = infoPanel.Q<Label>("BeforeMoneyText");
        _purchaseCntText = infoPanel.Q<Label>("CntText");
        _addCntBtn = infoPanel.Q<VisualElement>("AddBtn");
        _addCntBtn.RegisterCallback<ClickEvent>(e =>
        {
            AddBtn();
        });
        _minusCntBtn = infoPanel.Q<VisualElement>("MinusBtn");
        _minusCntBtn.RegisterCallback<ClickEvent>(e =>
        {
            MinusBtn();
        });
        _purchaseBtn = infoPanel.Q<VisualElement>("PurchaseBtn");
        _purchaseBtn.RegisterCallback<ClickEvent>(e =>
        {
            PurchaseBtn();
        });

        _itemCardTemp = Define.GetManager<ResourceManager>().Load<VisualTreeAsset>("UIDoc/ItemCardTemp");
        _currentFeather = Define.GetManager<DataManager>().GetFeather();
        UpdateStoreUI();
    }

    public void ShowItemStore(ItemStoreTableSO table)
    {
        _itemScrollPanel.Clear();
        _root.style.display = DisplayStyle.Flex;
        foreach (ItemPrice item in table.table)
        {
            VisualElement card = _itemCardTemp.Instantiate();
            card.RegisterCallback<ClickEvent>(e =>
            {
                SelectItme(card, item.itemID, item.price);
            });

            _itemScrollPanel.Add(card);
        }
    }

    public void SelectItme(VisualElement item,ItemID itemID,int itemPrice)
    {
        if(_currentItem != null)
            BorderWdith(_currentItem, 0f);

        _currentItem = item;
        BorderWdith(_currentItem, 2f);

        _currentItemID = itemID;
        _currentItemPrice = itemPrice;
        _currentPurchaseCnt = 1;


        UpdateStoreUI();
    }

    public void BorderWdith(VisualElement visualElement,float width)
    {
       VisualElement card = visualElement.Q<VisualElement>("card");
        card.style.borderLeftWidth = new StyleFloat(width);
        card.style.borderRightWidth = new StyleFloat(width);
        card.style.borderTopWidth = new StyleFloat(width);
        card.style.borderBottomWidth = new StyleFloat(width);

        card.style.borderRightColor = new StyleColor(Color.red);
        card.style.borderLeftColor = new StyleColor(Color.red);
        card.style.borderTopColor = new StyleColor(Color.red);
        card.style.borderBottomColor = new StyleColor(Color.red);
    }

    public void AddBtn()
    {
        _currentPurchaseCnt++;
        UpdateStoreUI();
    }

    public void MinusBtn()
    {
        if (_currentPurchaseCnt <= 0) return;
        _currentPurchaseCnt--;
        UpdateStoreUI();
    }

    public void PurchaseBtn()
    {
        int value = _currentFeather - (_currentItemPrice * _currentPurchaseCnt);
        if (value < 0) return;



        _currentFeather = value;
        Define.GetManager<DataManager>().SetFeahter(_currentFeather);
        Define.GetManager<DataManager>().AddItemInInventory(_currentItemID,_currentPurchaseCnt);
        UpdateStoreUI();
    }
    public void UpdateStoreUI()
    {
        _currentfeatherText.text = _currentFeather.ToString();
        _purchaseCntText.text = _currentPurchaseCnt.ToString();

        _beforefeather = _currentFeather - (_currentItemPrice * _currentPurchaseCnt);
        _beforefeatherText.text = _beforefeather.ToString();
    }

}
