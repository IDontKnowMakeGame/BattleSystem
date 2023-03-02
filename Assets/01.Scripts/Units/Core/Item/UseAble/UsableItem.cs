using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableItem
{
    [SerializeField]
    protected int itemCnt;

    // To Do DataManager 형태로 바꿀 것
    protected int ID;
    protected KeyCode key;

    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }

    public abstract void Use();
}
