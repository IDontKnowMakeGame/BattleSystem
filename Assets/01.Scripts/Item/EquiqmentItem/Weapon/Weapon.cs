using Actors.Bases;
using Actors.Characters;
using Actors.Characters.Enemy;
using Actors.Characters.Player;
using Data;
using Acts.Characters.Player;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditorInternal;
using UnityEngine;
using System;
using Managements;

public class Weapon : EquiqmentItem
{
	public ItemInfo WeaponInfo
	{
		get
		{
			if (!isEnemy)
				return info + _weaponClassLevelInfo + _weaponLevelInfo;
			else
				return info;
		}
	}

	#region Ÿ�̸� ���� ����
	protected bool _isCoolTime = false;
	protected float _currentTimerSecound = 0f;
	#endregion

	public bool isEnemy = true;
	protected CharacterActor _characterActor;
	protected PlayerActor _playerActor = null;
	protected PlayerAnimation _playerAnimation;

	public AttackInfo AttackInfo => _attackInfo;
	protected AttackInfo _attackInfo = new AttackInfo();

	protected ItemInfo _weaponClassLevelInfo = new ItemInfo();
	protected ItemInfo _weaponLevelInfo = new ItemInfo();

	protected EventParam _eventParam = new EventParam();

	public override void Equiqment(CharacterActor actor)
	{
		_characterActor = actor;
		if(!isEnemy)
		{
			_playerActor = _characterActor as PlayerActor;
			_playerAnimation = _playerActor.GetAct<PlayerAnimation>();
			LoadWeaponClassLevel();
			LoadWeaponLevel();
		}
	}

	/// <summary>
	/// ���� Ŭ���� ������ ���� �� ������ ���� ����� �Ѵ�.
	/// </summary>
	public virtual void LoadWeaponClassLevel()
	{

	}

	/// <summary>
	/// Weapono Level�� ���� ���� �� ���ָ� �� ���ϴ�.
	/// </summary>
	public virtual void LoadWeaponLevel()
	{

	}

	/// <summary>
	/// Weapon���� �⺻ ��ų
	/// </summary>
	public virtual void Skill()
	{
		if (_isCoolTime)
			return;
	}

	public override void Update()
	{
		Timer();
	}
	protected void Timer()
	{
		if (!_isCoolTime)
			return;

		if(_currentTimerSecound < info.CoolTime)
			_currentTimerSecound += Time.deltaTime;
		else
		{
			_isCoolTime = false;
			_currentTimerSecound = 0;
		}
	}
	protected int KillToLevel(int count) => count switch
	{
		<= 40 => 1,
		<= 50 => 2,
		<= 60 => 3,
		<= 70 => 4,
		<= 80 => 5,
		_ => 0
	};
}
