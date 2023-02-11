using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
using Core;
using Unit.Core;

public class EquipmentItem : Item,IStatChange
{
    protected InputManager _inputManager;

    protected UnitStats _addstat = new UnitStats { Agi = 0, Atk = 0, Hp = 0 };
    protected UnitStats _multistat = new UnitStats { Agi = 1, Atk = 1, Hp = 1 };
    public UnitStats addstat { get => _addstat; set => value = null; }
    public UnitStats multistat { get => _multistat; set => value = null; }
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
