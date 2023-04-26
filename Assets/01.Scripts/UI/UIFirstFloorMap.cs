using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

public class UIFirstFloorMap : UIBase
{
    private const Floor floor = Floor.First;
    private bool isOpen = false;

    private VisualElement _mapCast;

    private Dictionary<int,bool> cristalDictionary = new Dictionary<int,bool>();

    private Dictionary<int, List<int>> castMap = new Dictionary<int, List<int>>()
    {
        {0,new List<int>{ 0} },
        {1,new List<int>{ 1,2} },
        {2,new List<int>{3}},
        {3,new List<int>{4,5,6}},
        {4,new List<int>{7,8}},
        {5,new List<int>{9}},
        {6,new List<int>{10,11}},
        {7,new List<int>{12,13}},
        {8,new List<int>{14}},
        {9,new List<int>{15}},
        {10,new List<int>{16,17,18,19}},
        {11,new List<int>{20}},
        {12,new List<int>{21,22}},
        {13,new List<int>{23,24}},
        {14,new List<int>{25}},
        {15,new List<int>{26}},
        {16,new List<int>{27,28}},
        {17,new List<int>{29,30,31}},
        {18,new List<int>{32}},
        {19,new List<int>{33,34}},
        {20,new List<int>{35}},
        {21,new List<int>{36}},
        {22,new List<int>{37}},
        {23,new List<int>{38}},
        {24,new List<int>{39}},
        {25,new List<int>{40}},
        {26,new List<int>{}}
    };

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
        if(!isOpen)
        {
            isOpen = true;
            MapCast();
        }
        else
            isOpen = false;

        FlagDisplay(isOpen);
    }
    public void CristalInit()
    {
        for(int i = 0;i<41;i++)
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
        bool isCast = true;
        for (int i = 0 ; i< 27;i++)
        {
            foreach(int k in castMap[i])
            {
                if (cristalDictionary[i] == false)
                    isCast = false;
            }

            if(isCast)
                _root.Q<VisualElement>($"{i}").style.display = DisplayStyle.None;

            isCast = true;
        }
    }
}
