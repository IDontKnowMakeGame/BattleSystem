using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestUICoder : MonoBehaviour
{
    private UIDocument _document;

    private VisualElement _hpBar;
    private VisualElement _angerBar;
    private VisualElement _adranalineBar;

    public  void Awake()
    {
        _document = GetComponent<UIDocument>();

        VisualElement root = _document.rootVisualElement;
        _hpBar = root.Q<VisualElement>("HpBar");
        _angerBar = root.Q<VisualElement>("AngerBar");
        _adranalineBar = root.Q<VisualElement>("AdrenalineBar");


    }

    public  void Start()
    {
        SetHpValue(25);
    }

    public void SetHpValue(float value)
    {
        VisualElement fill = _hpBar.Q<VisualElement>("Fill");
        fill.style.width = new Length(value, LengthUnit.Percent);
    }
    public void SetMaxHpValue(float value)
    {
        VisualElement fill = _hpBar.Q<VisualElement>("BackGround");
        fill.style.width = new Length(value, LengthUnit.Percent);
    }
}
