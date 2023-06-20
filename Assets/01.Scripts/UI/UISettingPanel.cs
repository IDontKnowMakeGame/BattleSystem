using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UISettingPanel : UIBase
{
    private VisualElement _btnPanel;
    private VisualElement _settingPanel;

    private DropdownField _graphicDropdown;

    private VisualElement _selectBtn;
    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_SettingPanel");

        _btnPanel = _root.Q<VisualElement>("panel-btn");
        int index = 0;
        foreach(VisualElement card in _btnPanel.Children())
        {
            int num = index;
            card.RegisterCallback<ClickEvent>(e =>
            {
                ChangeSelectBtn(card);
                ShowSettingPanel(num);
            });
            card.RegisterCallback<MouseEnterEvent>(e =>
            {
                card.AddToClassList("btn-drag");
            });
            card.RegisterCallback<MouseLeaveEvent>(e =>
            {
                card.RemoveFromClassList("btn-drag");
            });
            index++;
        }
        _selectBtn = _btnPanel[0];
        _settingPanel = _root.Q<VisualElement>("panel-setting");

        _graphicDropdown = _root.Q<DropdownField>("dropdown-graphic");



        _root.style.display = DisplayStyle.None;
    }

    public override void Show()
    {
        _root.style.display = DisplayStyle.Flex;
        UIManager.Instance.MoveAndInputStop();
        UIManager.OpenPanels.Push(this);
    }

    public override void Hide()
    {
        _root.style.display = DisplayStyle.None;
    }
    public void ChangeSelectBtn(VisualElement card)
    {
        if (_selectBtn != null)
            _selectBtn.RemoveFromClassList("btn-select");

        _selectBtn = card;
        _selectBtn.AddToClassList("btn-select");

    }
    public void ShowSettingPanel(int num)
    {

    }
}
