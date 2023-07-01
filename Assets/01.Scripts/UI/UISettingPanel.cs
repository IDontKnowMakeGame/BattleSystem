using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UISettingPanel : UIBase
{
    private VisualElement _btnPanel;
    private VisualElement _settingPanel;

    private DropdownField _graphicDropdown;

    private Toggle _windowModeToggle;

    private Slider _masterSlider;
    private Slider _backgroundSlider;
    private Slider _vfxSlider;

    private VisualElement _selectBtn;

    private SettingData _settingData;
    private string graphic;

    private bool openPanel = false;
    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_SettingPanel");

        _btnPanel = _root.Q<VisualElement>("panel-btn");
        int index = 0;
        _selectBtn = _btnPanel[0];
        foreach (VisualElement card in _btnPanel.Children())
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
        _settingPanel = _root.Q<VisualElement>("panel-move");
        _graphicDropdown = _root.Q<DropdownField>("dropdown-graphic");
        _graphicDropdown.choices = new List<string>();
        _graphicDropdown.choices.Add("1920x1080");
        _graphicDropdown.choices.Add("1366x768");
        _graphicDropdown.choices.Add("2560x1440");
        _graphicDropdown.choices.Add("3840x2160");

        _windowModeToggle = _root.Q<Toggle>("windowToggle");
        _windowModeToggle.RegisterCallback<ClickEvent>(_e =>
        {
            _settingData.fullScreenMode = _windowModeToggle.value;
            SetResolution();
        });

        _masterSlider = _root.Q<Slider>("slider-master");
        _backgroundSlider = _root.Q<Slider>("slider-background");
        _vfxSlider = _root.Q<Slider>("slider-vfx");

        _root.style.display = DisplayStyle.None;
        ShowSettingPanel(0);

        _settingData = DataManager.SettingData_;
        _graphicDropdown.value = _settingData.graphic;
        _windowModeToggle.value = _settingData.fullScreenMode;
        _masterSlider.value = _settingData.masterVolume;
        _backgroundSlider.value = _settingData.backgroundVolume;
        _vfxSlider.value = _settingData.vfxVolume;
    }

    public override void Show()
    {
        openPanel = true;

        _root.style.display = DisplayStyle.Flex;

        UIManager.Instance.uiOn = true;
        UIManager.Instance.MoveAndInputStop();
        UIManager.OpenPanels.Push(this);
    }

    public override void Hide()
    {
        openPanel = false;

        _root.style.display = DisplayStyle.None;
        _settingData.graphic = _graphicDropdown.value;
        _settingData.fullScreenMode = _windowModeToggle.value;
        _settingData.masterVolume = (int)_masterSlider.value;
        _settingData.backgroundVolume = (int)_backgroundSlider.value;
        _settingData.vfxVolume = (int)_vfxSlider.value;
        DataManager.SettingData_ = _settingData;
        UIManager.Instance.uiOn = false;
        Define.GetManager<DataManager>().SaveToSettingData();
    }
    public override void Update()
    {
        if(openPanel ==false) return;

        int value = (int)_masterSlider.value;
        _masterSlider.Q<Label>("text-value").text = value.ToString();
        Define.GetManager<SoundManager>().SetMasterVolume(Mathf.Lerp(-80,5,(float)value/100f));
        value = (int)_backgroundSlider.value;
        _backgroundSlider.Q<Label>("text-value").text = value.ToString();
        Define.GetManager<SoundManager>().SetBGmVolume(Mathf.Lerp(-80, 0, (float)value / 100f));
        value = (int)_vfxSlider.value;
        _vfxSlider.Q<Label>("text-value").text = value.ToString();
        Define.GetManager<SoundManager>().SetSFXVolume(Mathf.Lerp(-80, 0, (float)value / 100f));

        if (graphic != _graphicDropdown.value)
        {
            graphic = _graphicDropdown.value;
            SetResolution();
        }
        
    }
    public void SetResolution()
    {
        string[] values = graphic.Split('x');
        Screen.SetResolution(int.Parse(values[0]), int.Parse(values[1]), _settingData.fullScreenMode);
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
        _settingPanel.style.translate = new StyleTranslate(new Translate(0, new Length(num * -100,LengthUnit.Percent)));
    }
}
