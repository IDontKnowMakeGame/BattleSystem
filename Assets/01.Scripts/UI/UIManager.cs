using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Managements.Managers.Base;
using Core;

public class UIManager : Manager
{
    static GameObject ob;
    private UIDocument _document;

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
        _document.visualTreeAsset = Define.GetManager<ResourceManagers>().Load<VisualTreeAsset>("UIDoc/InGame");
        VisualElement root = _document.rootVisualElement;

        _hpBar = root.Q<VisualElement>("HpBar");
        _angerBar = root.Q<VisualElement>("AngerBar");
        _adranalineBar = root.Q<VisualElement>("AdrenalineBar");

       // _firstWeaponIamge = root.Q<VisualElement>("AdrenalineBar");
        //_secondWeaponImage = root.Q<VisualElement>("AdrenalineBar");
    }

    #region HpSlider
    public void SetMaxHpValue(int value)
    {
        VisualElement fill = _hpBar.Q<VisualElement>("BackGround");
        fill.style.width = new Length(value, LengthUnit.Percent);
    }
    public void SetHpValue(int value)
    {
        VisualElement fill = _hpBar.Q<VisualElement>("Fill");
        fill.style.width = new Length(value, LengthUnit.Percent);
    }
    #endregion

    #region AngerSlider
    public void SetAngerValue(float value)
    {
        VisualElement fill = _angerBar.Q<VisualElement>("Fill");
        fill.style.width = new Length(value, LengthUnit.Percent);
    }
    #endregion

    #region AdranalineSlider
    public void SetAdranalineValue(float value)
    {
        VisualElement fill = _adranalineBar.Q<VisualElement>("Fill");
        fill.style.width = new Length(value, LengthUnit.Percent);
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
