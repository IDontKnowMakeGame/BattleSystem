using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirType
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
    private AttackRange[] attackRanges = new AttackRange[4];
    private Vector3[] oriCenterSize = new Vector3[4];
    private Vector3[] oriSize = new Vector3[4];

    private Vector3[] changeCenterSize = new Vector3[4];
    private Vector3[] changeSize = new Vector3[4];

    private void Start()
    {
        // Attack �ݶ��̴� �ֱ�(U-D-L-R)
        for (int i = 0; i < transform.childCount; i++)
        {
            attackCol[i] = transform.GetChild(i).GetComponent<BoxCollider>();
            attackRanges[i] = transform.GetChild(i).GetComponent<AttackRange>();
            oriCenterSize[i] = changeCenterSize[i] = attackCol[i].center;
            oriSize[i] = changeSize[i] = attackCol[i].size;
        }

        /*-----------------TestCase--------------------*/

        // ��(W)
        /*
        // 1�����
        AllDisableDir();
        ChangeSizeX(DirType.Up, 1);
        EnableDir(DirType.Up);
        // 2�����
        AllDisableDir();
        ChangeSizeX(1);
        EnableDir(DirType.Up);
        */
        /*
        // ���(W)
        AllDisableDir();
        ChangeSizeX(DirType.Up, 1);
        EnableDir(DirType.Up);
        */
        // �ְ�(W)
        /*
        // ���ݹ��� 1����
        AllDisableDir();
        ChangeSizeZ(2);
        EnableDir(DirType.Left, DirType.Right);
        */
        /*
        // ���ݹ��� 2����
        AllDisableDir();
        ChangeSizeZ(2);
        EnableDir(DirType.Left, DirType.Right);
        ChangeSizeX(DirType.Up, 3);
        ChangeSizeZ(DirType.Up, 1);
        EnableDir(DirType.Up);
        */
        // â(W)
        /*        AllDisableDir();
                ChangeSizeZ(DirType.Up, 2);
                EnableDir(DirType.Up);*/
        // â(D)
        /*
        AllDisableDir();
        ChangeSizeX(DirType.Right, 2);
        EnableDir(DirType.Right);
        */
        /*
        // Ȱ(W)
        AllDisableDir();
        ChangeSizeZ(DirType.Up, 6);
        EnableDir(DirType.Up); 
        */

        ChangeSizeX(2);
    }

    public BoxCollider GetAttackCol(int direction)
    {
        if(direction <= -1 && direction >= (int)DirType.Size)
        {
            Debug.LogError("Type�� ������ �Ѿ���ϴ�.");
            return new BoxCollider();
        }
        return attackCol[(int)direction];
    }

    public void ChangeSizeX(DirType direction, int space)
    {
        changeCenterSize[(int)direction].x = oriCenterSize[(int)direction].x + ((oriCenterSize[(int)direction].x * 0.5f) * (space - 1));
        changeSize[(int)direction].x = oriSize[(int)direction].x * space;

        SetAttackSize((int)direction);
    }

    public void ChangeSizeX(int space)
    {
        for(int i = 0; i < (int)DirType.Size; i++)
        {
            changeCenterSize[i].x = oriCenterSize[i].x + ((oriCenterSize[i].x * 0.5f) * (space - 1));
            changeSize[i].x = oriSize[i].x * space;

            SetAttackSize(i);
        }
    }

    public void ChangeSizeZ(DirType direction, int space)
    {
        changeCenterSize[(int)direction].z = oriCenterSize[(int)direction].z + (oriCenterSize[(int)direction].z > 0 ? 1 : -1) * 0.5f * (space - 1);
        changeSize[(int)direction].z = oriSize[(int)direction].z * space;

        SetAttackSize((int)direction);
    }

    public void ChangeSizeZ(int space)
    {
        for (int i = 0; i < (int)DirType.Size; i++)
        {
            changeCenterSize[i].z = oriCenterSize[i].z + (oriCenterSize[i].z > 0 ? 1 : -1) * 0.5f * (space - 1);
            changeSize[i].z = oriSize[i].z * space;

            SetAttackSize(i);
        }
    }

    private void SetAttackSize(int direction)
    {
        attackCol[direction].center = changeCenterSize[direction];
        attackCol[direction].size = changeSize[direction];
    }

    // �ϳ��� ���⸸ �Ѿ� �� ��
    public void EnableDir(DirType dirType)
    {
        for(int i = 0; i < attackCol.Length; i++)
        {
            if (i == (int)dirType)
            {
                attackCol[i].enabled = true;
                break;
            }
        }
    }

    // �� ���� ���⸸ �Ѿ��ҋ�(ex)���� �����ʸ�)
    public void EnableDir(DirType dirType, DirType dirType2)
    {
        int count = 0;
        for (int i = 0; i < attackCol.Length; i++)
        {
            if (i == (int)dirType || i == (int)dirType2)
            {
                attackCol[i].enabled = true;
                count++;

                if (count == 2) break;
            }
        }
    }

    // ��� ������ �� �Ѿ��Ҷ�
    public void AllEnableDir()
    {
        for (int i = 0; i < attackCol.Length; i++)
        {
            attackCol[i].enabled = true;
        }
    }

    // ��� ������ �� �����Ҷ�
    public void AllDisableDir()
    {
        for (int i = 0; i < attackCol.Length; i++)
        {
            attackCol[i].enabled = false;
            attackRanges[i].EnemysClear();
        }
    }

    public GameObject CurrntDirNearEnemy()
    {
        float minDistnace = float.MaxValue;
        GameObject temp = null;
        for (int i = 0; i < (int)DirType.Size; i++)
        {
            if(attackCol[i].enabled)
            {
                if (attackRanges[i].NearEnemy().distance < minDistnace)
                {
                    temp = attackRanges[i].NearEnemy().obj;
                    minDistnace = attackRanges[i].NearEnemy().distance;
                }
            }
        }

        return temp;
    }
}
