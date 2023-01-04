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

public class Pair<T, K>
{
	public Pair()
	{

	}

	public Pair(T first, K secound)
	{
		this.first = first;
		this.secound = secound;
	}

	public T first;
	public K secound;
}

[Serializable]
public class PlayerWeapon : Behaviour
{
	[UnityEngine.SerializeField] private WeaponSO weapon;
	private Dictionary<SwordType, Action> weaponSkills = new Dictionary<SwordType,Action>();

	[Header("그레이트 소드라고용")]
	[SerializeField]
	GreatSwordStat _greatSwordStat;

	[Space(6)]

	[SerializeField]
	ShotSwordStat _shotSwordStat;



	private float _currentTime;
	private InputManager _inputManager;

	public bool isSkill = false;
	private bool isCoolTime = false;
	public override void Awake()
	{
		weaponSkills.Add(SwordType.GreatSword, OnGreatSwordDash);
		weaponSkills.Add(SwordType.LongSword, OnRollSkill);
		weaponSkills.Add(SwordType.ShotSword, OnScotchSkill);
	}

	#region 기본 설정 해주기
	public override void Start()
	{
		Reset();
		_inputManager = GameManagement.Instance.GetManager<InputManager>();
	}

	public override void Update()
	{
		weaponSkills[weapon.type]?.Invoke();
	}

	public void SetWeapon(WeaponSO weapon)
	{
		this.weapon = weapon;
	}

	public void UseSkill()
	{
		weaponSkills[weapon.type]?.Invoke();
	}

	public void Reset()
	{
		isSkill = false;
		isCoolTime = false;

		_shotSwordStat.count = 0;
	}
	#endregion

	//Skills are added to the list in the order they are in the weaponSO

	#region 대검 스킬
	private void OnGreatSwordDash()
	{
		if (isCoolTime)
			return;

		if (_inputManager.GetKeyInput(InputManager.InputSignal.Skill))
			isSkill = true;

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

	}

	[Serializable]
	struct GreatSwordStat
	{
		[SerializeField]
		private float _dashBeforeWait;
		[SerializeField]
		private float _dashSpeed;

		public float DashBeforeWait => _dashBeforeWait;
		public float DashSpeed => _dashSpeed;

		public Vector3 vec;
	}
	#endregion

	#region 롱소드 스킬
	private void RollSkill(Vector3 vec)
	{
		isCoolTime = true;
		Debug.Log("스킬 시작");
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
		else if(_inputManager.GetKeyUpInput(InputManager.InputSignal.Skill))
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

	#region 단검 스킬
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

	private struct ShotSwordStat
	{
		public Vector3 vec;
		public int count;
	}
	#endregion
}
