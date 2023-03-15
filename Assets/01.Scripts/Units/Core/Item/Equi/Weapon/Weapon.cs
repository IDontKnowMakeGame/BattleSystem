using System;
using UnityEngine;
using Units.Behaviours.Unit;
using Units.Base.Unit;
using Managements.Managers;
using Units.Base.Player;
using Core;
using Tools;
using System.Collections.Generic;

namespace Unit.Core.Weapon
{
	[Serializable]
	public class Weapon : EquipmentItem
	{
		#region weapon Stat변경하는 것들
		protected WeaponStats _weaponStats = null;
		protected WeaponStats _changeStats = new WeaponStats();
		protected WeaponStats _changeBuffStats = new WeaponStats();
		private WeaponStats _WeaponStats = new WeaponStats();
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
		#endregion
		//유닛 공격, 유닛 move
		#region 유닛 공격이나 move등등
		protected AttackCollider _attackCollider = null;
		protected UnitMove _unitMove;
		protected UnitStat _unitStat;
		protected UnitAnimation _unitAnimation;

		protected PlayerAnimation _playerAnimation;
		protected PlayerAttack _playerAttack;
		#endregion

		#region 타이머
		protected float _currentTime;
		protected float _maxTime;

		protected bool _isCoolTime = false;
		#endregion

		public bool isSkill = false;

		protected bool _isEnemy = true;

		protected SliderObject _sliderObject;

		protected WeaponClassLevel _weaponClassLevel;

		private bool attackInit = false;

		protected Action attackEndAction = null;
		public override void Awake()
		{
			base.Awake();
			GetWeaponStateData(this.GetType().Name);
		}
		public override void Start()
		{
			if (!_isEnemy)
				_attackCollider = thisBase.GetComponentInChildren<AttackCollider>();

			_unitMove = thisBase.GetBehaviour<UnitMove>();
			_playerAttack = thisBase.GetBehaviour<PlayerAttack>();
			_unitStat = thisBase.GetBehaviour<UnitStat>();
			_unitAnimation = thisBase.GetBehaviour<UnitAnimation>();

			_playerAnimation = _unitAnimation as PlayerAnimation;

			_changeStats = _weaponStats;
			WeaponLevel();
		}
		public override void Update()
		{
			Timer();
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


		protected virtual void Attack(Vector3 vec)
		{

		}
		protected virtual void AttackCoroutine(Vector3 vec)
		{
			if (thisBase.GetBehaviour<PlayerItem>().PlayerShield.UseAble) return;

			if (thisBase.State.HasFlag(BaseState.Attacking) ||
				thisBase.State.HasFlag(BaseState.Moving))
				return;

			if (thisBase.State.HasFlag(BaseState.Moving))
				return;

			if (thisBase.GetBehaviour<PlayerEquiq>().WeaponAnimation() != 1 && thisBase.GetBehaviour<PlayerEquiq>().WeaponAnimation() != 3)
				thisBase.GetBehaviour<PlayerMove>().stop = true;

			if (!thisBase.State.HasFlag(BaseState.Attacking))
			{
				_playerAttack.AttackAnimation(vec);
				thisBase.AddState(BaseState.Attacking);
			}
			else
				thisBase.GetBehaviour<PlayerMove>().stop = false;
		}
		protected virtual void Attack()
		{
			if (!thisBase.State.HasFlag(BaseState.Attacking))
				return;
			if (attackInit)
				return;

			thisBase.StartCoroutines(WeaponStat.Afs, () => attackInit = true,
				() =>
				{
					thisBase.RemoveState(BaseState.Attacking);
					attackInit = false;
				});


			List<EnemyBase> enemys = new List<EnemyBase>();
			enemys = _attackCollider.AllCurrentDirEnemy();

			if (enemys.Count > 0)
			{
				//playerBuff.ChangeAdneraline(1);
				EventParam param = new EventParam();
				param.intParam = 1;
				Define.GetManager<EventManager>().TriggerEvent(EventFlag.PlayTimeLine, param);
			}

			foreach (EnemyBase enemy in enemys)
			{
				enemy.ThisStat.Damaged(WeaponStat.Atk, thisBase);
				GameObject obj = Define.GetManager<ResourceManagers>().Instantiate("Damage");
				obj.GetComponent<DamagePopUp>().DamageText(WeaponStat.Atk, enemy.transform.position);
				attackEndAction?.Invoke();
			}
		}
		protected virtual void Skill()
		{

		}
		public virtual void ChangeKey()
		{
			InputManager.OnAttackPress += AttackCoroutine;
			InputManager.OnSkillPress += Skill;
			LevelSystem();
		}
		public virtual void Reset()
		{
			InputManager.OnAttackPress -= AttackCoroutine;
			InputManager.OnSkillPress -= Skill;
		}

		#region LevelSystem + ClassLevelSystem
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
		public virtual void LevelSystem()
		{

		}
		protected virtual void WeaponLevel()
		{
			switch (Define.GetManager<DataManager>().LoadWeaponLevelData(this.GetType().Name))
			{
				case 1:
					_changeStats.Atk = _weaponStats.Atk + 20;
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
		#endregion

		#region Data
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
		#endregion
	}
}