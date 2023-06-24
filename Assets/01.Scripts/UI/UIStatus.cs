using Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UIStatus : UIBase
{
    private VisualElement _backGround;

    private VisualElement _characterInfoPanel;
    private VisualElement _weaponInfoPanel;

    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_Status");

        _backGround = _root.Q<VisualElement>("BackGround");

        _characterInfoPanel = _backGround.Q<VisualElement>("charaterInfo");
        _weaponInfoPanel = _backGround.Q<VisualElement>("weaponInfo-panel");


        _root.style.display = DisplayStyle.None;
    }

    public override void Show()
    {
        _root.style.display = DisplayStyle.Flex;
        _backGround.RemoveFromClassList("backGround-hide");

        CharacterStatusUpdate();

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
    public void InitWeaponStatus()
    {
        
    }
    public void UpdateWeaponStatus()
    {

    }
    public void CharacterStatusUpdate()
    {
        Debug.Log("asd");
        Label label = _characterInfoPanel.Q<Label>("label-info-hp");
        label.text = label.text.Replace("x", InGame.Player.GetAct<PlayerStatAct>().BaseStat.maxHP.ToString());
        label = _characterInfoPanel.Q<Label>("label-info-feather");
        label.text = label.text.Replace("x", DataManager.UserData_.feather.ToString());
        //_characterInfoPanel.Q<Label>("label-info-potion").text.Replace("x", "");
        label = _characterInfoPanel.Q<Label>("label-info-weapon");
        label.text = label.text.Replace("x", DataManager.UserData_.firstWeapon.ToString()).Replace("y", DataManager.UserData_.secondWeapon.ToString());
        label = _characterInfoPanel.Q<Label>("label-info-halo");
        label.text = label.text.Replace("x", DataManager.UserData_.firstHalo.ToString());
    }
}
