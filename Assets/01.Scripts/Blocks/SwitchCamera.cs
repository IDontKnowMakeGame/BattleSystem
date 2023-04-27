using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Core;
using Managements.Managers;

public class SwitchCamera : MonoBehaviour
{
    private static SwitchCamera CurrentOnSwitchCamera;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    private float verticalTagetAngle = 45;
    [SerializeField]
    private float horizontalTargetAngle = 0;
    [SerializeField]
    private float targetFov = 50;
    [SerializeField]
    private float duration = 3f;

    private float originalVerticalValue;
    private float originalHorizontalValue;
    private float originFov;

    public bool trigger = false;

    private float timer;

    private void Start()
    {
        timer = 0;
        originalVerticalValue = virtualCamera.transform.rotation.eulerAngles.x;
        originalHorizontalValue = virtualCamera.transform.rotation.eulerAngles.y;
        originFov = virtualCamera.m_Lens.FieldOfView;
    }
    private void Update()
    {
        if(transform.position == InGame.Player.Position && !trigger)
        {
            Init();
            if(CurrentOnSwitchCamera!=this && CurrentOnSwitchCamera != null)
            {
                Debug.Log(CurrentOnSwitchCamera.name);
                CurrentOnSwitchCamera.trigger = false;
            }
                
            CurrentOnSwitchCamera = this;
        }

        if (trigger)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            // 카메라 회전
            float angleX = Mathf.Lerp(originalVerticalValue, verticalTagetAngle, t);
            float angleY = Mathf.Lerp(originalHorizontalValue, horizontalTargetAngle, t);
            virtualCamera.transform.rotation = Quaternion.Euler(angleX, angleY, 0);

            // 카메라 FOV
            float currentFov = Mathf.Lerp(originFov, targetFov, t);
            virtualCamera.m_Lens.FieldOfView = currentFov;
        }

        if(timer >= duration)
        {
            trigger = false;
        }
    }

    public void Init()
    {
        timer = 0;
        trigger = true;
        originalVerticalValue = virtualCamera.transform.rotation.eulerAngles.x;
        originalHorizontalValue = virtualCamera.transform.rotation.eulerAngles.y;
        originFov = virtualCamera.m_Lens.FieldOfView;
    }
}
