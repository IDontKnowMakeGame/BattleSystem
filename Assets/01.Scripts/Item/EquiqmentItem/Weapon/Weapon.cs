using Actors.Characters;
using Actors.Characters.Player;
using Data;
using Acts.Characters.Player;
using UnityEngine;
using TMPro;
using Core;
using System;

public class Weapon : EquiqmentItem
{
	public Weapon DeepCopy()
	{
		Weapon other = (Weapon)this.MemberwiseClone();
		return other;
	}

	public ItemInfo WeaponInfo
	{
		get
		{
			if (!isEnemy)
			{
				return info + _weaponClassLevelInfo + _weaponLevelInfo + _weaponBuffInfo;
			}
			else
				return info;
		}
	}

	#region 타이머 관련 변수
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
	protected ItemInfo _weaponBuffInfo = new ItemInfo();

	protected EventParam _eventParam = new EventParam();

	public override void Equiqment(CharacterActor actor)
	{
		_characterActor = actor;
		if(actor is PlayerActor)
		{
			_playerActor = _characterActor as PlayerActor;
			_playerAnimation = _characterActor.GetAct<PlayerAnimation>();
			Debug.Log(_playerAnimation);
			LoadWeaponClassLevel();
			LoadWeaponLevel();
		}
	}

	public override void UnEquipment(CharacterActor actor)
	{

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
		int levelData = Define.GetManager<DataManager>().LoadWeaponLevelData(info.Id);
		switch (levelData)
		{
			case 1:
				_weaponLevelInfo.Atk = 20;
				break;
			case 2:
				_weaponLevelInfo.Atk = 45;
				break;
			case 3:
				_weaponLevelInfo.Atk = 75;
				break;
			case 4:
				_weaponLevelInfo.Atk = 110;
				break;
			case 5:
				_weaponLevelInfo.Atk = 150;
				break;
			case 6:
				_weaponLevelInfo.Atk = 195;
				break;
			case 7:
				_weaponLevelInfo.Atk = 245;
				break;
			case 8:
				_weaponLevelInfo.Atk = 300;
				break;
			case 9:
				_weaponLevelInfo.Atk = 360;
				break;
			case 10:
				_weaponLevelInfo.Atk = 425;
				break;
			case 11:
				_weaponLevelInfo.Atk = 495;
				break;
			case 12:
				_weaponLevelInfo.Atk = 570;
				break;
		}
	}

	/// <summary>
	/// Weapon들의 기본 스킬
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
		{
			_currentTimerSecound += Time.deltaTime;
		}
		else
		{
			_isCoolTime = false;
			_currentTimerSecound = 0;
		}
	}
	protected int KillToLevel(int count) => count switch
	{
		>= 40 and < 50  => 1,
		>= 50 and < 60 => 2,
		>= 60 and < 70 => 3,
		>= 70 and < 80 => 4,
		>= 80 => 5,
		_ => 0
	};
}
