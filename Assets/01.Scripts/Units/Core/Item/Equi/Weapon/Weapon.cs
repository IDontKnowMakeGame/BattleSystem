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
    [Serializable]
    public class Weapon:EquipmentItem
    {
        public UnitBase _thisBase;

        protected WeaponStats _weaponStats = null;
        protected WeaponStats _changeStats = new WeaponStats();
        protected WeaponStats _changeBuffStats = new WeaponStats();

        private WeaponStats _WeaponStats = new WeaponStats();

		protected AttackCollider _attackCollider = null;
		public WeaponStats WeaponStat
		{
			get
			{
				_WeaponStats.Atk = _changeStats.Atk + _changeBuffStats.Atk;
				_WeaponStats.Ats = _changeStats.Ats + _changeBuffStats.Ats;
				_WeaponStats.Afs = _changeStats.Afs + _changeBuffStats.Afs;
				_WeaponStats.Weight = _changeStats.Weight + _changeBuffStats.Weight;
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
		public override void Awake()
		{
			base.Awake();
			GetWeaponStateData(this.GetType().Name);
		}
		public override void Start()
		{
			_unitAttack = _thisBase.GetBehaviour<UnitAttack>();
			_unitMove = _thisBase.GetBehaviour<UnitMove>();
			_unitStat = _thisBase.GetBehaviour<UnitStat>();
			_unitAnimation = _thisBase.GetBehaviour<UnitAnimation>();

			_playerAttack = _unitAttack as PlayerAttack;
			_playerAnimation = _unitAnimation as PlayerAnimation;

			_changeStats = _weaponStats;
			WeaponLevel();
		}
		public override void Update()
		{
			Timer();
		}
		public virtual void ChangeKey()
		{
			InputManager.OnAttackPress += AttackCoroutine;
			InputManager.OnSkillPress += Skill;
			//_playerAttack.AttackColParent.AllDisableDir();
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
					Debug.Log(name);
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

			if (_thisBase.GetBehaviour<PlayerEquiq>().WeaponAnimation() != 1 && _thisBase.GetBehaviour<PlayerEquiq>().WeaponAnimation() != 3 &&
				_thisBase.GetBehaviour<PlayerAnimation>().CurWeaponAnimator.LastChange)
				_thisBase.GetBehaviour<PlayerMove>().stop = true;

			_playerAnimation.CurWeaponAnimator.SetDir = vec;
			_playerAnimation.CurWeaponAnimator.Attack = true;
			_playerAnimation.SetAnmation();
			AnimeClip animeClip = _playerAnimation.GetClip();
			_playerAnimation.GetClip().SetEventOnFrame(0, () => Attack(vec));
			_thisBase.AddState(BaseState.Attacking);
		}

		protected virtual void Skill()
		{

		}

		public virtual void LoadClassLevel(string name)
		{
			_weaponClassLevel = Define.GetManager<DataManager>().LoadWeaponClassLevel(name);
		}
		public virtual void SaveClassLevel()
		{
			Define.GetManager<DataManager>().SaveWeaponClassListData(_weaponClassLevel);
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
		protected virtual void WeaponLevel()
		{
			switch (Define.GetManager<DataManager>().LoadWeaponLevelData(this.GetType().Name))
			{
				case 1:
					_changeStats.Atk = _weaponStats.Atk +  20;
					break;
				case 2:
					_changeStats.Atk = _weaponStats.Atk + 45;
					break;
				case 3:
					_changeStats.Atk = _weaponStats.Atk + 75;
					break;
				case 4:
					_changeStats.Atk = _weaponStats.Atk + 110;
					break;
				case 5:
					_changeStats.Atk = _weaponStats.Atk + 150;
					break;
				case 6:
					_changeStats.Atk = _weaponStats.Atk + 195;
					break;
				case 7:
					_changeStats.Atk = _weaponStats.Atk + 245;
					break;
				case 8:
					_changeStats.Atk = _weaponStats.Atk + 300;
					break;
				case 9:
					_changeStats.Atk = _weaponStats.Atk + 360;
					break;
				case 10:
					_changeStats.Atk = _weaponStats.Atk + 425;
					break;
				case 11:
					_changeStats.Atk = _weaponStats.Atk + 495;
					break;
				case 12:
					_changeStats.Atk = _weaponStats.Atk + 570;
					break;
				default:
					break;
			}
		}
		public void KillEnemy()
		{
			_weaponClassLevel.killedCount++;
			LevelSystem();
			SaveClassLevel();
		}
		public virtual void Reset()
		{
			InputManager.OnAttackPress -= AttackCoroutine;
			InputManager.OnSkillPress -= Skill;
		}
	}
}