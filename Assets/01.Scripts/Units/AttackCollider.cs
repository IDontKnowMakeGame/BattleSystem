using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum DirType
{
    Up,
    Down,
    Left,
    Right,
    Size,
}
public class AttackCollider : MonoBehaviour
{
    private BoxCollider[] attackCol = new BoxCollider[4];
    private Vector3[] oriCenterSize = new Vector3[4];
    private Vector3[] oriSize = new Vector3[4];

    private Vector3[] changeCenterSize = new Vector3[4];
    private Vector3[] changeSize = new Vector3[4];

    private void Start()
    {
        // Attack 콜라이더 넣기(U-D-L-R)
        for (int i = 0; i < transform.childCount; i++)
        {
            attackCol[i] = transform.GetChild(i).GetComponent<BoxCollider>();
            oriCenterSize[i] = attackCol[i].center;
            oriSize[i] = attackCol[i].size;
        }

        changeCenterSize = oriCenterSize;
        changeSize = oriSize;
    }

    public BoxCollider GetAttackCol(int direction)
    {
        if(direction <= -1 && direction >= (int)DirType.Size)
        {
            Debug.LogError("Type의 범위가 넘어갔습니다.");
            return new BoxCollider();
        }
        return attackCol[(int)direction];
    }

    public void ChangeSizeX(int direction, int space)
    {
        changeCenterSize[direction].x = oriCenterSize[direction].x * space;
        changeSize[direction].x = oriSize[direction].x * space;

        SetAttackSize(direction);
    }

    public void ChangeSizeX(int space)
    {
        for(int i = 0; i < (int)DirType.Size; i++)
        {
            changeCenterSize[i].x = oriCenterSize[i].x * space;
            changeSize[i].x = oriSize[i].x * space;

            SetAttackSize(i);
        }
    }

    public void ChangeSizeZ(int direction, int space)
    {
        changeCenterSize[direction].z = oriCenterSize[direction].z * space;
        changeSize[direction].z = oriSize[direction].z * space;

        SetAttackSize(direction);
    }

    public void ChangeSizeZ(int space)
    {
        for (int i = 0; i < (int)DirType.Size; i++)
        {
            changeCenterSize[i].z = oriCenterSize[i].z * space;
            changeSize[i].z = oriSize[i].z * space;

            SetAttackSize(i);
        }
    }

    private void SetAttackSize(int direction)
    {
        attackCol[direction].center = changeCenterSize[direction];
        attackCol[direction].size = changeSize[direction];
    }
}
