using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class Shake : MonoBehaviour
{
    public string name = string.Empty;
    [SerializeField] CinemachineImpulseSource screenShake;
    [SerializeField] private float minAmount;
    [SerializeField] private float maxAmount;

    public void ScreenShake(Vector3 dir)
    {
        float amount = Random.Range(minAmount, maxAmount);
        screenShake.GenerateImpulse(amount);
    }
}
