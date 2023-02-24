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
    private Transform _cameraAnchor;
    
    public CameraArea CurrentArea
    {
        get => _currentArea;
        set
        {
            _currentArea = value;
            if (_currentArea.IsFollow)
                return;
            var ease = Ease.OutQuad;
            var duration = 0.5f;
            var offsetZ = 3;
            if (_currentArea.IsRoute)
            {
                ease = Ease.Linear;
                duration = 0.4f;
                offsetZ = 5;
            }
            var camPos = new Vector3((_currentArea.StartPos.x + _currentArea.EndPos.x) * 0.5f, 4, _currentArea.EndPos.z - offsetZ);
            var anchorPos = _currentArea.StartPos.Center(_currentArea.EndPos);
            camPos -= anchorPos;
            camPos = Quaternion.Euler(0, _currentArea.Rotation, 0) * camPos;
            _cameraAnchor.DOMove(anchorPos, duration).SetEase(ease);
            Define.MainCam.transform.DOLocalMove(camPos, duration).SetEase(ease);
            Define.MainCam.transform.DORotate(new Vector3(30, _currentArea.Rotation, 0), duration);
        }
    }

    private void Awake()
    {
        _cameraAnchor = Define.MainCam.transform.parent;
    }

    private void Update()
    {
        if (_currentArea.IsFollow)
        {
            Transform transform1;
            (transform1 = Define.MainCam.transform).rotation = Quaternion.Euler(30, _currentArea.Rotation, 0);
            _cameraAnchor.position = InGame.PlayerBase.transform.position + Vector3.up * 4;
            var camPos = Vector3.back * 6;
            camPos = Quaternion.Euler(0, _currentArea.Rotation, 0) * camPos;
            Define.MainCam.transform.localPosition = camPos;
        }
    }
}
