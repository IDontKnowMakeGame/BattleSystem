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

    public void CameraShaking(float strength, float shakingTime)
    {
        usingCamera.CameraShake(strength, shakingTime);
    }
    public void CameraZooming(float strength, float zoominTime, float waitTime, float zoomOutTime)
    {
        usingCamera.CameraZoom(strength,zoominTime,waitTime,zoomOutTime);
    }
}
