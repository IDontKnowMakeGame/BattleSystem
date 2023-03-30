using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
using Walls.Acts;

public class CameraEffecter : MonoBehaviour
{
    private CinemachineVirtualCamera _cvCam;

    private bool _isCameraAction;

    private Shake _damagedShake;
    private Shake _basicShake;
    private Shake _greateSwordShake;
    private void Awake()
    {
        Time.timeScale = 1;
        _cvCam = GetComponent<CinemachineVirtualCamera>();

        _damagedShake = transform.Find("DamagedShake").GetComponent<Shake>();
        _basicShake = transform.Find("BasicShake").GetComponent<Shake>();
        _greateSwordShake = transform.Find("GreateSwordShake").GetComponent<Shake>();
    }
    public void StartCameraAction()
    {

    }
    public void EndCameraAction()
    {

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
        StartCoroutine(ZoomInOut(40, 0.002f));
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
    
    public LayerMask Mask;

    private void Update()
    {
        var playerPos = InGame.Player.transform.position;
        var cam = Define.MainCamera;
        var dir = cam.transform.position - playerPos;
        var hits = Physics.RaycastAll(playerPos, dir.normalized, 3000, Mask);
        foreach (var hit in hits)
        {
            var actor = InGame.GetActor(hit.collider.gameObject.GetInstanceID());
            actor.GetAct<WallRender>().Invisible();
        }
    }
}
