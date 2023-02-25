using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;

public abstract class UserableItem : MonoBehaviour
{
    protected int cnt;

    protected abstract void Use();
}
