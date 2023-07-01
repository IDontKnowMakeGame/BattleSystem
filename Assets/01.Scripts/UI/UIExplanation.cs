using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIExplanation : UIBase
{
    private VisualElement _background;
    private VisualElement _voidPanel;
    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_Explanation");

        _background = _root.Q<VisualElement>("background");
        _voidPanel = _background.Q<VisualElement>("void-panel");

        _root.style.display = DisplayStyle.None;
    }
    public void Show(int pageNum)
    {
        if(pageNum >=3 && pageNum <= 7)
            VideoManager.Instance.ChangeVideo(pageNum);

        _voidPanel.style.translate = new StyleTranslate(new Translate(new Length(-pageNum * 100,LengthUnit.Percent), 0));
        Show();
    }
    public void Show(ItemID itemID)
    {
        _voidPanel.style.translate = new StyleTranslate(new Translate(0, 0));
        Show();
    }
    public override void Show()
    {
        _root.style.display = DisplayStyle.Flex;
        _background.RemoveFromClassList("background-off");
        UIManager.Instance.MoveAndInputStop();
        Time.timeScale = 0;
        UIManager.OpenPanels.Push(this);
    }

    public override void Hide()
    {
        UIManager.Instance.StartCoroutine(HideCoroutine());
    }
    private IEnumerator HideCoroutine()
    {
        _background.AddToClassList("background-off");
        Time.timeScale = HaloOfTime.currentTime;
        UIManager.Instance.MoveAndInputPlay();
        yield return new WaitForSeconds(0.4f);
        _root.style.display = DisplayStyle.None;

    }
}
