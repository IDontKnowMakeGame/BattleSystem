using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Managements.Managers.Base;
using Core;
using System;

public class UIManager : Manager
{
    static GameObject ob;
    private UIDocument _document;

    private enum MyUI
    {
        InGame,
        Dialog,
        Inventory,
        none
    }

    private MyUI currentUI = MyUI.none;

    #region InGame
    private VisualElement _hpBar;
    private VisualElement _angerBar;
    private VisualElement _adranalineBar;

    private VisualElement _firstWeaponIamge;
    private VisualElement _secondWeaponImage;

    private VisualElement _potionImage;
    private VisualElement _itemImage;

    private VisualElement _featherPanel;
    #endregion

    #region Dialog
    private VisualElement _sentencePanel;
    private VisualElement _choicePanel;
    #endregion


    public override void Awake()
    {
        if(ob == null)
        {
            ob = new GameObject();
            ob.AddComponent<UIDocument>();
        }
        _document = ob.GetComponent<UIDocument>();
        _document.panelSettings = Define.GetManager<ResourceManagers>().Load<PanelSettings>("UI Toolkit/PanelSettings");

        InGameInit();
    }
    public void InGameInit()
    {
        if (currentUI == MyUI.InGame) return;

        currentUI = MyUI.InGame;
        _document.visualTreeAsset = Define.GetManager<ResourceManagers>().Load<VisualTreeAsset>("UIDoc/InGame");
        VisualElement root = _document.rootVisualElement;

        _hpBar = root.Q<VisualElement>("HpBar");
        _angerBar = root.Q<VisualElement>("AngerBar");
        _adranalineBar = root.Q<VisualElement>("AdrenalineBar");

       // _firstWeaponIamge = root.Q<VisualElement>("AdrenalineBar");
        //_secondWeaponImage = root.Q<VisualElement>("AdrenalineBar");
    }
    public void DialogInit()
    {
        if (currentUI == MyUI.Dialog) return;

        currentUI = MyUI.Dialog;
        _document.visualTreeAsset = Define.GetManager<ResourceManagers>().Load<VisualTreeAsset>("UIDoc/Dialog");
        VisualElement root = _document.rootVisualElement;

        _sentencePanel = root.Q<VisualElement>("TextBox");
        _choicePanel = root.Q<VisualElement>("ChoicePanel");
    }

    #region HpSlider
    public void SetMaxHpValue(int value)
    {
        InGameInit();

        VisualElement fill = _hpBar.Q<VisualElement>("BackGround");
        fill.style.width = new Length(value, LengthUnit.Percent);
    }
    public void SetHpValue(int value)
    {
        InGameInit();

        VisualElement fill = _hpBar.Q<VisualElement>("Fill");
        fill.style.width = new Length(value, LengthUnit.Percent);
    }
    #endregion

    #region AngerSlider
    public void SetAngerValue(float value)
    {
        InGameInit();

        VisualElement fill = _angerBar.Q<VisualElement>("Fill");
        fill.style.width = new Length(value, LengthUnit.Percent);
    }
    #endregion

    #region AdranalineSlider
    public void SetAdranalineValue(float value)
    {
        InGameInit();

        VisualElement fill = _adranalineBar.Q<VisualElement>("Fill");
        fill.style.width = new Length(value, LengthUnit.Percent);
    }
    #endregion

    #region Dialog
    public void CreateDialog(string text)
    {
        DialogInit();

        Label labeltext = _sentencePanel.Q<Label>("Label");
        labeltext.text = text;
    }

    public void CreateChoiceBox(string text,Action action)
    {
        DialogInit();

        VisualTreeAsset temple = Define.GetManager<ResourceManagers>().Load<VisualTreeAsset>("UIDoc/ChoiceBox");
        VisualElement choiceBox = temple.Instantiate();
        Label label = choiceBox.Q<Label>("Text");
        label.text = text;

        choiceBox.RegisterCallback<ClickEvent>(e =>
        {
            action();
        });

        _choicePanel.Add(choiceBox);
    }
    #endregion

    #region GameOption
    public void StartGame()
    { 
        SceneManager.LoadScene("BossBattle");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
    #endregion
}
