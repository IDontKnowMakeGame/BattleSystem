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
	}
	public override void ChangeKey()
	{
		base.ChangeKey();
		_inputManager.ChangeInGameKey(InputTarget.UpAttack, KeyCode.W);
		_inputManager.ChangeInGameKey(InputTarget.DownAttack, KeyCode.S);
		_inputManager.ChangeInGameKey(InputTarget.LeftAttack, KeyCode.A);
		_inputManager.ChangeInGameKey(InputTarget.RightAttack, KeyCode.D);

		_inputManager.AddInGameAction(InputTarget.UpAttack, InputStatus.Hold, Charge);
		_inputManager.AddInGameAction(InputTarget.DownAttack, InputStatus.Hold, Charge);
		_inputManager.AddInGameAction(InputTarget.LeftAttack, InputStatus.Hold, Charge);
		_inputManager.AddInGameAction(InputTarget.RightAttack, InputStatus.Hold, Charge);

		GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderInit, new EventParam() { floatParam = _maxChargeTime });
		GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderFalse, new EventParam() { boolParam = false });
	}
	public override void Update()
	{
		base.Update();
	}

	protected override void UpAttack()
	{
		Attack(Vector3.forward);
	}

	protected override void DownAttack()
	{
		Attack(Vector3.back);
	}

	protected override void LeftAttack()
	{
		Attack(Vector3.left);
	}

	protected override void RightAttack()
	{
		Attack(Vector3.right);
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
	}

	private void Charge()
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

		arrow.InitArrow(projectileSpeed, _weaponStats.Atk,
			_thisBase.Position + vec,vec, _arrowName);
		arrow.ShootArrow();

		hasArrow = false;
	}

	public override void Reset()
	{
		base.Reset();
		_inputManager.RemoveInGameAction(InputTarget.UpAttack, InputStatus.Hold, Charge);
		_inputManager.RemoveInGameAction(InputTarget.DownAttack, InputStatus.Hold, Charge);
		_inputManager.RemoveInGameAction(InputTarget.LeftAttack, InputStatus.Hold, Charge);
		_inputManager.RemoveInGameAction(InputTarget.RightAttack, InputStatus.Hold, Charge);
	}
}
