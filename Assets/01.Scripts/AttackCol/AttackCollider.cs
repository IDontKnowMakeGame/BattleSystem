using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Flags]
public enum DirType
{
    None = 0,

    Up = 1 << 0,
    Down = 1 << 1,
    Left = 1 << 2,
    Right = 1 << 3,

    All = int.MaxValue
}
public class AttackCollider : MonoBehaviour
{
    private Dictionary<DirType, BoxCollider> attackCol = new Dictionary<DirType, BoxCollider>();
    private Dictionary<DirType, AttackRange> attackRanges = new Dictionary<DirType, AttackRange>();

    private Dictionary<DirType, Vector3> oriCenterSize = new Dictionary<DirType, Vector3>();
    private Dictionary<DirType, Vector3> oriSize = new Dictionary<DirType, Vector3>();

    private Dictionary<DirType, Vector3> changeCenterSize = new Dictionary<DirType, Vector3>();
    private Dictionary<DirType, Vector3> changeSize = new Dictionary<DirType, Vector3>();

    const int none = -987654321;

    AttackInfo currentInfo;

    private void Start()
    {
        // Attack 콜라이더 넣기(U-D-L-R)
        for (int i = 0; i < 4; i++)
        {
            int check = 1 << i;
            DirType curType = (DirType)check;

            attackCol[curType] = transform.GetChild(i).GetComponent<BoxCollider>();
            attackRanges[curType] = transform.GetChild(i).GetComponent<AttackRange>();
            oriCenterSize[curType] = changeCenterSize[curType] = attackCol[curType].center;
            oriSize[curType] = changeSize[curType] = attackCol[curType].size;
        }

        /*        // Test Code
                AttackInfo attackInfo = new AttackInfo();

                attackInfo.SizeZ = 2;
                attackInfo.AddDir(DirType.Up);

                SetAttackCol(attackInfo);*/
    }


    public void SetAttackCol(AttackInfo attackInfo)
    {
        currentInfo = attackInfo;

        for (int i = 0; i < 4; i++)
        {
            int check = 1 << i;

            DirType curDir = (DirType)check;
            ChangeSizeX(curDir, currentInfo.SizeX);
            ChangeSizeZ(curDir, currentInfo.SizeZ);
            ChangeOffsetX(curDir, currentInfo.OffsetX);
            ChangeOffsetZ(curDir, currentInfo.OffsetZ);
        }
    }

    public void AllReset()
    {
        for (int i = 0; i < 4; i++)
        {
            DirType curType = (DirType)(1 << i);
            attackCol[curType].enabled = true;
            //attackRanges[curType].EnemysClear();
        }
    }

    public void AllDisable()
    {
        for (int i = 0; i < 4; i++)
        {
            DirType curType = (DirType)(1 << i);
            attackCol[curType].enabled = false;
            attackRanges[curType].EnemysClear();
        }
    }

    // TO DO
    public SampleControoler CurrntDirNearEnemy()
    {
        float minDistnace = float.MaxValue;
        SampleControoler temp = null;

        if(currentInfo == null)
        {
            Debug.LogError("CurrentInfo Is Null.");
        }

        for (int i = 0; i < 4; i++)
        {
            DirType type = (DirType)(1 << i);
            if (currentInfo.WantDir.HasFlag(type))
            {
                if (attackRanges[type].NearEnemy().distance < minDistnace)
                {
                    temp = attackRanges[type].NearEnemy().obj.GetComponent<SampleControoler>();
                    minDistnace = attackRanges[type].NearEnemy().distance;
                }
            }
        }
        return temp;
    }

    #region Private Method.
    private void SetAttackSize(DirType direction)
    {
        attackCol[direction].center = changeCenterSize[direction];
        attackCol[direction].size = changeSize[direction];
    }

    private void ChangeSizeX(DirType direction, int space)
    {
        if (space == none) return;

        changeCenterSize[direction] = changeCenterSize[direction].SetX(oriCenterSize[direction].x + ((oriCenterSize[direction].x * 0.5f)) * (space - 1));
        changeSize[direction] = changeSize[direction].SetX(oriSize[direction].x * space);

        SetAttackSize(direction);
    }

    private void ChangeOffsetX(DirType direction, float space)
    {
        if (space == none) return;

        changeCenterSize[direction] = changeCenterSize[direction].SetX(space);

        SetAttackSize(direction);
    }

    private void ChangeSizeZ(DirType direction, float space)
    {
        if (space == none) return;

        changeCenterSize[direction] = changeCenterSize[direction].SetZ(oriCenterSize[direction].z + (oriCenterSize[direction].z > 0 ? 1 : -1) * 0.5f * (space - 1));
        changeSize[direction] = changeSize[direction].SetZ(oriSize[direction].z * space);

        SetAttackSize(direction);
    }

    private void ChangeOffsetZ(DirType direction, float space)
    {
        if (space == none) return;

        changeCenterSize[direction] = changeCenterSize[direction].SetZ(space);

        SetAttackSize(direction);
    }

    private void DeleteDir(DirType dirType)
    {
        attackCol[dirType].enabled = false;
        attackRanges[dirType].EnemysClear();
    }
    #endregion
}