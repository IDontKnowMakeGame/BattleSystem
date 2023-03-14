using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
using Core;
using Unit.Core;

public class EquipmentItem : Item
{
    protected InputManager _inputManager;
    public override void Awake()
	{
        _inputManager  = Define.GetManager<InputManager>();
    }
    public override void Start()
    {
        
    }

    public override void Update()
    {
        
    }
}
