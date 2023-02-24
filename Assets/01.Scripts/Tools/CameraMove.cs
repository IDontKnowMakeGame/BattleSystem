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
            var ease = Ease.OutQuad;
            var duration = 0.5f;
            var offsetZ = 3;
            if (_currentArea.IsRoute)
            {
                ease = Ease.Linear;
                duration = 0.4f;
                offsetZ = 5;
            }
            Define.MainCam.transform.DOMoveZ(_currentArea.EndPos.z - offsetZ, duration).SetEase(ease);
            Define.MainCam.transform.DOMoveX((_currentArea.StartPos.x + _currentArea.EndPos.x) * 0.5f, duration).SetEase(ease);
        }
    }
}
