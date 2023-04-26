using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

public class UIFirstFloorMap : UIBase
{
    private const Floor floor = Floor.First;

    private VisualElement mapCast;

    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_FirstFloorMap");

        mapCast = _root.Q<VisualElement>("Cast");
    }

    public void FlagDisplay(bool flag)
    {
        if (flag)
        {
            _root.style.display = DisplayStyle.Flex;
        }
        else
        {
            _root.style.display = DisplayStyle.None;
        }
    }
    public void ShowFirstFloorMap()
    {
        FlagDisplay(true);
        MapCast();
    }

    public void MapCast()
    {

    }
}
