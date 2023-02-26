using System;
using UnityEngine;
using Units.Behaviours.Unit;
using Units.Base.Unit;
using Managements;
using Managements.Managers;
using Units.Base.Player;
using Core;
using Tools;

namespace Unit.Core.Weapon
{
	public enum WeaponType
	{
		OldStraightSword,
		OldGreatSword,
		OldTwinSword,
		OldSpear,
		TaintedSword,
		End
	}

    [Serializable]
    public class Weapon:EquipmentItem
    {
        public UnitBase _thisBase;

        protected WeaponStats _weaponStats = null;
        protected WeaponStats _changeWeaponStats = new WeaponStats();
        protected WeaponStats _WeaponStats = new WeaponStats();

		protected AttackCollider _attackCollider = null;
		public WeaponStats WeaponStat
		{
			get
			{
				_WeaponStats.Atk = _weaponStats.Atk + _changeWeaponStats.Atk;
				_WeaponStats.Ats = _weaponStats.Ats + _changeWeaponStats.Ats;
				_WeaponStats.Afs = _weaponStats.Afs + _changeWeaponStats.Afs;
				_WeaponStats.Weight = _weaponStats.Weight + _changeWeaponStats.Weight;
				return _WeaponStats;
			}
		}

		//유닛 공격, 유닛 move
		protected UnitAttack _unitAttack;
		protected UnitMove _unitMove;
		protected UnitStat _unitStat;
		protected UnitAnimation _unitAnimation;

		protected PlayerAnimation _playerAnimation;
		protected PlayerAttack _playerAttack;

        protected float _currentTime;
        protected float _maxTime;

        protected bool _isCoolTime = false;

        public bool isSkill = false;

		protected bool _isEnemy = true;

		protected SliderObject _sliderObject;

		protected WeaponClassLevel _weaponClassLevel;
		public override void Start()
		{
			//여기서 다 받아주고
			_unitAttack = _thisBase.GetBehaviour<UnitAttack>();
			_unitMove = _thisBase.GetBehaviour<UnitMove>();
			_unitStat = _thisBase.GetBehaviour<UnitStat>();
			_unitAnimation = _thisBase.GetBehaviour<UnitAnimation>();

			_playerAttack = _unitAttack as PlayerAttack;
			_playerAnimation = _unitAnimation as PlayerAnimation;
		}
		public override void Update()
		{
			Timer();
		}

		public virtual void ChangeKey()
		{
			InputManager.OnAttackPress += AttackCoroutine;
			InputManager.OnSkillPress += Skill;
		}

		protected void Timer()
		{
			if (_currentTime < _maxTime && _isCoolTime)
			{
				_currentTime += Time.deltaTime;
			}
			else
			{
				_isCoolTime = false;
				_currentTime = 0;
			}
		}

		protected void GetWeaponStateData(string name)
		{
			WeaponStateDataList weaponStateDataList = DataJson.LoadJsonFile<WeaponStateDataList>(Application.streamingAssetsPath + "/SAVE/Weapon", "WeaponStatus");
			foreach (WeaponStateData data in weaponStateDataList.weaponList)
			{
				if (data.name == name)
				{
					_weaponStats = WeaponSerializable(data);
					break;
				}
			}
		}
		public WeaponStats WeaponSerializable(WeaponStateData data)
		{
			WeaponStats state = new WeaponStats();

			state.Afs = data.attackAfterDelay;
			state.Atk = data.damage;
			state.Ats = data.attackSpeed;
			state.Weight = data.weaponWeight;

			return state;
		}

		protected virtual void Attack(Vector3 vec)
		{
			
		}

		protected virtual void AttackCoroutine(Vector3 vec)
		{
			if (_thisBase.GetBehaviour<PlayerItem>().PlayerShield.UseAble) return;


			if (_thisBase.State.HasFlag(BaseState.Attacking) ||
				!_thisBase.GetBehaviour<PlayerAnimation>().CurWeaponAnimator.LastChange || _thisBase.State.HasFlag(BaseState.Moving))
				return;
			if (_thisBase.State.HasFlag(BaseState.Moving))
				return;

			if (_thisBase.GetBehaviour<PlayerEqiq>().WeaponAnimation() != 1 && _thisBase.GetBehaviour<PlayerEqiq>().WeaponAnimation() != 3 &&
				_thisBase.GetBehaviour<PlayerAnimation>().CurWeaponAnimator.LastChange)
				_thisBase.GetBehaviour<PlayerMove>().stop = true;

			_playerAnimation.CurWeaponAnimator.SetDir = vec;
			_playerAnimation.CurWeaponAnimator.Attack = true;
			_playerAnimation.SetAnmation();
			AnimeClip animeClip = _playerAnimation.GetClip();
			_playerAnimation.GetClip().SetEventOnFrame(5, () => Attack(vec));
			_thisBase.AddState(BaseState.Attacking);
		}

		protected virtual void Skill()
		{

		}
		public virtual void LoadClassLevel(string name)
		{
			_weaponClassLevel = Define.GetManager<DataManager>().LoadWeaponClassLevel(name);
		}
		protected int CountToLevel(int count) => count switch
		{
			<= 40 => 1,
			<= 50 => 2,
			<= 60 => 3,
			<= 70 => 4,
			<= 80 => 5,
			_ => 1
		};
		protected virtual void LevelSystem()
		{

		}
		public virtual void Reset()
		{
			InputManager.OnAttackPress -= AttackCoroutine;
			InputManager.OnSkillPress -= Skill;
		}
	}
}