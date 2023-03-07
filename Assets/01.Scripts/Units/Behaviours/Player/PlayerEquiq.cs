using Units.Behaviours.Unit;
using System;
using Core;
using Managements.Managers;
using Unit.Core.Weapon;
using UnityEngine;
namespace Units.Base.Player
{
	[Serializable]
	public class PlayerEquiq : UnitEquiq
	{
		private int count;

		private PlayerAnimation playerAnimation;
		private PlayerAttack playerAttack;
		private AnimationClip animationClip;

		private bool isEquiq = true;

		public override void Awake()
		{
			var st = Define.GetManager<DataManager>().LoadWeaponData();
			foreach (var a in st)
			{
				Type type = Type.GetType(a);
				Weapon weaponClass = Activator.CreateInstance(type) as Weapon;
				weaponClass._thisBase = ThisBase;
				weapons.Add(a, weaponClass);
			}
			base.Awake();
			_currentWeapon = DataManager.UserData.firstWeapon;
			_secoundWeapon = DataManager.UserData.secondWeapon;

		}
		public override void Start()
		{
			playerAnimation = ThisBase.GetBehaviour<PlayerAnimation>();
			playerAttack = ThisBase.GetBehaviour<PlayerAttack>();
			animationClip = ThisBase.GetComponent<AnimationClip>();

			playerAnimation.ChangeClips(animationClip.GetClip(WeaponAnimation()));
			playerAnimation.CurWeaponAnimator = playerAnimation.WeaponAnimators[WeaponAnimation()];
			playerAnimation.CurWeaponAnimator.ChangeWeapon = true;
			playerAnimation.SetAnmation();

			InputManager.OnChangePress += ChangeWeapon;
			InputManager.OnOffPress += WeaponOnOff;
			InputManager.OnTestChangePress += TestChangeWeapon;

			Define.GetManager<EventManager>().StartListening(EventFlag.WeaponUpgrade, WeaponUpgrade);
			Define.GetManager<EventManager>().StartListening(EventFlag.SetWeapon, SetWeapon);
			Define.GetManager<EventManager>().StartListening(EventFlag.UnsetWeapon, UnSetWeapon);

			base.Start();

			ThisBase.GetBehaviour<PlayerEquiq>().InsertHelo("DirtyHalo", 0);
			ThisBase.GetBehaviour<PlayerEquiq>().InsertHelo("EvilSpiritHalo", 1);

			ChangeWeapon();
		}
		public override void Update()
		{
			base.Update();
			CurrentWeapon?.Update();
		}
		public override void OnDisable()
		{
			var manager = Define.GetManager<EventManager>();
			CurrentWeapon?.Reset();
			base.OnDisable();
		}


		private void WeaponOnOff()
		{
			if (isEquiq)
				CurrentWeapon?.Reset();
			else
				CurrentWeapon?.ChangeKey();

			isEquiq = !isEquiq;
		}
		private void ChangeWeapon()
		{
			if (CurrentWeapon == null || SecoundWeapon == null)
				return;

			if (ThisBase.State.HasFlag(Unit.BaseState.Skill) || ThisBase.State.HasFlag(Unit.BaseState.StopMove))
				return;

			CurrentWeapon?.Reset();
			SecoundWeapon?.ChangeKey();

			string temp = _currentWeapon;
			_currentWeapon = _secoundWeapon;
			_secoundWeapon = temp;

			InGame.PlayerBase.GetBehaviour<PlayerMove>().ClearMove();
			Define.GetManager<EventManager>().TriggerEvent(EventFlag.WeaponSwap, new EventParam());

			playerAttack.ChangeDelay(CurrentWeapon != null ? CurrentWeapon.WeaponStat.Afs : 0);

			ThisBase.GetBehaviour<PlayerMove>().stop = false;

			playerAnimation.ChangeClips(animationClip.GetClip(WeaponAnimation()));
			playerAnimation.CurWeaponAnimator = playerAnimation.WeaponAnimators[WeaponAnimation()];
			playerAnimation.CurWeaponAnimator.ChangeWeapon = true;
			playerAnimation.SetAnmation();
		}
		private void TestChangeWeapon()
		{
			count++;
			count = count % 7;
			int dicCount = 0;
			foreach (var a in weapons)
			{
				dicCount++;
				if (dicCount == count)
				{
					CurrentWeapon.Reset();
					_currentWeapon = a.Key;
					CurrentWeapon.ChangeKey();
					return;
				}
			}
		}
		public void SetWeapon(EventParam eventParam)
		{
			//없는 상황일 때 넣기
			if (_currentWeapon == "")
			{
				_currentWeapon = DataManager.UserData.firstWeapon;
				CurrentWeapon.ChangeKey();
				return;
			}
			else if (_secoundWeapon == "")
			{
				_secoundWeapon = DataManager.UserData.secondWeapon;
				return;
			}

			//여기서 있을 경우 바꾸는거 발동
			CurrentWeapon.Reset();
			_currentWeapon = DataManager.UserData.firstWeapon;
			CurrentWeapon.ChangeKey();

			_currentWeapon = DataManager.UserData.secondWeapon;
		}

		public void UnSetWeapon(EventParam eventParam)
		{
			if (DataManager.UserData.firstWeapon == "")
			{
				CurrentWeapon.Reset();
			}

			_currentWeapon = DataManager.UserData.firstWeapon;
			_secoundWeapon = DataManager.UserData.secondWeapon;
		}
		public void WeaponUpgrade(EventParam eventParam)
		{
			CurrentWeapon.LevelSystem();
		}

	}
}
