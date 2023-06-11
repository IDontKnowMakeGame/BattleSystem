using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIStatus : UIBase
{
    private VisualElement _backGround;
    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_Status");

        _backGround = _root.Q<VisualElement>("BackGround");

        _root.style.display = DisplayStyle.None;
    }

    public override void Show()
    {
        _root.style.display = DisplayStyle.Flex;
        _backGround.RemoveFromClassList("backGround-hide");

        UIManager.OpenPanels.Push(this);
    }
    public override void Hide()
    {
        _backGround.AddToClassList("backGround-hide");
        UIManager.Instance.StartCoroutine(HideCoroutine(1f));
    }
    public IEnumerator HideCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        _root.style.display = DisplayStyle.None;
    }
}
