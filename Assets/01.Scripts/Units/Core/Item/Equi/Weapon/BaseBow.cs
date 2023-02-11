using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
using Unit.Core.Weapon;
using Core;

public class BaseBow : Weapon
{
	protected float _chargeTime;
	protected float _maxChargeTime => WeaponStat.Ats;

	protected string _arrowName;

	private Vector3 _currentVector;

	private float projectileSpeed = 10;

	public bool hasArrow = true;

	public override void Start()
	{
		base.Start();
		_arrowName = this.GetType().Name + "Arrow";
		Debug.Log(_arrowName);
	}
	public override void ChangeKey()
	{
		base.ChangeKey();
		_inputManager.ChangeInGameKey(InputTarget.UpAttack, KeyCode.W);
		_inputManager.ChangeInGameKey(InputTarget.DownAttack, KeyCode.S);
		_inputManager.ChangeInGameKey(InputTarget.LeftAttack, KeyCode.A);
		_inputManager.ChangeInGameKey(InputTarget.RightAttack, KeyCode.D);

		_inputManager.RemoveInGameAction(InputTarget.UpAttack, InputStatus.Press, UpAttack);
		_inputManager.RemoveInGameAction(InputTarget.DownAttack, InputStatus.Press, DownAttack);
		_inputManager.RemoveInGameAction(InputTarget.LeftAttack, InputStatus.Press, LeftAttack);
		_inputManager.RemoveInGameAction(InputTarget.RightAttack, InputStatus.Press, RightAttack);

		_inputManager.AddInGameAction(InputTarget.UpAttack, InputStatus.Hold, UpAttack);
		_inputManager.AddInGameAction(InputTarget.DownAttack, InputStatus.Hold, DownAttack);
		_inputManager.AddInGameAction(InputTarget.LeftAttack, InputStatus.Hold, LeftAttack);
		_inputManager.AddInGameAction(InputTarget.RightAttack, InputStatus.Hold, RightAttack);
	}
	public override void Update()
	{
		base.Update();
		Charge();
	}

	protected override void Attack(Vector3 vec)
	{
		if (!hasArrow)
			return;

		if (_thisBase.State.HasFlag(Units.Base.Unit.BaseState.Charge))
			return;

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
		}
		else
		{
			_chargeTime += Time.deltaTime;
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
}
