using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class UIBossBar : UIBase
{
    private VisualElement _bossIcon;
    private VisualElement _bossBar;
    private Label _bossName;

    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_BossBar");

        _bossIcon = _root.Q<VisualElement>("BossIcon");
        _bossBar = _root.Q<VisualElement>("BossBar");
        _bossName = _root.Q<Label>("BossName");
    }

    public void ShowBossBar(string name)
    {
        _root.style.display = DisplayStyle.Flex;

        ChangeBossBarValue(100);
        _bossName.text = name;
        _bossIcon.style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManager>().Load<Sprite>($"Image/Boss/{name}"));
    }

    public void HideBossBar()
    {
        _root.style.display = DisplayStyle.None;
    }

    public void ChangeBossBarValue(int value)
    {
        VisualElement fill = _bossBar.Q<VisualElement>("Fill");

        fill.style.width = new Length(value, LengthUnit.Percent);
    }
}
