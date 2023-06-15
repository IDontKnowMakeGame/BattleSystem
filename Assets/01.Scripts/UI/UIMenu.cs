using Core;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UIMenu : UIBase
{

    private VisualElement _menuPanel;
    private VisualTreeAsset _menuBtnTemp;

    private VisualElement _selectBtn = null;
    private bool _isShow = false;
    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_Menu");

        _menuPanel = _root.Q<VisualElement>("MenuPanel");
        _menuBtnTemp = Define.GetManager<ResourceManager>().Load<VisualTreeAsset>("UIDoc/MenuBtnTemp");

        
        for(int i = 0; i < 4; i++)
        {
            if (i == 2) continue;
            VisualElement panel = _menuBtnTemp.Instantiate();
            int index = i;
            panel.RegisterCallback<ClickEvent>(e =>
            {
                ClickMenuBtn(panel, index);
            });
            panel.RegisterCallback<MouseEnterEvent>(e =>
            {
                SetMenuBtnStyle(panel, 10);
            });
            panel.RegisterCallback<MouseLeaveEvent>(e =>
            {
                SetMenuBtnStyle(panel, 0.5f);
            });

            SettingBtn(panel,i);
            _menuPanel.Add(panel);
        }
        
    }
    public override void Show()
    {   if (_isShow) return;
        _isShow = true;
        UIManager.Instance.StartCoroutine(ShowPanelCoroutine());
    }
    private IEnumerator ShowPanelCoroutine()
    {
        _menuPanel.RemoveFromClassList("MenuPanel-hide");
        yield return new WaitForSeconds(0.5f);

        foreach(VisualElement panel in _menuPanel.Children())
        {
            panel.Q<VisualElement>("MenuBtn").RemoveFromClassList("MenuBtn-hide");
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.3f);
        UIManager.OpenPanels.Push(this);
        
    }
    public override void Hide()
    {
        if (_isShow == false) return;
        
        UIManager.Instance.StartCoroutine(HidePanelCoroutine());
    }
    private IEnumerator HidePanelCoroutine()
    {
        foreach (VisualElement panel in _menuPanel.Children())
        {
            panel.Q<VisualElement>("MenuBtn").AddToClassList("MenuBtn-hide");
            yield return new WaitForSeconds(0.05f);
        }

        _menuPanel.AddToClassList("MenuPanel-hide");
        yield return new WaitForSeconds(0.5f);
       
        _isShow = false;
    }

    private void ClickMenuBtn(VisualElement btn,int num)
    {
        //if(btn == _selectBtn)
        //{
        //    SetMenuBtnStyle(btn, 0.5f);
        //    _selectBtn = null;
        //    return;
        //}

        //if(_selectBtn != null)
        //    SetMenuBtnStyle(_selectBtn, 0.5f);

        //_selectBtn = btn;
        //SetMenuBtnStyle(_selectBtn, 1f);

        switch (num)
        {
            case 0:
                UIManager.Instance.Status.Show();
                break;
            case 1:
                UIManager.Instance.PadeInOut.Pade(0, () => {
                    UIManager.Instance.Inventory.Show();
                });
                break;
            case 2:

                break;
            case 3:
                UIManager.Instance.Quit.Show();
                break;
        }
    }
    private void SettingBtn(VisualElement btn, int num)
    {
        btn.Q<VisualElement>("icon").style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManager>().Load<Sprite>($"MenuIcon/{num}"));

        string name = "";
        switch(num)
        {
            case 0:
                name = "스테이터스";
                break;
            case 1:
                name = "인벤토리";
                break;
            case 2:
                name = "설정";
                break;
            case 3:
                name = "종료";
                break;
        }
        btn.Q<Label>("Label").text = name;
    }

    private void SetMenuBtnStyle(VisualElement card, float opacity)
    {
        card.style.opacity = new StyleFloat(opacity);
    }
}
