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
            transform.position = InGame.Player.Position + new Vector3(0.1f, 0.82f, -0.3f);
            transform.rotation = Quaternion.Euler(-120, -90, 0f);
            m_rotateX = -rotate;
            m_posX = pos;
            m_rotateY = 0;
            m_posZ = 0;
        }
        else if(setPos == Vector3.left)
        {
            transform.position = InGame.Player.Position + new Vector3(-0.1f, 0.82f, -0.3f);
            transform.rotation = Quaternion.Euler(-45, -90, 0f);
            m_rotateX = rotate;
            m_posX = -pos;
            m_rotateY = 0;
            m_posZ = 0;
        }
        else if(setPos == Vector3.forward)
        {
            transform.position = InGame.Player.Position + new Vector3(0f, 0.82f, -0.3f);
            transform.rotation = Quaternion.Euler(-90, -90, 0f);
            m_rotateY = -rotate;
            m_posZ = pos;
            m_rotateX = 0;
            m_posX = 0;
        }
        else if(setPos == Vector3.back)
        {
            transform.position = InGame.Player.Position + new Vector3(0f, 0.82f, -0.3f);
            transform.rotation = Quaternion.Euler(-90, -90, 0f);
            m_rotateY = rotate;
            m_posZ = -pos;
            m_rotateX = 0;
            m_posX = 0;
        }

        startPos = transform.position;
        endPos = (transform.position + new Vector3(m_posX, 0, m_posZ)).SetY(0);
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
