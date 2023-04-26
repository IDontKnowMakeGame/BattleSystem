using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

public class UIFirstFloorMap : UIBase
{
    private const Floor floor = Floor.First;

    private VisualElement _mapCast;

    private Dictionary<int,bool> cristalDictionary = new Dictionary<int,bool>();


    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_FirstFloorMap");

        _mapCast = _root.Q<VisualElement>("Cast");
        CristalInit();
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
    public void CristalInit()
    {
        for(int i = 0;i<4;i++)
        {
            cristalDictionary.Add(i, false);
        }
    }

    public void MapCast()
    {
        List<int> list = Define.GetManager<DataManager>().OnCristalData(Floor.First);

        foreach (int i in list)
        {
                cristalDictionary[i] = true;
        }

        CastOnMap();
    }

    public void CastOnMap()
    {
        if(cristalDictionary[0] && cristalDictionary[1])
        {
            _mapCast.Q<VisualElement>("1").style.display = DisplayStyle.None;
        }
    }
}
