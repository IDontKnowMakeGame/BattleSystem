using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Core;
using UnityEngine;
using UnityEngine.VFX;

public class Fog : MonoBehaviour
{
    private VisualEffect _visualEffect;
    private void Awake()
    {
        _visualEffect = GetComponent<VisualEffect>();
    }

    void Update()
    {
        _visualEffect.SetVector3("HolePosition", InGame.CameraMove.CurrentArea.StartPos.Center(InGame.CameraMove.CurrentArea.EndPos));
    }
}
