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
		Debug.Log("greatSword");
		_inputManager.ChangeInGameKey(InputTarget.UpAttack, KeyCode.W);
		_inputManager.ChangeInGameKey(InputTarget.DownAttack, KeyCode.S);
		_inputManager.ChangeInGameKey(InputTarget.LeftAttack, KeyCode.A);
		_inputManager.ChangeInGameKey(InputTarget.RightAttack, KeyCode.D);

		_inputManager.AddInGameAction(InputTarget.UpMove, InputStatus.Press, Move);
		_inputManager.AddInGameAction(InputTarget.DownMove, InputStatus.Press, Move);
		_inputManager.AddInGameAction(InputTarget.LeftMove, InputStatus.Press, Move);
		_inputManager.AddInGameAction(InputTarget.RightMove, InputStatus.Press, Move);

		_inputManager.RemoveInGameAction(InputTarget.UpAttack, InputStatus.Press, UpAttack);
		_inputManager.RemoveInGameAction(InputTarget.DownAttack, InputStatus.Press, DownAttack);
		_inputManager.RemoveInGameAction(InputTarget.LeftAttack, InputStatus.Press, LeftAttack);
		_inputManager.RemoveInGameAction(InputTarget.RightAttack, InputStatus.Press, RightAttack);

		_inputManager.AddInGameAction(InputTarget.UpAttack, InputStatus.Hold, UpAttack);
		_inputManager.AddInGameAction(InputTarget.DownAttack, InputStatus.Hold, DownAttack);
		_inputManager.AddInGameAction(InputTarget.LeftAttack, InputStatus.Hold, LeftAttack);
		_inputManager.AddInGameAction(InputTarget.RightAttack, InputStatus.Hold, RightAttack);

		_inputManager.AddInGameAction(InputTarget.UpAttack, InputStatus.Release, AttackUP);
		_inputManager.AddInGameAction(InputTarget.DownAttack, InputStatus.Release, AttackUP);
		_inputManager.AddInGameAction(InputTarget.LeftAttack, InputStatus.Release, AttackUP);
		_inputManager.AddInGameAction(InputTarget.RightAttack, InputStatus.Release, AttackUP);
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
		base.Attack(vec);
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
			_playerAttack.AttackColParent.AllDisableDir();
			_playerAttack.AttackColParent.ChangeSizeZ(1);
			_playerAttack.AttackColParent.ChangeSizeX(1);
			_playerAttack.AttackColParent.EnableDir(_playerAttack.AttackColParent.DirReturn(_currentVector));
			_playerAttack.Attack(_unitStat.NowStats.Atk);
			_playerAttack.AttackColParent.ChangeWeapon();
			_playerAttack.AttackColParent.AllEnableDir();
			_chargeTime = 0;
		}
		else
		{
			_chargeTime += Time.deltaTime;
		}
	}

	public override void Reset()
	{
		Debug.Log("greatSword");
		_chargeTime = 0;
		_thisBase.RemoveState(Units.Base.Unit.BaseState.Charge);
		_currentVector = Vector3.zero;

		_inputManager.RemoveInGameAction(InputTarget.UpAttack, InputStatus.Hold, UpAttack);
		_inputManager.RemoveInGameAction(InputTarget.DownAttack, InputStatus.Hold, DownAttack);
		_inputManager.RemoveInGameAction(InputTarget.LeftAttack, InputStatus.Hold, LeftAttack);
		_inputManager.RemoveInGameAction(InputTarget.RightAttack, InputStatus.Hold, RightAttack);

		_inputManager.RemoveInGameAction(InputTarget.UpAttack, InputStatus.Release, AttackUP);
		_inputManager.RemoveInGameAction(InputTarget.DownAttack, InputStatus.Release, AttackUP);
		_inputManager.RemoveInGameAction(InputTarget.LeftAttack, InputStatus.Release, AttackUP);
		_inputManager.RemoveInGameAction(InputTarget.RightAttack, InputStatus.Release, AttackUP);
	}
}
