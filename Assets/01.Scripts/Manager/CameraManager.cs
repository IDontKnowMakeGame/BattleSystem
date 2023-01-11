using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : IManager
{
    BaseCam usingCamera;
    public override void Awake()
    {
        usingCamera = GameObject.FindObjectOfType<BaseCam>();
    }

    public override void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CameraShaking();
        }
    }

    public void CameraShaking()
    {
        usingCamera.CameraZoom(2,1f,2f,0.5f);
    }
}
