using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Core;

[System.Serializable]
public class Shake : MonoBehaviour
{
    CinemachineImpulseSource screenShake;

    [SerializeField] float shakeForce;
    private void Awake()
    {
        screenShake = GetComponent<CinemachineImpulseSource>();
    }
    public void ScreenShake()
    {
        screenShake.GenerateImpulse(shakeForce);
    }
}
