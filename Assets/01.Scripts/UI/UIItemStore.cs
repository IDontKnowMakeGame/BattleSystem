using Core;
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
        _minusCntBtn = infoPanel.Q<VisualElement>("MinusBtn");
        _purchaseBtn = infoPanel.Q<VisualElement>("PurchaseBtn");

        _itemCardTemp = Define.GetManager<ResourceManager>().Load<VisualTreeAsset>("ItemCardTemp");
    }

    public void ShowItemStore(ItemStoreTableSO table)
    {
        _itemScrollPanel.Clear();
        foreach(ItemPrice item in table.table)
        {
            VisualElement card = _itemCardTemp.Instantiate();
            card.RegisterCallback<ClickEvent>(e =>
            {

            });

            _itemScrollPanel.Add(card);
        }
    }
}
