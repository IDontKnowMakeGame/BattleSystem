using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableItem
{
    [SerializeField]
    protected int itemCnt;

    public virtual void Update()
    {
        Use();
    }

    protected abstract void Use();
}
