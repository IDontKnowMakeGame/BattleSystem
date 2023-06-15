using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIQuit : UIBase
{
    private Button _backBtn;
    private Button _quitBtn;
    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_Quit");

        _backBtn = _root.Q<Button>("Btn-back");
        _backBtn.RegisterCallback<MouseEnterEvent>(e =>
        {
            SetOpacity(_backBtn, 10f);
        });
        _backBtn.RegisterCallback<MouseLeaveEvent>(e =>
        {
            SetOpacity(_backBtn, 0.3f);
        });
        _backBtn.clicked += OnClickBackBtn;
        _quitBtn = _root.Q<Button>("Btn-quit");
        _quitBtn.RegisterCallback<MouseEnterEvent>(e =>
        {
            SetOpacity(_quitBtn, 10f);
        });
        _quitBtn.RegisterCallback<MouseLeaveEvent>(e =>
        {
            SetOpacity(_quitBtn, 0.3f);
        });
        _quitBtn.clicked += OnClickQuitBtn;

        _root.style.display = DisplayStyle.None;
    }
    public override void Show()
    {
        UIManager.OpenPanels.Push(this);
        UIManager.Instance.MoveAndInputStop();
        SetOpacity(_backBtn, 0.3f);
        SetOpacity(_quitBtn, 0.3f);
        _root.style.display = DisplayStyle.Flex;
    }
    public override void Hide()
    {
        _root.style.display = DisplayStyle.None;
    }

    public void OnClickBackBtn()
    {
        UIManager.Instance.Escape();
    }
    public void OnClickQuitBtn()
    {
        Application.Quit();
    }
    public void SetOpacity(VisualElement card, float opacity)
    {
        card.style.opacity = new StyleFloat(opacity);
    }
}
