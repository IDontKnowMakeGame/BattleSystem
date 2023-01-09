using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Manager;
using Unit;
public class Weapon
{
	public Unit.Unit _baseObject;
	protected int range;
	protected InputManager _inputManager;

	protected UnitMove _move;
	protected UnitAttack _attack;

	public Weapon(Unit.Unit unit)
	{
		SetBase(unit);
	}

	public Weapon()
	{

	}

	public virtual void Awake()
	{

	}

	public virtual void Start()
	{
		_inputManager = GameManagement.Instance.GetManager<InputManager>();
		_move = _baseObject.GetBehaviour<UnitMove>();
		_attack = _baseObject.GetBehaviour<UnitAttack>();
	}

	public virtual void Update()
	{

	}


	protected virtual void Move()
	{

	}

	protected virtual void Attack()
	{

	}

	protected virtual void Skii()
	{

	}

	public void SetBase(Unit.Unit _base)
	{
		_baseObject = _base;
	}
}
