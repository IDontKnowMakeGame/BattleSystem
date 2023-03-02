using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableItem
{
    [SerializeField]
    protected int itemCnt;

    // To Do DataManager ���·� �ٲ� ��
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
