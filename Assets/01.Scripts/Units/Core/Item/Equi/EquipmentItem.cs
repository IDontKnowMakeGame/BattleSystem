using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
using Core;
public class EquipmentItem : Item
{
    protected InputManager _inputManager;
    public virtual void Awake()
	{
        _inputManager  = Define.GetManager<InputManager>();
    }
    public virtual void Start()
    {
        
    }

    public virtual void Update()
    {
        
    }
}
