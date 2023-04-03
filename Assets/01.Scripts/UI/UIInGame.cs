using Core;
using Data;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class UIInGame : UIBase
{
    private VisualElement _hpSlider;
    private VisualElement _angerSlider;
    private VisualElement _adrenalineSlider;

    private VisualElement _firstWaepon;
    private VisualElement _secondWeapon;
    private VisualElement _itembox;

    private Label _feather;

    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_InGame");

        _hpSlider = _root.Q<VisualElement>("slider_hpbar");
        _angerSlider = _root.Q<VisualElement>("slider_angerbar");
        _adrenalineSlider = _root.Q<VisualElement>("slider_adrenaline");

        _firstWaepon = _root.Q<VisualElement>("weaponbox_first");
        _secondWeapon = _root.Q<VisualElement>("weaponbox_second");
        _itembox = _root.Q<VisualElement>("itembox");

        _feather = _root.Q<Label>("featherCnt");

        _feather.text = Define.GetManager<DataManager>().GetFeather().ToString();

        ChangeFirstWeaponImage(DataManager.UserData_.firstWeapon);
        ChangeSecondWeaponImage(DataManager.UserData_.secondWeapon);
    }

    public void ChanageMaxHP(int value)
    {
        VisualElement bar = _hpSlider.Q<VisualElement>("bar_hpbackgroud");
        bar.style.width = new Length(value, LengthUnit.Percent);
    }
    public void ChangeCurrentHP(int value)
    {
        VisualElement bar = _hpSlider.Q<VisualElement>("Fill");
        bar.style.width = new Length(value, LengthUnit.Percent);
    }

    public void ChangeAngerValue(int value)
    {
        VisualElement bar = _angerSlider.Q<VisualElement>("Fill");
        bar.style.width = new Length(value, LengthUnit.Percent);
    }
    public void ChangeAdrenalineValue(int value)
    {
        VisualElement bar = _adrenalineSlider.Q<VisualElement>("Fill");
        bar.style.width = new Length(value, LengthUnit.Percent);
    }

    public void ChangeFirstWeaponImage(ItemID itemID)
    {
        Sprite sprite = Define.GetManager<ResourceManager>().Load<Sprite>("Item/" + (int)itemID);
        _firstWaepon.style.backgroundImage = new StyleBackground(sprite);
    }
    public void ChangeSecondWeaponImage(ItemID itemID)
    {
        Sprite sprite = Define.GetManager<ResourceManager>().Load<Sprite>("Item/" + (int)itemID);
        _secondWeapon.style.backgroundImage = new StyleBackground(sprite);
    }
    public void ChangeItemPanelImage(Sprite sprite)
    {
        _itembox.style.backgroundImage = new StyleBackground(sprite);
    }
    public void WriteFeatherValue(int value)
    {
        _feather.text = value.ToString();
    }
}
