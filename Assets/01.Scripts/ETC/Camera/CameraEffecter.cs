using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
using Walls.Acts;
using static UnityEngine.Rendering.DebugUI;
using Unity.VisualScripting;

public class CameraEffecter : MonoBehaviour
{
    [SerializeField]
    private float duration = 0.5f;

    private CinemachineVirtualCamera _cvCam;

    private float originalFOV = 40;
    private bool _isCameraAction;

    public bool trigger = false;

    private float timer;
    private float originFov;
    private float targetFov;

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
        _isCameraAction = true;
    }
    public void EndCameraAction()
    {
        _isCameraAction = false;
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
    public void ZoomIn(float time)
    {
        Debug.Log("ZoomIn");
        originFov = _cvCam.m_Lens.FieldOfView;
        targetFov = 20;
        timer = 0;
        duration = time;
        trigger = true;
    }
    public void ZoomOut()
    {
        Debug.Log($"ZoomOut : {SwitchCamera.FOV}");
        originFov = _cvCam.m_Lens.FieldOfView;
        targetFov = SwitchCamera.FOV;
        timer = 0;
        duration = 2f;
        trigger = true;
    }
    private void Update()
    {
        if(trigger)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            // ī�޶� FOV
            float currentFov = Mathf.Lerp(originFov, targetFov, t);
            _cvCam.m_Lens.FieldOfView = currentFov;
        }

        if (timer >= duration)
        {
            timer = 0;
            trigger = false;
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


    private void LateUpdate()
    {
        if(InGame.Player == null) return;
        var playerPos = InGame.Player.transform.position;
        var cam = Define.MainCamera;
        var dir = cam.transform.position - playerPos;
        RaycastHit[] hit0 = new RaycastHit[3000]; 
        RaycastHit[] hit1 = new RaycastHit[3000]; 
        RaycastHit[] hit2 = new RaycastHit[3000]; 
        var size0 = Physics.RaycastNonAlloc(playerPos, dir.normalized, hit0, 3000, Mask);
        var size1 = Physics.RaycastNonAlloc(playerPos + Vector3.right, dir.normalized, hit1, 3000, Mask);
        var size2 = Physics.RaycastNonAlloc(playerPos + Vector3.left, dir.normalized, hit2, 3000, Mask);
        for (var i = 0; i < size0; i++)
        {
            var actor = InGame.GetActor(hit0[i].collider.gameObject.GetInstanceID());
            actor.GetAct<WallRender>()?.Invisible();
        }
        for (var i = 0; i < size1; i++)
        {
            var actor = InGame.GetActor(hit1[i].collider.gameObject.GetInstanceID());
            actor.GetAct<WallRender>()?.Invisible();
        }
        for (var i = 0; i < size2; i++)
        {
            var actor = InGame.GetActor(hit2[i].collider.gameObject.GetInstanceID());
            actor.GetAct<WallRender>()?.Invisible();
        }
    }
}
