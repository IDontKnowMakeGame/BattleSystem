using System;
using Behaviour = Unit.Behaviour;
using DG.Tweening;
using Unit.Player;
using Unit;
using Manager;

[Serializable]
public class PlayerWeapon : UnitWeapon
{
	protected InputManager _inputManager;

	public override void Start()
	{
		base.Start();
		_inputManager = GameManagement.Instance.GetManager<InputManager>();
		SetPlayer();
	}

	public override void Update()
	{
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.TestWeaponChange))
		{
			ChangeWeapon();
		}
		base.Update();
	}
	public void ChangeWeapon()
	{
		_currentSword = SwordType.TwinSword;
	}

	private void SetPlayer()
	{
		foreach(var v in weaponSkills)
		{
			v.Value._isEnemy = false;
		}
	}

	//public void UseSkill()
	//{
	//	weaponSkills[weapons.CurrentWeapon.type]?.Invoke();
	//}

	//public void Reset()
	//{
	//	isSkill = false;
	//	isCoolTime = false;

	//	_shotSwordStat.count = 0;
	//}

	//#region GreatSword
	//private void OnGreatSwordDash()
	//{
	//	if (isCoolTime)
	//		return;

	//	if (_inputManager.GetKeyInput(InputManager.InputSignal.Skill))
	//		isSkill = true;
	//	else if (_inputManager.GetKeyUpInput(InputManager.InputSignal.Skill))
	//		isSkill = false;

	//	if (!isSkill)
	//		return;

	//	if (_inputManager.GetKeyDownInput(InputManager.InputSignal.FowardAttack))
	//		GreatSwordDash(Vector3.forward);

	//	if (_inputManager.GetKeyDownInput(InputManager.InputSignal.BackwardAttack))
	//		GreatSwordDash(Vector3.back);

	//	if (_inputManager.GetKeyDownInput(InputManager.InputSignal.LeftAttack))
	//		GreatSwordDash(Vector3.left);

	//	if (_inputManager.GetKeyDownInput(InputManager.InputSignal.RightAttack))
	//		GreatSwordDash(Vector3.right);
	//}

	//private void GreatSwordDash(Vector3 direction)
	//{
	//	PlayerMove playerMove = thisBase.GetBehaviour<PlayerMove>();
	//	playerMove.onMoveEnd = GreatSwordAttack;
	//	_greatSwordStat.vec = direction;
	//	playerMove.Translate(direction);
	//}


	//private void GreatSwordAttack()
	//{
	//	PlayerAttack playerAttack = thisBase.GetBehaviour<PlayerAttack>();
	//	playerAttack.onAttackEnd = GreatSwordEnd;
	//	playerAttack.DoAttack(_greatSwordStat.vec);
	//}
	//private void GreatSwordEnd()
	//{
	//	isCoolTime = true;
	//	isSkill = false;
	//	_currentTime = 0;
	//	_maxTime = _greatSwordStat.CoolTime;
	//}

	//[Serializable]
	//struct GreatSwordStat
	//{
	//	[SerializeField]
	//	private float _dashBeforeWait;
	//	[SerializeField]
	//	private float _dashSpeed;

	//	[SerializeField]
	//	private float _coolTime;

	//	public float CoolTime => _coolTime;
	//	public float DashBeforeWait => _dashBeforeWait;
	//	public float DashSpeed => _dashSpeed;

	//	[HideInInspector]
	//	public Vector3 vec;
	//}
	//#endregion

	//#region RongSword
	//private void RollSkill(Vector3 vec)
	//{
	//	isCoolTime = true;
	//	Debug.Log("��ų ����");
	//	PlayerMove playerMove = thisBase.GetBehaviour<PlayerMove>();
	//	playerMove.onMoveEnd = RollSkillEnd;
	//	playerMove.Translate(vec * 2);
	//}

	//private void RollSkillEnd()
	//{
	//	isSkill = false;
	//	isCoolTime = false;
	//}

	//private void OnRollSkill()
	//{
	//	if (isCoolTime)
	//		return;

	//	if (_inputManager.GetKeyInput(InputManager.InputSignal.Skill))
	//		isSkill = true;
	//	else if (_inputManager.GetKeyUpInput(InputManager.InputSignal.Skill))
	//		isSkill = false;

	//	if (!isSkill)
	//		return;

	//	if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveForward))
	//		RollSkill(Vector3.forward);

	//	if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveBackward))
	//		RollSkill(Vector3.back);

	//	if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveLeft))
	//		RollSkill(Vector3.left);

	//	if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveRight))
	//		RollSkill(Vector3.right);
	//}
	//#endregion

	//#region ScotchSword
	//private void OnScotchSkill()
	//{
	//	if (isCoolTime)
	//		return;

	//	if (_inputManager.GetKeyInput(InputManager.InputSignal.Skill))
	//		isSkill = true;

	//	if (!isSkill)
	//		return;

	//	if (_inputManager.GetKeyDownInput(InputManager.InputSignal.FowardAttack))
	//		Scotch(Vector3.forward);

	//	if (_inputManager.GetKeyDownInput(InputManager.InputSignal.BackwardAttack))
	//		Scotch(Vector3.back);

	//	if (_inputManager.GetKeyDownInput(InputManager.InputSignal.LeftAttack))
	//		Scotch(Vector3.left);

	//	if (_inputManager.GetKeyDownInput(InputManager.InputSignal.RightAttack))
	//		Scotch(Vector3.right);
	//}

	//private void Scotch(Vector3 vec)
	//{
	//	if (_shotSwordStat.count == 6)
	//	{
	//		_shotSwordStat.count = 0;
	//		isSkill = false;

	//		_currentTime = 0;
	//		_maxTime = _shotSwordStat.CoolTime;
	//		return;
	//	}
	//	isSkill = true;
	//	isCoolTime = true;
	//	_shotSwordStat.vec = vec;
	//	PlayerAttack playerAttack = thisBase.GetBehaviour<PlayerAttack>();
	//	playerAttack.onAttackEnd = ScotchEnd;
	//	_shotSwordStat.count++;
	//	playerAttack.DoAttack(vec);
	//}

	//private void ScotchEnd()
	//{
	//	Scotch(_shotSwordStat.vec);
	//}

	//[Serializable]
	//private struct ShotSwordStat
	//{
	//	[SerializeField]
	//	private float _coolTime;

	//	public float CoolTime => _coolTime;

	//	[HideInInspector]
	//	public Vector3 vec;
	//	[HideInInspector]
	//	public int count;
	//}
	//#endregion
}
