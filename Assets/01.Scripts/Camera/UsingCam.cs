using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class UsingCam : BaseCam
{
    [Header("CameraShake")]
    [Range(0, 10)]
    [SerializeField] private float ShakeStrength;
    [Range(0f,1f)]
    [SerializeField] private float ShakeTime;

    float startStrength = 0;
    float shaketimer = 0;
    CinemachineVirtualCamera vitualCam;
    CinemachineBasicMultiChannelPerlin channelPerlin;

    private void Awake()
    {
        vitualCam = GetComponent<CinemachineVirtualCamera>();
        channelPerlin = vitualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        channelPerlin.m_AmplitudeGain = 0;
    }

    private void Update()
    {
        if(shaketimer > 0)
        {
            shaketimer -= Time.deltaTime;
            channelPerlin.m_AmplitudeGain = Mathf.Lerp(0, startStrength, shaketimer / ShakeTime);
        }
    }

    public override void CameraShake()
    {
        channelPerlin.m_AmplitudeGain = ShakeStrength;
        startStrength = ShakeStrength;

        shaketimer = ShakeTime;
    }

    public override void CameraShake(float strength, float shaketime)
    {
        channelPerlin.m_AmplitudeGain = strength;
        startStrength = strength;

        shaketimer = shaketime;
    }

    public override void CameraZoom(float strength = 1.5f, float zoominTime = 0.5f, float waitingTime = 1f, float zoomOutTime = 0.5f)
    {
        StartCoroutine(CameraZoomCoroutine(strength, zoominTime, zoomOutTime,new WaitForSeconds(waitingTime)));
    }

    IEnumerator CameraZoomCoroutine(float str, float zinT, float zoutT,WaitForSeconds wait)
    {
        DOTween.To(() => vitualCam.m_Lens.FieldOfView, x => vitualCam.m_Lens.FieldOfView = x, 60 / str, zinT);
        yield return wait;
        DOTween.To(() => vitualCam.m_Lens.FieldOfView, x => vitualCam.m_Lens.FieldOfView = x, 60, zoutT);
    }
}
