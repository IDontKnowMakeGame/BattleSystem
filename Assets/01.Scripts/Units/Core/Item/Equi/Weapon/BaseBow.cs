using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
using Unit.Core.Weapon;
public class BaseBow : Weapon
{
	protected float _chargeTime;
	protected float _maxChargeTime;

	private Vector3 _currentVector;

	public override void ChangeKey()
	{
		base.ChangeKey();
		_inputManager.ChangeInGameKey(InputTarget.UpMove, KeyCode.UpArrow);
		_inputManager.ChangeInGameKey(InputTarget.DownMove, KeyCode.DownArrow);
		_inputManager.ChangeInGameKey(InputTarget.LeftMove, KeyCode.LeftArrow);
		_inputManager.ChangeInGameKey(InputTarget.RightMove, KeyCode.RightArrow);

		_inputManager.ChangeInGameKey(InputTarget.UpAttack, KeyCode.UpArrow);
		_inputManager.ChangeInGameKey(InputTarget.DownAttack, KeyCode.DownArrow);
		_inputManager.ChangeInGameKey(InputTarget.LeftAttack, KeyCode.LeftArrow);
		_inputManager.ChangeInGameKey(InputTarget.RightAttack, KeyCode.RightArrow);

		_inputManager.ChangeInGameAction(InputTarget.UpAttack, InputStatus.Hold, () => Attack(Vector3.forward));
		_inputManager.ChangeInGameAction(InputTarget.DownAttack, InputStatus.Hold, () => Attack(Vector3.back));
		_inputManager.ChangeInGameAction(InputTarget.LeftAttack, InputStatus.Hold, () => Attack(Vector3.left));
		_inputManager.ChangeInGameAction(InputTarget.RightAttack, InputStatus.Hold, () => Attack(Vector3.right));

		_inputManager.ChangeInGameAction(InputTarget.UpAttack, InputStatus.Release, () => AttackUP());
		_inputManager.ChangeInGameAction(InputTarget.DownAttack, InputStatus.Release, () => AttackUP());
		_inputManager.ChangeInGameAction(InputTarget.LeftAttack, InputStatus.Release, () => AttackUP());
		_inputManager.ChangeInGameAction(InputTarget.RightAttack, InputStatus.Release, () => AttackUP());
	}
	public override void Update()
	{
		base.Update();
		Charge();
	}

	protected override void Attack(Vector3 vec)
	{
		_thisBase.AddState(Units.Base.Unit.BaseState.Charge);
		_currentVector = vec;
	}

	private void AttackUP()
	{
		_thisBase.RemoveState(Units.Base.Unit.BaseState.Charge);
		_chargeTime = 0;
	}

	private void Charge()
	{
		if (!_thisBase.State.HasFlag(Units.Base.Unit.BaseState.Charge))
			return;

		if (_chargeTime >= _maxChargeTime)
		{
			_thisBase.RemoveState(Units.Base.Unit.BaseState.Charge);
			_unitAttack.Attack(_currentVector);
			_chargeTime = 0;
		}
		else
		{
			_chargeTime += Time.deltaTime;
		}
	}
}
