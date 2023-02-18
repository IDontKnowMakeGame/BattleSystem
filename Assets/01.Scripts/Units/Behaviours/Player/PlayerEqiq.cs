using Units.Behaviours.Unit;
using System;
using Core;
using Managements.Managers;
using Unit.Core.Weapon;
using UnityEngine;
namespace Units.Base.Player
{
	[Serializable]
	public class PlayerEqiq : UnitEquiq
	{
		private int count;

		private UnitAnimation unitAnimation;
		private PlayerAttack playerAttack;
		private AnimationClip animationClip;

		public override void Awake()
		{
			base.Awake();
			_currentWeapon = DataManager.UserData.currentWeapon;
			_secoundWeapon = DataManager.UserData.secondWeapon;
		}

		public override void Update()
		{
			base.Update();
			CurrentWeapon?.Update();
		}
		public override void Start()
		{
			unitAnimation = ThisBase.GetBehaviour<UnitAnimation>();
			playerAttack = ThisBase.GetBehaviour<PlayerAttack>();
			animationClip = ThisBase.GetComponent<AnimationClip>();

			unitAnimation.ChangeClips(animationClip.GetClip(WeaponAnimation()));
			unitAnimation.ChangeState(10);

			InputManager.OnChangePress += ChangeWeapon;
			InputManager.OnOffPress += WeaponOnOff;
			InputManager.OnTestChangePress += TestChangeWeapon;

			Define.GetManager<EventManager>().StartListening(EventFlag.WeaponChange, ChangeWeapon);

			base.Start();

			ThisBase.GetBehaviour<PlayerEqiq>().InsertHelo("DirtyHalo", 0);
			ThisBase.GetBehaviour<PlayerEqiq>().InsertHelo("EvilSpiritHalo", 1);

		}

        public override void OnDisable()
        {
	        var manager = Define.GetManager<EventManager>();
			manager?.StopListening(EventFlag.WeaponChange, ChangeWeapon);
			base.OnDisable();
        }

        private void WeaponOnOff()
		{
			CurrentWeapon.Reset();
		}
		private void ChangeWeapon()
		{
			if (ThisBase.State.HasFlag(Unit.BaseState.Skill))
				return;

			weapons[_currentWeapon].Reset();
			weapons[_secoundWeapon].ChangeKey();
			string temp = _currentWeapon;
			_currentWeapon = _secoundWeapon;
			_secoundWeapon = temp;

			InGame.PlayerBase.GetBehaviour<PlayerMove>().ClearMove();

			playerAttack.ChangeDelay(CurrentWeapon.WeaponStat.Afs);
			unitAnimation.ChangeClips(animationClip.GetClip(WeaponAnimation()));
			unitAnimation.ChangeState(10);
		}

		public void ChangeWeapon(EventParam eventParam)
		{
			if(eventParam.stringParam == null)
			{
				if(eventParam.intParam == 1)
				{
					weapons[_currentWeapon].Reset();
					_currentWeapon = eventParam.stringParam;
				}
				else
					_secoundWeapon = eventParam.stringParam;
			}
			else
			{
				if (eventParam.intParam == 1)
					_currentWeapon = eventParam.stringParam;
				else
					_secoundWeapon = eventParam.stringParam;

				CurrentWeapon?.ChangeKey();
			}	
		}

		private void TestChangeWeapon()
		{
			count++;
			count = count % 7;
			int dicCount = 0;
			foreach(var a in weapons)
			{
				dicCount++;
				if(dicCount == count)
				{
					CurrentWeapon.Reset();
					_currentWeapon = a.Key;
					CurrentWeapon.ChangeKey();
					return;
				}
			}
		}
	}
}
