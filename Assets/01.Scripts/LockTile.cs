using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Flags]
public enum ChangeLockDir
{
    UP = 1 << 0,
    DOWN = 1 << 1,
    LEFT = 1 << 2,
    RIGHT = 1 << 3,
}

[RequireComponent(typeof(BlockBase))]
public class LockTile : MonoBehaviour
{
    [SerializeField] 
    private ChangeLockDir changeLockDir;
    private BlockBase blockBase;

    private void Start()
    {
        blockBase = GetComponent<BlockBase>();
    }

    public void CheckDir(Vector3 targetPos)
    {
        Vector3 distance = targetPos - transform.position.SetY(1);

        if ((changeLockDir.HasFlag(ChangeLockDir.UP) && distance == Vector3.forward)
            || (changeLockDir.HasFlag(ChangeLockDir.DOWN) && distance == Vector3.back)
            || (changeLockDir.HasFlag(ChangeLockDir.LEFT) && distance == Vector3.left)
            || (changeLockDir.HasFlag(ChangeLockDir.RIGHT) && distance == Vector3.right))
            blockBase.ChangeTile = false;    
    }
}
