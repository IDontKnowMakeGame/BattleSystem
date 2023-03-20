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
