using Actors.Characters;
using Actors.Characters.Player;
using Data;
using Acts.Characters.Player;
using UnityEngine;
using Core;
using System.Collections;
using Managements.Managers;
using Acts.Characters;

public class Weapon : EquiqmentItem
{
	public ItemInfo WeaponInfo
	{
		get
		{
			if (!isEnemy)
			{
				return info + _weaponClassLevelInfo + _weaponLevelInfo;
			}
			else
				return info;
		}
	}

	#region 타이머 관련 변수
	protected bool _isCoolTime
	{
		get
		{
			return _coolTime;
		}
		set
		{
			if(value == true && !_coolTime)
			{
				UIManager.Instance.InGame.FlagCoolTimePanel(info.CoolTime);
			}

			_coolTime = value;
		}
	}

	private bool _coolTime = false;
	protected float _currentTimerSecound = 0f;
	#endregion

	public bool isEnemy = true;
	protected bool _attakAble = true;
	protected bool _input = false;

	protected CharacterActor _characterActor;
	protected PlayerActor _playerActor = null;
	protected UnitAnimation _unitAnimation;
	protected PlayerAnimation _playerAnimation;
	protected CharacterStatAct _stat;

	public AttackInfo AttackInfo => _attackInfo;
	protected AttackInfo _attackInfo = new AttackInfo();

	protected ItemInfo _weaponClassLevelInfo = new ItemInfo();
	protected ItemInfo _weaponLevelInfo = new ItemInfo();

	protected EventParam _eventParam = new EventParam();

	public override void Equiqment(CharacterActor actor)
	{
		_characterActor = actor;
		_stat = _characterActor.GetAct<CharacterStatAct>();
		_unitAnimation = _characterActor.GetAct<UnitAnimation>();
		if (actor is PlayerActor)
		{
			_playerActor = _characterActor as PlayerActor;
			_playerAnimation = _unitAnimation as PlayerAnimation;
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
	public virtual void Skill(Vector3 vec)
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

	protected IEnumerator SameTimeInput()
	{
		STFirstSkill();
		yield return new WaitForSeconds(1f);
		SkillInputEnd(_characterActor.UUID,Vector3.zero);
	}

	protected virtual void STFirstSkill()
	{
		_input = true;
		InputManager<Weapon>.OnClickPress += STimeInputSkill;
		_characterActor.AddState(CharacterState.Skill);
	}

	protected virtual void STimeInputSkill(Vector3 vec)
	{

	}

	protected virtual void SkillInputEnd(int i, Vector3 vec)
	{
		_input = false;
		_isCoolTime = true;
		_characterActor.RemoveState(CharacterState.Skill);
		InputManager<Weapon>.OnClickPress -= STimeInputSkill;
		//InputManager<Weapon>.OnAttackPress -= STimeInputSkill;
	}

	protected void AtsTimer() => _characterActor.StartCoroutine(WaitAttack());

	private IEnumerator WaitAttack()
	{
		_attakAble = false;
		yield return new WaitForSeconds(info.Afs);
		_attakAble = true;
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

	public static Vector3 DirReturn(Vector3 vec)
	{
		//int i_width = Screen.width;
		//int i_height = Screen.height;

		//float heightDot = (float)i_height / 2;

		//Vector3 screen = new Vector3(i_width, i_height,0);
		//Vector3 screenDot = screen / 2;

		Vector3 angleDir = Camera.main.WorldToScreenPoint(InGame.Player.Position) - vec;

		float angle = Mathf.Atan2(angleDir.x, angleDir.y) * Mathf.Rad2Deg;

		//angle = angle < 0 ? Mathf.Abs(angle) + 180f : angle;
		//Debug.Log(angle);

		return AngleToDir(angle);
	}

	public static Vector3 AngleToDir(float angle)
	{
		if (angle > 145 || angle < -145)
			return Vector3.forward;
		else if (angle > 90 && angle < 145)
			return Vector3.left;
		else if (angle > -90 && angle < 90)
			return Vector3.back;
		else if (angle > -145 && angle < -90)
			return Vector3.right;

		return Vector3.zero;
	}
}
