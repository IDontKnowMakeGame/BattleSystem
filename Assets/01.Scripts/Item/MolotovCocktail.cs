using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using System;
using Managements.Managers;
using Blocks;

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
        if (setPos == Vector3.right)
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
        else if (setPos == Vector3.left)
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
        else if (setPos == Vector3.forward)
        {
            // 카메라의 회전에 영향받지 않는 고정 오프셋을 설정합니다.
            Vector3 offset = new Vector3(0f, 0.82f, -0.3f);

            // 오브젝트의 위치를 설정합니다.
            transform.position = SetForwardPos(offset);

            transform.rotation = SetRotation(Quaternion.Euler(-90, -90, 0f));

            m_rotateY = -rotate;
            m_rotateX = 0;
        }
        else if (setPos == Vector3.back)
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

        if (setPos == Vector3.right)
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
        // 벽 충돌
        if (m_posX > 0)
        {
            for (int i = 1; i <= m_posX; i++)
            {
                if(DisableRitch(i, 0))
                {
                    m_posX = i - 1;
                    break;
                }
            }
        }
        else
        {
            for (int i = -1; i >= m_posX; i--)
            {
                if (DisableRitch(i, 0))
                {
                    m_posX = i + 1;
                    break;
                }
            }
        }
        if (m_posZ > 0)
        {
            for (int i = 1; i <= m_posZ; i++)
            {
                if (DisableRitch(0, i))
                {
                    m_posZ = i - 1;
                    break;
                }
            }
        }
        else
        {
            for (int i = -1; i >= m_posZ; i--)
            {
                if (DisableRitch(0, i))
                {
                    m_posZ = i + 1;
                    break;
                }
            }
        }
        Debug.Log("M_POSX:" + m_posX + ", M_POSZ:" + m_posZ);
        startPos = transform.position;
        endPos = (InGame.Player.Position + new Vector3(m_posX, 0, m_posZ)).SetY(0);
    }

    private bool DisableRitch(int posX, int posZ)
    {

        MapManager _map = Define.GetManager<MapManager>();
        Vector3 pos = (InGame.Player.Position + new Vector3(posX, 0, posZ)).SetY(0);
        if ((_map.GetBlock(pos.SetY(0)) == null))
        {
            return true;
        }
        return false;
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
            SetFlames();
            Instantiate(explosition, transform.position.SetY(0.5f), Quaternion.identity);

            //Instantiate(flames, transform.position.SetY(0.5f), Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void SetFlames()
    {
        MapManager _map = Define.GetManager<MapManager>();

        for (int z = -1; z <= 1; z++)
        {
            for(int x = -1; x <= 1; x++)
            {
                float exploreZ = endPos.z + z;
                float exploreX = endPos.x + x;

                Vector3 pos = new Vector3(exploreX, 0.5f, exploreZ);

                if (_map.GetBlock(pos.SetY(0)) != null && _map.GetBlock(pos.SetY(0)) is not EmptyBlock)
                {
                    Instantiate(flames, pos, Quaternion.identity);
                    _map.GetBlock(new Vector3(exploreX, 0, exploreZ)).isFire = true;

                    if (Define.GetManager<MapManager>().GetBlock(pos.SetY(0)).ActorOnBlock != null)
                    {
                        Define.GetManager<MapManager>().GetBlock(pos.SetY(0)).ActorOnBlock.GetAct<CharacterStatAct>()?.Burns();
                    }
                }
            }
        }
    }

    private void Move()
    {
        anim += Time.deltaTime;

        transform.position = Parabola(startPos, endPos, 1.4f, anim / 1 - (0.05f * (3 - Mathf.Abs(m_posX + m_posZ))));
    }


    public Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }
}
