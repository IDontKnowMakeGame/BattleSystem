using System;
using System.Collections;
using System.Collections.Generic;
using _01.Scripts.Tools;
using Core;
using DG.Tweening;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private CameraArea _currentArea;
    
    public CameraArea CurrentArea
    {
        get => _currentArea;
        set
        {
            _currentArea = value;
            
            
            Define.MainCam.transform.DOMoveZ(_currentArea.EndPos.z - 3, 0.3f);
            Define.MainCam.transform.DOMoveX((_currentArea.StartPos.x + _currentArea.EndPos.x) * 0.5f, 0.3f);
        }
    }
}
