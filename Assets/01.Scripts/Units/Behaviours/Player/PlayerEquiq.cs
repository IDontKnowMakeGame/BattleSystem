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

		private bool isEquiq = true;

		public override void Awake()
		{
			base.Awake();
			//���⼭ ���� �ε����� �޾� �ش�.
			//_currentWeapon = DataManager.UserData.firstWeapon;
			//_secoundWeapon = DataManager.UserData.secondWeapon;

		}
		public override void Start()
		{
			playerAnimation = ThisBase.GetBehaviour<PlayerAnimation>();
			playerAttack = ThisBase.GetBehaviour<PlayerAttack>();

			playerAnimation.Play("Equip");

			InputManager.OnChangePress += ChangeWeapon;
			InputManager.OnOffPress += WeaponOnOff;
			InputManager.OnTestChangePress += TestChangeWeapon;

			Define.GetManager<EventManager>().StartListening(EventFlag.WeaponUpgrade, WeaponUpgrade);
			Define.GetManager<EventManager>().StartListening(EventFlag.SetWeapon, SetWeapon);
			Define.GetManager<EventManager>().StartListening(EventFlag.UnsetWeapon, UnSetWeapon);

			base.Start();

			CurrentWeapon?.ChangeKey();

			//InsertHelo("DirtyHalo", 0);
			//InsertHelo("EvilSpiritHalo", 1);

			ChangeWeapon();
		}
		public override void Update()
		{
			base.Update();
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

			ItemID temp = _currentWeapon;
			_currentWeapon = _secoundWeapon;
			_secoundWeapon = temp;

			InGame.PlayerBase.GetBehaviour<PlayerMove>().ClearMove();
			Define.GetManager<EventManager>().TriggerEvent(EventFlag.WeaponSwap, new EventParam());

			playerAttack.ChangeDelay(CurrentWeapon != null ? CurrentWeapon.WeaponStat.Afs : 0);

			ThisBase.GetBehaviour<PlayerMove>().stop = false;

			playerAnimation.Play("Equip");
		}
		private void TestChangeWeapon()
		{
			count++;
			count = count % 100;
			int dicCount = 0;
			foreach (var a in items)
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
			//���� ��Ȳ�� �� �ֱ�
			if (_currentWeapon == ItemID.None)
			{
				//TODO : �־��ֱ�
				//_currentWeapon = DataManager.UserData.firstWeapon;
				CurrentWeapon.ChangeKey();
				return;
			}
			else if (_secoundWeapon == ItemID.None)
			{
				//TODO : �־��ֱ�
				//_secoundWeapon = DataManager.UserData.secondWeapon;
				return;
			}

			//���⼭ ���� ��� �ٲٴ°� �ߵ�
			CurrentWeapon.Reset();

			//TODO : �־��ֱ�
			//_currentWeapon = DataManager.UserData.firstWeapon;
			CurrentWeapon.ChangeKey();

			InGame.PlayerBase.GetBehaviour<PlayerMove>().ClearMove();
			Define.GetManager<EventManager>().TriggerEvent(EventFlag.WeaponSwap, new EventParam());

			playerAnimation.Play("Equip");

			//TODO : �־��ֱ�
			//_secoundWeapon = DataManager.UserData.secondWeapon;
		}

		public void UnSetWeapon(EventParam eventParam)
		{
			if (DataManager.UserData.firstWeapon == "")
			{
				CurrentWeapon.Reset();
			}

			//TODO : �־��ֱ�
			//_currentWeapon = DataManager.UserData.firstWeapon;
			//_secoundWeapon = DataManager.UserData.secondWeapon;
		}
		public void WeaponUpgrade(EventParam eventParam)
		{
			CurrentWeapon.LevelSystem();
		}

	}
}
