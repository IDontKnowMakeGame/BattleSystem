using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
using Unit.Core.Weapon;
public class BaseBow : Weapon
{
	private bool _isCharge;
	protected float _chargeTime;
	protected float _maxChargeTime;

	private Vector3 _currentVector;


	public override void Start()
	{
		base.Start();
		if (!_isEnemy)
		{
			_inputManager.ChangeInGameKey(InputTarget.UpMove, KeyCode.UpArrow);
			_inputManager.ChangeInGameKey(InputTarget.DownMove, KeyCode.DownArrow);
			_inputManager.ChangeInGameKey(InputTarget.LeftMove, KeyCode.LeftArrow);
			_inputManager.ChangeInGameKey(InputTarget.RightMove, KeyCode.RightArrow);

			_inputManager.ChangeInGameKey(InputTarget.UpAttack, KeyCode.UpArrow);
			_inputManager.ChangeInGameKey(InputTarget.DownAttack, KeyCode.DownArrow);
			_inputManager.ChangeInGameKey(InputTarget.LeftAttack, KeyCode.LeftArrow);
			_inputManager.ChangeInGameKey(InputTarget.RightAttack, KeyCode.RightArrow);

			_inputManager.RemoveInGameAction(InputTarget.UpAttack, InputStatus.Press, () => Attack(Vector3.forward));
			_inputManager.RemoveInGameAction(InputTarget.DownAttack, InputStatus.Press, () => Attack(Vector3.back));
			_inputManager.RemoveInGameAction(InputTarget.LeftAttack, InputStatus.Press, () => Attack(Vector3.left));
			_inputManager.RemoveInGameAction(InputTarget.RightAttack, InputStatus.Press, () => Attack(Vector3.right));

			_inputManager.ChangeInGameAction(InputTarget.UpAttack, InputStatus.Hold, () => Attack(Vector3.forward));
			_inputManager.ChangeInGameAction(InputTarget.DownAttack, InputStatus.Hold, () => Attack(Vector3.back));
			_inputManager.ChangeInGameAction(InputTarget.LeftAttack, InputStatus.Hold, () => Attack(Vector3.left));
			_inputManager.ChangeInGameAction(InputTarget.RightAttack, InputStatus.Hold, () => Attack(Vector3.right));

			_inputManager.ChangeInGameAction(InputTarget.UpAttack, InputStatus.Release, () => AttackUP());
			_inputManager.ChangeInGameAction(InputTarget.DownAttack, InputStatus.Release, () => AttackUP());
			_inputManager.ChangeInGameAction(InputTarget.LeftAttack, InputStatus.Release, () => AttackUP());
			_inputManager.ChangeInGameAction(InputTarget.RightAttack, InputStatus.Release, () => AttackUP());
		}
	}

	public override void Update()
	{
		base.Update();
		Charge();
	}

	protected override void Move(Vector3 vec)
	{
		if (isSkill)
			return;
		if (_isCharge)
			return;

		_unitMove.Translate(vec);
	}

	protected override void Attack(Vector3 vec)
	{
		_isCharge = true;
		_currentVector = vec;
	}

	private void AttackUP()
	{
		_isCharge = false;
		_chargeTime = 0;
	}

	private void Charge()
	{
		if (!_isCharge)
			return;

		if (_chargeTime >= _maxChargeTime)
		{
			_isCharge = false;
			_unitAttack.Attack(_currentVector);
			_chargeTime = 0;
		}
		else
		{
			_chargeTime += Time.deltaTime;
		}
	}
}
