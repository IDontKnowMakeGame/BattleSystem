using Actors.Bases;
using Actors.Characters;
using Actors.Characters.Enemy;
using Actors.Characters.Player;
using Data;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditorInternal;
using UnityEngine;

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

	public bool isEnemy;
	protected CharacterActor _characterActor;
	protected PlayerActor _playerActor = null;

	public AttackInfo AttackInfo => _attackInfo;
	protected AttackInfo _attackInfo;

	protected ItemInfo _weaponClassLevelInfo;
	protected ItemInfo _weaponLevelInfo;

	public override void Equiqment(CharacterActor actor)
	{
		_characterActor = actor;
		if(!isEnemy)
		{
			_playerActor = _characterActor as PlayerActor;
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
}
