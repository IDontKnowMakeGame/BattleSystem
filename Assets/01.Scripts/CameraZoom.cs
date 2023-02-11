using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    CinemachineVirtualCamera cmVCamera;

    private bool zoomTrigger = false;

    public bool ZoomTrigger
    {
        set
        {
            zoomTrigger = value;
        }
    }

    [SerializeField]
    private float smoothTime;

    [SerializeField]
    private float minSize;

    [SerializeField]
    private float maxSize;

    private float lastZoomSpeed;

    // 1 In -1 Out;
    private int mode = 1;

    private void Start()
    {
        cmVCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void FixedUpdate()
    {
        if (zoomTrigger)
        {
            if (mode == 1) ZoomIn();
            else if (mode == -1) ZoomOut();
        }
    }

    private void Zoom(float size, float targetSize)
    {
        float smoothZoomSize = Mathf.SmoothDamp(size, targetSize, ref lastZoomSpeed, smoothTime);

        cmVCamera.m_Lens.FieldOfView = smoothZoomSize;
    }

    public void ZoomIn()
    {
        float size = cmVCamera.m_Lens.FieldOfView;

        if(size - minSize <= 0.001f)
        {
            cmVCamera.m_Lens.FieldOfView = minSize;
            zoomTrigger = false;
            mode *= -1;
            return;
        }

        Zoom(size, minSize);
    }

    public void ZoomOut()
    {
        float size = cmVCamera.m_Lens.FieldOfView;

        if (maxSize - size <= 0.001f)
        {
            cmVCamera.m_Lens.FieldOfView = maxSize;
            zoomTrigger = false;
            mode *= -1;
            return;
        }

        Zoom(size, maxSize);
    }

    public IEnumerator ZoomInOut(float changeTime)
    {
        mode = 1;

        zoomTrigger = true;

        yield return new WaitForSeconds(changeTime);

        mode = -1;

        zoomTrigger = true;
    }
}
