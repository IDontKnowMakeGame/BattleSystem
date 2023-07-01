using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public enum PadeType
{
    padeUp =0,
    padeDown = 1,
}
public class UIPadeInOut : UIBase
{
    private VisualElement _padePanel;

    public bool isPaded = false;
    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_PadeInOut");
        _root.style.display = DisplayStyle.None;

        _padePanel = _root.Q<VisualElement>("PadePanel");
       
    }

    public void Pade(PadeType padeType,Action action = null)
    {
        if(padeType == PadeType.padeUp)
            UIManager.Instance.StartCoroutine(PadeCoroutine("PadePanel-out-under", "PadePanel-out-top", action));
        else if(padeType == PadeType.padeDown)
            UIManager.Instance.StartCoroutine(PadeCoroutine("PadePanel-out-top", "PadePanel-out-under", action));
    }
    private IEnumerator PadeCoroutine(string removeClass,string addClass,Action action = null)
    {
        _root.style.display = DisplayStyle.Flex;
        UIManager.Instance.stopEsc = true;

        _padePanel.RemoveFromClassList(removeClass);
        if(HaloOfTime.currentTime > 1f)
            yield return new WaitForSeconds(2f);
        else
            yield return new WaitForSeconds(0.7f);
        action?.Invoke();
        _padePanel.AddToClassList(addClass);
        yield return new WaitForSeconds(0.7f);
        _root.style.display = DisplayStyle.None;
        UIManager.Instance.stopEsc = false;
    }
}
