using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Core;
using Managements.Managers;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    private float targetValue = 45;
    [SerializeField]
    private float targetFov = 50;
    [SerializeField]
    private float duration = 3f;

    private float originalValue;
    private float originFov;
    private bool trigger = false;

    private float timer;

    private void Start()
    {
        originalValue = virtualCamera.transform.rotation.eulerAngles.x;
        originFov = virtualCamera.m_Lens.FieldOfView;
    }
    private void Update()
    {
        if(transform.position == InGame.Player.Position && !trigger)
        {
            timer = 0;
            trigger = true;
        }

        if (trigger)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            // 카메라 회전
            float angle = Mathf.Lerp(originalValue, targetValue, t);
            virtualCamera.transform.rotation = Quaternion.Euler(angle, 180, 0);

            // 카메라 FOV
            float currentFov = Mathf.Lerp(originFov, targetFov, t);
            virtualCamera.m_Lens.FieldOfView = currentFov;
        }
    }
}
