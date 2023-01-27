using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Shake : MonoBehaviour
{
    [SerializeField] CinemachineImpulseSource screenShake;
    [SerializeField] private float powerAmount;

    public void ScreenShake(Vector3 dir)
    {
        screenShake.GenerateImpulseWithVelocity(dir * powerAmount);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ScreenShake(new Vector3(1,1,1));
        }
    }
}
