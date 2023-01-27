using UnityEngine;
using Unit.Core.Weapon;
using Managements.Managers;
public class BaseGreatSword : Weapon
{
	protected float _chargeTime;
	protected float _maxChargeTime => WeaponStat.Ats;

	private Vector3 _currentVector;
	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		base.Update();
		Charge();
	}

	public override void ChangeKey()
	{
		base.ChangeKey();
		_inputManager.ChangeInGameKey(InputTarget.UpMove, KeyCode.UpArrow);
		_inputManager.ChangeInGameKey(InputTarget.DownMove, KeyCode.DownArrow);
		_inputManager.ChangeInGameKey(InputTarget.LeftMove, KeyCode.LeftArrow);
		_inputManager.ChangeInGameKey(InputTarget.RightMove, KeyCode.RightArrow);

		_inputManager.ChangeInGameKey(InputTarget.UpAttack, KeyCode.W);
		_inputManager.ChangeInGameKey(InputTarget.DownAttack, KeyCode.S);
		_inputManager.ChangeInGameKey(InputTarget.LeftAttack, KeyCode.A);
		_inputManager.ChangeInGameKey(InputTarget.RightAttack, KeyCode.D);

		_inputManager.AddInGameAction(InputTarget.UpMove, InputStatus.Press, Move);
		_inputManager.AddInGameAction(InputTarget.DownMove, InputStatus.Press, Move);
		_inputManager.AddInGameAction(InputTarget.LeftMove, InputStatus.Press, Move);
		_inputManager.AddInGameAction(InputTarget.RightMove, InputStatus.Press, Move);

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
	private void Move()
	{
		if (!_thisBase.State.HasFlag(Units.Base.Unit.BaseState.Skill))
		{
			_thisBase.RemoveState(Units.Base.Unit.BaseState.Charge);
			_chargeTime = 0;
		}
	}

	protected override void Attack(Vector3 vec)
	{
		if (_thisBase.State.HasFlag(Units.Base.Unit.BaseState.Charge))
			return;

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
			Debug.Log("?");
			_chargeTime = 0;
		}
		else
		{
			_chargeTime += Time.deltaTime;
		}
	}
}
