using Actors.Characters;
using Actors.Characters.Player;
using Data;
using Acts.Characters.Player;
using UnityEngine;
using TMPro;

public class Weapon : EquiqmentItem
{
	public ItemInfo WeaponInfo
	{
		get
		{
			if (!isEnemy)
				return info + _weaponClassLevelInfo + _weaponLevelInfo + _weaponBuffInfo;
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
		if(!isEnemy)
		{
			_playerActor = _characterActor as PlayerActor;
			_playerAnimation = _playerActor.GetAct<PlayerAnimation>();
			Debug.Log(_playerAnimation);
			Debug.Log(_characterActor);
			LoadWeaponClassLevel();
			LoadWeaponLevel();
		}

		if (isEnemy)
			return;
	}

	public override void UnEquipment(CharacterActor actor)
	{
		if (isEnemy)
			return;
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
