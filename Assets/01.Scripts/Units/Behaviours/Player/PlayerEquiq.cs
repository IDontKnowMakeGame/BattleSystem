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
			base.Awake();
			_currentWeapon = DataManager.UserData.firstWeapon;
			_secoundWeapon = DataManager.UserData.secondWeapon;

			var st = Define.GetManager<DataManager>().LoadWeaponData();
			foreach (var a in st)
			{
				Type type = Type.GetType(a);
				Weapon weaponClass = Activator.CreateInstance(type) as Weapon;
				weaponClass._thisBase = ThisBase;
				weapons.Add(a, weaponClass);
			}
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

			Define.GetManager<EventManager>().StartListening(EventFlag.WeaponChange, ChangeWeapon);
			Define.GetManager<EventManager>().StartListening(EventFlag.WeaponUpgrade, WeaponUpgrade);

			base.Start();

			ThisBase.GetBehaviour<PlayerEquiq>().InsertHelo("DirtyHalo", 0);
			ThisBase.GetBehaviour<PlayerEquiq>().InsertHelo("EvilSpiritHalo", 1);

		}
		public override void Update()
		{
			base.Update();
			CurrentWeapon?.Update();
		}
		public override void OnDisable()
		{
			var manager = Define.GetManager<EventManager>();
			manager?.StopListening(EventFlag.WeaponChange, ChangeWeapon);
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

			playerAnimation.ChangeClips(animationClip.GetClip(WeaponAnimation()));
			playerAnimation.CurWeaponAnimator = playerAnimation.WeaponAnimators[WeaponAnimation()];
			playerAnimation.CurWeaponAnimator.ChangeWeapon = true;
			playerAnimation.SetAnmation();
		}
		public void ChangeWeapon(EventParam eventParam)
		{
			//if (eventParam.stringParam != null)
			//{
			//	if (eventParam.intParam == 1)
			//	{
			//		Debug.Log("�ֱ� 1��°" + eventParam.stringParam);
			//		_currentWeapon = eventParam.stringParam;
			//	}
			//	else
			//	{
			//		Debug.Log("�ֱ� 2��°" + eventParam.stringParam);
			//		_secoundWeapon = eventParam.stringParam;
			//	}

			//	CurrentWeapon?.ChangeKey();
			//}
			//else
			//{
			//	if (eventParam.intParam == 1)
			//	{
			//		CurrentWeapon?.Reset();
			//		_currentWeapon = eventParam.stringParam;
			//	}
			//	else
			//	{
			//		_secoundWeapon = eventParam.stringParam;
			//	}
			//}
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
		public void SetWeapon(string weaponEnum)
		{
			//���� ��Ȳ�� �� �ֱ�
			if (_currentWeapon == "")
			{
				CurrentWeapon.ChangeKey();
				_currentWeapon = weaponEnum;
				return;
			}
			else if (_secoundWeapon == "")
			{
				_secoundWeapon = weaponEnum;
				return;
			}

			//���⼭ ���� ��� �ٲٴ°� �ߵ�

		}

		public void WeaponUpgrade(EventParam eventParam)
		{
			CurrentWeapon.LevelSystem();
		}

		public void UnSetWeapon(int index)
		{
			if (index == 1)
			{
				CurrentWeapon.Reset();
				_currentWeapon = "";
			}
			else if (index == 2)
			{
				_secoundWeapon = "";
			}
		}
	}
}
