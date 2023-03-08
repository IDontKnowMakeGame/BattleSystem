using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
using Unit.Core.Weapon;
using Core;
using Managements;
using Units.Base.Unit;
using Units.Base.Player;
using Tools;

public class BaseBow : Weapon
{
	protected float _chargeTime;
	protected float _maxChargeTime => WeaponStat.Ats;

	[SerializeField]
	protected ArrowType _arrowName;

	protected Vector3 _currentVector;

	private float projectileSpeed = 10;
	private Units.Base.Units _unit = null;

	public bool hasArrow = true;

	public override void Start()
	{
		base.Start();
		GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderFalse, new EventParam() { boolParam = false });
		LoadClassLevel("Bow");
		LevelSystem();
	}
	public override void LevelSystem()
	{
		int level = CountToLevel(_weaponClassLevel.killedCount);

		float basicValue = 10;
		switch (level)
		{
			case 1:
				_changeBuffStats.Atk = 5;
				projectileSpeed = basicValue + 0.5f;
				break;
			case 2:
				_changeBuffStats.Atk = 10;
				projectileSpeed = basicValue + 0.7f;
				break;
			case 3:
				_changeBuffStats.Atk = 15;
				projectileSpeed = basicValue + 1f;
				break;
			case 4:
				_changeBuffStats.Atk = 20;
				projectileSpeed = basicValue + 1.2f;
				break;
			case 5:
				_changeBuffStats.Atk = 20;
				projectileSpeed = basicValue + 2f;
				break;
		};
	}
	public override void ChangeKey()
	{
		base.ChangeKey();
		InputManager.ChangeKeyCode(KeyboardInput.AttackForward, KeyCode.W);
		InputManager.ChangeKeyCode(KeyboardInput.AttackBackward, KeyCode.S);
		InputManager.ChangeKeyCode(KeyboardInput.AttackLeft, KeyCode.A);
		InputManager.ChangeKeyCode(KeyboardInput.AttackRight, KeyCode.D);

		GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderInit, new EventParam() { floatParam = _maxChargeTime });
		GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderFalse, new EventParam() { boolParam = false });
	}
	public override void Update()
	{
		base.Update();
		Charge(_currentVector);
	}

	protected override void AttackCoroutine(Vector3 vec)
	{
		Attack(vec);
	}
	protected override void Attack(Vector3 vec)
	{
		if (!hasArrow)
			return;

		if (thisBase.State.HasFlag(Units.Base.Unit.BaseState.Charge))
			return;

		GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderFalse, new EventParam() { boolParam = true });
		thisBase.AddState(Units.Base.Unit.BaseState.Charge);
		thisBase.AddState(Units.Base.Unit.BaseState.StopMove);
		_currentVector = vec;

		LevelSystem();
	}
	private void Charge(Vector3 vec)
	{
		if (!thisBase.State.HasFlag(Units.Base.Unit.BaseState.Charge))
			return;

		if (_chargeTime >= _maxChargeTime)
		{
			thisBase.RemoveState(Units.Base.Unit.BaseState.Charge);
			thisBase.RemoveState(Units.Base.Unit.BaseState.StopMove);
			Shooting(_currentVector);
			_chargeTime = 0;

			GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderUp, new EventParam() { floatParam = _chargeTime });
			GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderFalse, new EventParam() { boolParam = false });
		}
		else
		{
			_chargeTime += Time.deltaTime;
			GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderUp, new EventParam() { floatParam = _chargeTime });
		}
	}
	public void Shoot(Vector3 vec) => Shooting(vec);
	private void Shooting(Vector3 vec)
	{
		GameObject obj = Managements.GameManagement.Instance.GetManager<ResourceManagers>().Instantiate("Arrow");
		BaseArrow arrow = obj.GetComponent<BaseArrow>();

		arrow.InitArrow(projectileSpeed,WeaponStat.Atk,
			thisBase.Position + vec, vec, _arrowName, thisBase);
		arrow.ShootArrow();

		hasArrow = false;
	}
}
