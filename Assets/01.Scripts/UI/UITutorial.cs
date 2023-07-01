using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UITutorial : UIBase
{
    private VisualElement _backGround;
    private VisualElement _nextBtn;

    private int currentPage;
    private int currentPageIndex = 0;
    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_TutorialPanel");

        _backGround = _root.Q<VisualElement>("background");
        _nextBtn = _root.Q<VisualElement>("btn-next");
        _nextBtn.RegisterCallback<ClickEvent>(e =>
        {
            NextBtn();
            
        });

        Hide();
    }
    public void Show(int page)
    {

        currentPage = page;
        currentPageIndex = 0;
        PagePosition(currentPageIndex, currentPage);
        Show();
    }
    public override void Show()
    {
        UIManager.Instance.stopEsc = true;
        _root.style.display = DisplayStyle.Flex;
    }

    public override void Hide()
    {
        UIManager.Instance.stopEsc = false;
        _root.style.display = DisplayStyle.None;
    }

    public void NextBtn()
    {
        if (_backGround[currentPage].childCount <= currentPageIndex+1)
        {
            Hide();

        }

        currentPageIndex++;
        PagePosition(currentPageIndex, currentPage);
    }

    public void PagePosition(int x,int y)
    {
        Length xPos = new Length(-x * 100, LengthUnit.Percent);
        Length yPos = new Length(-y * 100, LengthUnit.Percent);
        _backGround.style.translate = new StyleTranslate(new Translate(xPos, yPos));
    }
}
