using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIDeathPanel : UIBase
{
    private VisualElement _deathPanel;
    public override void Init()
    {
          _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_DeathPanel");

        _deathPanel = _root.Q<VisualElement>("backGround");
    }

    public override void Show()
    {
        _root.style.display = DisplayStyle.Flex;

        _deathPanel.RemoveFromClassList("DeathPanel-alphaZero");

		Define.GetManager<SoundManager>().Play("UI/UIDie", Define.Sound.Bgm);
	}

    public override void Hide()
    {
        _root.style.display = DisplayStyle.None;

        _deathPanel.AddToClassList("DeathPanel-alphaZero");
	}
}
