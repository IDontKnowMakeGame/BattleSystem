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
        for (int i = 0; i < 4; i++)
        {
            int check = 1 << i;

            DirType curDir = (DirType)check;
            if (attackInfo.WantDir.HasFlag(curDir))
            {
                ChangeSizeX(curDir, attackInfo.SizeX);
                ChangeSizeZ(curDir, attackInfo.SizeZ);
                ChangeOffsetX(curDir, attackInfo.OffsetX);
                ChangeOffsetZ(curDir, attackInfo.OffsetZ);
            }
            else
            {
                DeleteDir(curDir);
            }
        }
    }

    public void AllReset()
    {
        for (int i = (int)DirType.Up; i <= (int)DirType.Right; i++)
        {
            DirType curType = (DirType)i;
            attackCol[curType].enabled = true;
            attackRanges[(DirType)i].EnemysClear();
        }
    }

    // TO DO
    /*
    public EnemyController CurrntDirNearEnemy()
    {
        float minDistnace = float.MaxValue;
        EnemyController temp = null;
        for (int i = 0; i < 4; i++)
        {
            DirType type = (DirType)(1 << i);
            if (attackCol[type].enabled)
            {
                if (attackRanges[type].NearEnemy().distance < minDistnace)
                {
                    temp = attackRanges[type].NearEnemy().obj.GetComponent<EnemyController>();
                    minDistnace = attackRanges[type].NearEnemy().distance;
                }
            }
        }

        return temp;
    }
    */
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

    /*
    public BoxCollider GetAttackCol(int direction)
    {
        if (direction <= -1 && direction >= (int)DirType.Size)
        {
            Debug.LogError("Type의 범위가 넘어갔습니다.");
            return new BoxCollider();
        }
        return attackCol[(int)direction];
    }

        public void ChangeSizeX(int space)
    {
        for (int i = 0; i < (int)DirType.Size; i++)
        {
            changeCenterSize[i].x = oriCenterSize[i].x + ((oriCenterSize[i].x * 0.5f) * (space - 1));
            changeSize[i].x = oriSize[i].x * space;

            SetAttackSize(i);
        }
    }

    public void ChangeOffsetX(int space)
    {
        for (int i = 0; i < (int)DirType.Size; i++)
        {
            changeCenterSize[i].x = space;

            SetAttackSize(i);
        }
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

    public void ChangeOffsetZ(int space)
    {
        for (int i = 0; i < (int)DirType.Size; i++)
        {
            changeCenterSize[i].z = space;

            SetAttackSize(i);
        }
    }

    private void SetAttackSize(int direction)
    {
        attackCol[direction].center = changeCenterSize[direction];
        attackCol[direction].size = changeSize[direction];
    }

    // 두 개의 방향만 켜야할(ex)왼쪽 오른쪽만)
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

    // 모든 방향을 다 켜야할때
    public void AllEnableDir()
    {
        for (int i = 0; i < attackCol.Length; i++)
        {
            attackCol[i].enabled = true;
        }
    }

    // 모든 방향을 다 꺼야할때
    public void AllDisableDir()
    {
        for (int i = 0; i < attackCol.Length; i++)
        {
            attackCol[i].enabled = false;
        }
    }

    public void ChangeWeapon()
    {
        for (int i = 0; i < attackCol.Length; i++)
        {
            attackRanges[i].EnemysClear();
        }
    }

    public List<EnemyController> AllCurrentDirEnemy()
    {
        HashSet<EnemyController> currentEnemys = new HashSet<EnemyController>();

        for (int i = 0; i < (int)DirType.Size; i++)
        {
            if (attackCol[i].enabled)
            {
                List<GameObject> checkEnemy = attackRanges[i].AllEnemy();
                foreach (GameObject enemy in checkEnemy)
                {
                    EnemyController enemyBase = enemy.GetComponent<EnemyController>();
                    if (!currentEnemys.Contains(enemyBase))
                    {
                        currentEnemys.Add(enemyBase);
                    }
                }
            }
        }

        return currentEnemys.ToList();
    }

    
    public EnemyBase CurrntDirNearEnemy()
    {
        float minDistnace = float.MaxValue;
        EnemyBase temp = null;
        for (int i = 0; i < (int)DirType.Size; i++)
        {
            if (attackCol[i].enabled)
            {
                if (attackRanges[i].NearEnemy().distance < minDistnace)
                {
                    temp = attackRanges[i].NearEnemy().obj.GetComponent<EnemyBase>();
                    minDistnace = attackRanges[i].NearEnemy().distance;
                }
            }
        }

        return temp;
    }
    
    public DirType DirReturn(Vector3 vec)
    {
        if (vec == Vector3.forward)
            return DirType.Up;
        if (vec == Vector3.back)
            return DirType.Down;
        if (vec == Vector3.left)
            return DirType.Left;
        if (vec == Vector3.right)
            return DirType.Right;
        return DirType.Up;
    }
    */
}