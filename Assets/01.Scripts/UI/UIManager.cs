using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Managements.Managers.Base;

public class UIManager : Manager
{
    private UIDocument _document;

    private VisualElement _hpBar;
    private VisualElement _angerBar;
    private VisualElement _adranalineBar;

    private VisualElement _firstIamge;
    private VisualElement _secondImage;

    

    public override void Awake()
    {
        Init();
    }
    private void Init()
    {
        VisualElement root = _document.rootVisualElement;

        _hpBar = root.Q<VisualElement>("hpBar");
        _angerBar = root.Q<VisualElement>("AngerBar");
        _adranalineBar = root.Q<VisualElement>("AdrenalineBar");

        _firstIamge = root.Q<VisualElement>("AdrenalineBar");
        _secondImage = root.Q<VisualElement>("AdrenalineBar");
    }

    public void SetMaxHpValue(float value)
    {
        VisualElement fill = _hpBar.Q<VisualElement>("BackGround");
        fill.style.width = new Length(value, LengthUnit.Percent);
    }
    public void SetHpValue(float value)
    {
        VisualElement fill = _hpBar.Q<VisualElement>("Fill");
        fill.style.width = new Length(value, LengthUnit.Percent);
    }
    public void SetAngerValue(float value)
    {
        VisualElement fill = _angerBar.Q<VisualElement>("Fill");
        fill.style.width = new Length(value, LengthUnit.Percent);
    }
    public void SetAdranalineValue(float value)
    {
        VisualElement fill = _adranalineBar.Q<VisualElement>("Fill");
        fill.style.width = new Length(value, LengthUnit.Percent);
    }

    public void StartGame()
    { 
        SceneManager.LoadScene("BossBattle");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
