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

    private void Start()
    {
        EventManager.StartListening(EventFlag.CameraShake, ScreenShake);
    }
    public void ScreenShake(EventParam dir)
    {
        float amount = Random.Range(minAmount, maxAmount);
        screenShake.GenerateImpulse(amount);
    }
}
