using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private CinemachineVirtualCamera _cvCam;
    [SerializeField]
    private float value;

    private Shake _damagedShake;
    private Shake _basicShake;
    private Shake _greateSwordShake;
    private void Awake()
    {
        _cvCam = GetComponent<CinemachineVirtualCamera>();

        _damagedShake = transform.Find("DamagedShake").GetComponent<Shake>();
        _basicShake = transform.Find("BasicShake").GetComponent<Shake>();
        _greateSwordShake = transform.Find("GreateSwordShake").GetComponent<Shake>();
    }
    public void DamagedShake()
    {
        _damagedShake.ScreenShake();
    }
    public void BasicAttackShake()
    {
        _basicShake.ScreenShake();
    }
    public void GreateSwordAttackShake()
    {
        _greateSwordShake.ScreenShake();
    }
    public void ZoomIn(float value)
    {
        StopAllCoroutines();
        StartCoroutine(ZoomInOut(value, 0.002f));
    }
    public void ZoomOut()
    {
        StopAllCoroutines();
        StartCoroutine(ZoomInOut(60, 0.002f));
    }
    private IEnumerator ZoomInOut(float value, float smoothTime)
    {

        if (_cvCam.m_Lens.FieldOfView > value)
        {
            while (_cvCam.m_Lens.FieldOfView >= value)
            {
                _cvCam.m_Lens.FieldOfView -= 1;
                yield return new WaitForSeconds(smoothTime);
            }
        }
        else
        {
            while (_cvCam.m_Lens.FieldOfView <= value)
            {
                _cvCam.m_Lens.FieldOfView += 1;
                yield return new WaitForSeconds(smoothTime);
            }
        }

    }
    public void TimeSlow()
    {
        Time.timeScale = 0.1f;
    }
    public void TimeSet()
    {
        Time.timeScale = 1;
    }

}
