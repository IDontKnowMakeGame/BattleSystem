using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using System;

public class MolotovCocktail : MonoBehaviour
{
    public GameObject explosition;
    public GameObject flames;

    private Vector3 startPos, endPos;

    public bool isPlay = false;

    private float anim;

    private const float rotate = 8;
    private const float pos = 3;

    private float m_rotateX, m_posX;
    private float m_rotateY, m_posZ;

    public void InitBottle(Vector3 setPos)
    {
        if(setPos == Vector3.right)
        {
            // 카메라의 회전에 영향받지 않는 고정 오프셋을 설정합니다.
            Vector3 offset = new Vector3(0.1f, 0.65f, -1f);

            // 오브젝트의 위치를 설정합니다.
            transform.position = SetForwardPos(offset);

            transform.rotation = SetRotation(Quaternion.Euler(-120f, -90f, 0f));

            InGame.Player.SpriteTransform.localScale = new Vector3(2, 1, 1);
            m_rotateX = -rotate;
            m_rotateY = 0;
        }
        else if(setPos == Vector3.left)
        {
            // 카메라의 회전에 영향받지 않는 고정 오프셋을 설정합니다.
            Vector3 offset = new Vector3(-0.1f, 0.65f, -1f);

            // 오브젝트의 위치를 설정합니다.
            transform.position = SetForwardPos(offset);

            transform.rotation = SetRotation(Quaternion.Euler(-45, -90, 0f));

            InGame.Player.SpriteTransform.localScale = new Vector3(-2, 1, 1);
            m_rotateX = rotate;
            m_rotateY = 0;
        }
        else if(setPos == Vector3.forward)
        {
            // 카메라의 회전에 영향받지 않는 고정 오프셋을 설정합니다.
            Vector3 offset = new Vector3(0f, 0.82f, -0.3f);

            // 오브젝트의 위치를 설정합니다.
            transform.position = SetForwardPos(offset);

            transform.rotation = SetRotation(Quaternion.Euler(-90, -90, 0f));

            m_rotateY = -rotate;
            m_rotateX = 0;
        }
        else if(setPos == Vector3.back)
        {
            // 카메라의 회전에 영향받지 않는 고정 오프셋을 설정합니다.
            Vector3 offset = new Vector3(0f, 0.82f, -0.3f);

            // 오브젝트의 위치를 설정합니다.
            transform.position = SetForwardPos(offset);

            transform.rotation = SetRotation(Quaternion.Euler(-90, -90, 0f));

            m_rotateY = rotate;
            m_rotateX = 0;
        }

        setPos = InGame.CamDirCheck(setPos);

        if(setPos == Vector3.right)
        {
            m_posX = pos;
            m_posZ = 0;
        }
        else if (setPos == Vector3.left)
        {
            m_posX = -pos;
            m_posZ = 0;
        }
        else if (setPos == Vector3.forward)
        {
            m_posZ = pos;
            m_posX = 0;
        }
        else if (setPos == Vector3.back)
        {
            m_posZ = -pos;
            m_posX = 0;
        }

        startPos = transform.position;
        endPos = (InGame.Player.Position + new Vector3(m_posX, 0, m_posZ)).SetY(0);
    }

    private Vector3 SetForwardPos(Vector3 offset)
    {
        // 카메라의 회전에 영향을 받지 않도록 카메라의 forward 벡터를 기준으로 오브젝트의 위치를 설정합니다.
        Vector3 targetPosition = InGame.Player.Position + Camera.main.transform.forward * offset.z
                                  + Camera.main.transform.right * offset.x
                                  + Camera.main.transform.up * offset.y;

        return targetPosition;
    }

    private Quaternion SetRotation(Quaternion setRot)
    {
        Quaternion cameraRotation = Camera.main.transform.rotation;
        Quaternion diagonalRotation = setRot;

        Quaternion rotation = cameraRotation * diagonalRotation;
        return rotation;
    }

    void Update()
    {
        if (isPlay)
        {
            transform.Rotate(new Vector3(m_rotateX, m_rotateY, 0));
            Move();
        }

        if (transform.position.y < 0)
        {
            Instantiate(explosition, transform.position.SetY(0.5f), Quaternion.identity);
            Instantiate(flames, transform.position.SetY(0.5f), Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        anim += Time.deltaTime;

        transform.position = Parabola(startPos, endPos, 1.4f, anim / 1f);
    }


    public Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }
}
