using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInput : MonoBehaviour
{
    public UnitInputAction InputActions { get; private set; }

    protected virtual void Awake()
    {
        InputActions = new UnitInputAction();
    }

    protected virtual void OnEnable()
    {
        InputActions.Enable();
    }

    protected virtual void OnDisable()
    {
        InputActions.Disable();
    }
}
