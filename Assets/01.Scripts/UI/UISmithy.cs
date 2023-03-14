using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UISmithy : MonoBehaviour
{
    private string _selectWeaponName;
    private VisualTreeAsset _weaponCardTemp;
    private VisualElement _weaponImage;
    private Label _weaponName;

    private Label _atkText;
    private Label _atsText;
    private Label _afsText;
    private Label _weightText;

    private Label _needMoneyText;
    private Label _needItemText;

    private ScrollView _weaponScroll;
    private VisualElement _weaponUpgradeBtn;

    //public void WeaponStoreInit()
    //{
    //    if (currentUI == MyUI.WeaponStore) return;

    //    _onOtherPanel = false;

    //    currentUI = MyUI.WeaponStore;
    //    _document.visualTreeAsset = Define.GetManager<ResourceManagers>().Load<VisualTreeAsset>("UIDoc/WeaponStore");
    //    VisualElement root = _document.rootVisualElement;

    //    _weaponCardTemp = Define.GetManager<ResourceManagers>().Load<VisualTreeAsset>("UIDoc/WeaponCardTemp");

    //    _weaponScroll = root.Q<ScrollView>("WeaponScroll");
    //    _weaponName = root.Q<Label>("WeaponName");

    //    _exitBtn = root.Q<VisualElement>("ExitBtn");
    //    _exitBtn.RegisterCallback<ClickEvent>(e =>
    //    {
    //        InGameInit();
    //    });

    //    _weaponUpgradeBtn = root.Q<VisualElement>("Btn");
    //    _weaponUpgradeBtn.RegisterCallback<ClickEvent>(e =>
    //    {
    //        WeaponUpGradeBtn();
    //    });

    //    VisualElement rightWindow = root.Q<VisualElement>("RightWindow");

    //    _atkText = rightWindow.Q<Label>("AtkBoxLabel");
    //    _atsText = rightWindow.Q<Label>("AtsBoxLabel");
    //    _afsText = rightWindow.Q<Label>("AfsBoxLabel");
    //    _weightText = rightWindow.Q<Label>("WeightBoxLabel");

    //    _needMoneyText = rightWindow.Q<Label>("NeedMoneyBox/Label");
    //    _needItemText = rightWindow.Q<Label>("NeedItemBox/Label");
    //}
}
