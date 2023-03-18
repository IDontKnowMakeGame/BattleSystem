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
	/// 웨폰 클래스 레벨이 변경 될 때마다 실행 해줘야 한다.
	/// </summary>
	public virtual void LoadWeaponClassLevel()
	{

	}

	/// <summary>
	/// Weapono Level을 변경 해줄 때 해주면 될 듯하다.
	/// </summary>
	public virtual void LoadWeaponLevel()
	{

	}
}
