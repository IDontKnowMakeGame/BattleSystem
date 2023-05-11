using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Core;
using Managements.Managers;
using Unity.VisualScripting;

public class SwitchCamera : MonoBehaviour
{
    private static SwitchCamera CurrentOnSwitchCamera;
    public static float FOV = 40;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    private float verticalTagetAngle = 45;
    public float VerticalTargetAngle
    {
        get => verticalTagetAngle;
        set => verticalTagetAngle = value;
    }
    [SerializeField]
    private float horizontalTargetAngle = 0;
    public float HorizontalTargetAngle
    {
        get => horizontalTargetAngle;
        set => horizontalTargetAngle = value;
    }

    private float setHorizontalTarget;

    [SerializeField]
    private float targetFov = 50;
    public float TargetFov
    {
        get => targetFov;
        set => targetFov = value;
    }
    [SerializeField]
    private float duration = 3f;
    public float Duration
    {
        get => duration;
        set => duration = value;
    }

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
            float angleY = Mathf.Lerp(originalHorizontalValue, setHorizontalTarget, t);
            virtualCamera.transform.rotation = Quaternion.Euler(angleX, angleY, 0);

            // 카메라 FOV
            float currentFov = Mathf.Lerp(originFov, targetFov, t);
            virtualCamera.m_Lens.FieldOfView = currentFov;
        }

        if(timer >= duration && trigger == true)
        {
            FOV = virtualCamera.m_Lens.FieldOfView;
            trigger = false;
        }
    }

    public void Init()
    {
        timer = 0;
        originalVerticalValue = virtualCamera.transform.rotation.eulerAngles.x;
        originalHorizontalValue =  virtualCamera.transform.rotation.eulerAngles.y;

        setHorizontalTarget = horizontalTargetAngle;


        bool isLeftFaster = IsLeftTurnFaster(originalHorizontalValue, horizontalTargetAngle);


        if (isLeftFaster)
        {
            setHorizontalTarget = originalHorizontalValue + Mathf.DeltaAngle(originalHorizontalValue, horizontalTargetAngle);
            Debug.Log("왼쪽");
        }


        originFov = virtualCamera.m_Lens.FieldOfView;

        trigger = true;
    }

    // A 각도에서 B 각도로 돌 때, 왼쪽이 더 빠른지 판단하는 함수
    public bool IsLeftTurnFaster(float aAngle, float bAngle)
    {
        // 각도의 차이를 계산
        float angleDifference = Mathf.DeltaAngle(aAngle, bAngle);

        Debug.Log(angleDifference + "일거임");

        Debug.Log(angleDifference);

        // 왼쪽으로 도는 각도 차이가 작으면 더 빠른 방향으로 판단
        return Mathf.Abs(angleDifference) < 180f;
    }
}
