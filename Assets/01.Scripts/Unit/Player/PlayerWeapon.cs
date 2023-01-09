using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviour = Unit.Behaviour;
using DG.Tweening;
using Unit.Player;
using Manager;
public enum SwordType
{
	LongSword,
	GreatSword,
	ShotSword
}

[Serializable]
public class PlayerWeapon : Behaviour
{
	[UnityEngine.SerializeField] private WeaponContainer weapons;

	private Dictionary<SwordType, Action> weaponSkills = new Dictionary<SwordType, Action>();

	[Header("GreatSword")]
	[SerializeField]
	GreatSwordStat _greatSwordStat;

	[Header("ShotSword")]
	[SerializeField]
	ShotSwordStat _shotSwordStat;

	#region Basic
	private float _currentTime;
	private float _maxTime;
	private InputManager _inputManager;

	public bool isSkill = false;
	private bool isCoolTime = false;
	#endregion
	public override void Awake()
	{
		weaponSkills.Add(SwordType.GreatSword, OnGreatSwordDash);
		weaponSkills.Add(SwordType.LongSword, OnRollSkill);
		weaponSkills.Add(SwordType.ShotSword, OnScotchSkill);
	}

	#region Basic Setting
	public override void Start()
	{
		Reset();
		_inputManager = GameManagement.Instance.GetManager<InputManager>();
	}

	public override void Update()
	{
		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.TestWeaponChange))
		{
			Reset();
			weapons.ChangeWeapon();
		}

		weaponSkills[weapons.CurrentWeapon.type]?.Invoke();

		Timer();
	}

	public void UseSkill()
	{
		weaponSkills[weapons.CurrentWeapon.type]?.Invoke();
	}

	public void Reset()
	{
		isSkill = false;
		isCoolTime = false;

		_shotSwordStat.count = 0;
	}
	#endregion

	/// <summary>
	/// CoolTimer
	/// </summary>
	private void Timer()
	{
		if(_currentTime < _maxTime && isCoolTime)
		{
			_currentTime += Time.deltaTime;
		}
		else
		{
			isCoolTime = false;
			_currentTime = 0;
		}
	}

	#region GreatSword
	private void OnGreatSwordDash()
	{
		if (isCoolTime)
			return;

		if (_inputManager.GetKeyInput(InputManager.InputSignal.Skill))
			isSkill = true;
		else if (_inputManager.GetKeyUpInput(InputManager.InputSignal.Skill))
			isSkill = false;

		if (!isSkill)
			return;

		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.FowardAttack))
			GreatSwordDash(Vector3.forward);

		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.BackwardAttack))
			GreatSwordDash(Vector3.back);

		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.LeftAttack))
			GreatSwordDash(Vector3.left);

		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.RightAttack))
			GreatSwordDash(Vector3.right);
	}

	private void GreatSwordDash(Vector3 direction)
	{
		PlayerMove playerMove = thisBase.GetBehaviour<PlayerMove>();
		playerMove.onMoveEnd = GreatSwordAttack;
		_greatSwordStat.vec = direction;
		playerMove.Translation(direction);
	}


	private void GreatSwordAttack()
	{
		PlayerAttack playerAttack = thisBase.GetBehaviour<PlayerAttack>();
		playerAttack.onAttackEnd = GreatSwordEnd;
		playerAttack.DoAttack(_greatSwordStat.vec);
	}
	private void GreatSwordEnd()
	{
		isCoolTime = true;
		isSkill = false;
		_currentTime = 0;
		_maxTime = _greatSwordStat.CoolTime;
	}

	[Serializable]
	struct GreatSwordStat
	{
		[SerializeField]
		private float _dashBeforeWait;
		[SerializeField]
		private float _dashSpeed;

		[SerializeField]
		private float _coolTime;

		public float CoolTime => _coolTime;
		public float DashBeforeWait => _dashBeforeWait;
		public float DashSpeed => _dashSpeed;

		[HideInInspector]
		public Vector3 vec;
	}
	#endregion

	#region RongSword
	private void RollSkill(Vector3 vec)
	{
		isCoolTime = true;
		Debug.Log("��ų ����");
		PlayerMove playerMove = thisBase.GetBehaviour<PlayerMove>();
		playerMove.onMoveEnd = RollSkillEnd;
		playerMove.Translation(vec * 2);
	}

	private void RollSkillEnd()
	{
		isSkill = false;
		isCoolTime = false;
	}

	private void OnRollSkill()
	{
		if (isCoolTime)
			return;

		if (_inputManager.GetKeyInput(InputManager.InputSignal.Skill))
			isSkill = true;
		else if (_inputManager.GetKeyUpInput(InputManager.InputSignal.Skill))
			isSkill = false;

		if (!isSkill)
			return;

		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveForward))
			RollSkill(Vector3.forward);

		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveBackward))
			RollSkill(Vector3.back);

		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveLeft))
			RollSkill(Vector3.left);

		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveRight))
			RollSkill(Vector3.right);
	}
	#endregion

	#region ScotchSword
	private void OnScotchSkill()
	{
		if (isCoolTime)
			return;

		if (_inputManager.GetKeyInput(InputManager.InputSignal.Skill))
			isSkill = true;

		if (!isSkill)
			return;

		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.FowardAttack))
			Scotch(Vector3.forward);

		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.BackwardAttack))
			Scotch(Vector3.back);

		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.LeftAttack))
			Scotch(Vector3.left);

		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.RightAttack))
			Scotch(Vector3.right);
	}

	private void Scotch(Vector3 vec)
	{
		if (_shotSwordStat.count == 6)
		{
			_shotSwordStat.count = 0;
			isSkill = false;

			_currentTime = 0;
			_maxTime = _shotSwordStat.CoolTime;
			return;
		}
		isSkill = true;
		isCoolTime = true;
		_shotSwordStat.vec = vec;
		PlayerAttack playerAttack = thisBase.GetBehaviour<PlayerAttack>();
		playerAttack.onAttackEnd = ScotchEnd;
		_shotSwordStat.count++;
		playerAttack.DoAttack(vec);
	}

	private void ScotchEnd()
	{
		Scotch(_shotSwordStat.vec);
	}

	[Serializable]
	private struct ShotSwordStat
	{
		[SerializeField]
		private float _coolTime;

		public float CoolTime => _coolTime;

		[HideInInspector]
		public Vector3 vec;
		[HideInInspector]
		public int count;
	}
	#endregion
}
