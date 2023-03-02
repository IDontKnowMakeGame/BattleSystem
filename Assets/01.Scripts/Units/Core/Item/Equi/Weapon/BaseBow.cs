using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
using Unit.Core.Weapon;
using Core;
using Managements;

public class BaseBow : Weapon
{
	protected float _chargeTime;
	protected float _maxChargeTime => WeaponStat.Ats;

	protected string _arrowName;

	protected Vector3 _currentVector;

	private float projectileSpeed = 10;

	public bool hasArrow = true;

	public override void Start()
	{
		base.Start();
		_arrowName = this.GetType().Name + "Arrow";
		GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderFalse, new EventParam() { boolParam = false });
		LoadClassLevel("Bow");
		LevelSystem();
	}
	protected override void LevelSystem()
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

		if (_thisBase.State.HasFlag(Units.Base.Unit.BaseState.Charge))
			return;

		GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderFalse, new EventParam() { boolParam = true });
		_thisBase.AddState(Units.Base.Unit.BaseState.Charge);
		_thisBase.AddState(Units.Base.Unit.BaseState.StopMove);
		_currentVector = vec;

		LevelSystem();
	}
	private void Charge(Vector3 vec)
	{
		if (!_thisBase.State.HasFlag(Units.Base.Unit.BaseState.Charge))
			return;

		if (_chargeTime >= _maxChargeTime)
		{
			_thisBase.RemoveState(Units.Base.Unit.BaseState.Charge);
			_thisBase.RemoveState(Units.Base.Unit.BaseState.StopMove);
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
	private void Shooting(Vector3 vec)
	{
		GameObject obj = Managements.GameManagement.Instance.GetManager<ResourceManagers>().Instantiate("Arrow");
		BaseArrow arrow = obj.GetComponent<BaseArrow>();

		arrow.InitArrow(projectileSpeed,WeaponStat.Atk,
			_thisBase.Position + vec, vec, _arrowName);
		arrow.ShootArrow();

		hasArrow = false;
	}
	public override void Reset()
	{
		base.Reset();
		InputManager.OnAttackHold -= Charge;
	}
}
