using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;

public abstract class UseableItem : Item
{
    protected int cnt;

    protected abstract void Use();
}
