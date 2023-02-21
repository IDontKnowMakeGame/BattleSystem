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

		private PlayerAnimation playerAnimation;
		private PlayerAttack playerAttack;
		private AnimationClip animationClip;

		private bool isEquiq = true;
		public override void Awake()
		{
			base.Awake();
			_currentWeapon = DataManager.UserData.firstWeapon;
			_secoundWeapon = DataManager.UserData.secondWeapon;
		}

		public override void Update()
		{
			base.Update();
			CurrentWeapon?.Update();
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
			if (isEquiq)
				CurrentWeapon?.Reset();
			else
				CurrentWeapon?.ChangeKey();

			isEquiq = !isEquiq;
		}
		private void ChangeWeapon()
		{
			if (_currentWeapon == null || _secoundWeapon == null)
				return;

			if (ThisBase.State.HasFlag(Unit.BaseState.Skill))
				return;

			CurrentWeapon?.Reset();
			weapons[_secoundWeapon].ChangeKey();
			string temp = _currentWeapon;
			_currentWeapon = _secoundWeapon;
			_secoundWeapon = temp;

			InGame.PlayerBase.GetBehaviour<PlayerMove>().ClearMove();
			Define.GetManager<EventManager>().TriggerEvent(EventFlag.WeaponSwap,new EventParam());

			playerAttack.ChangeDelay(CurrentWeapon.WeaponStat.Afs);

			playerAnimation.ChangeClips(animationClip.GetClip(WeaponAnimation()));
			playerAnimation.CurWeaponAnimator = playerAnimation.WeaponAnimators[WeaponAnimation()];
			playerAnimation.CurWeaponAnimator.ChangeWeapon = true;
			playerAnimation.SetAnmation();
		}

		public void ChangeWeapon(EventParam eventParam)
		{
			if (eventParam.stringParam != null)
			{
				if (eventParam.intParam == 1)
				{
					Debug.Log("넣기 1번째" + eventParam.stringParam);
					_currentWeapon = eventParam.stringParam;
				}
				else
				{
					Debug.Log("넣기 2번째" + eventParam.stringParam);
					_secoundWeapon = eventParam.stringParam;
				}

				CurrentWeapon?.ChangeKey();
			}
			else
			{
				if (eventParam.intParam == 1)
				{
					CurrentWeapon?.Reset();
					Debug.Log("없애기 1번째" + eventParam.stringParam);
					_currentWeapon = eventParam.stringParam;
				}
				else
				{
					Debug.Log("없애기 2번째" + eventParam.stringParam);
					_secoundWeapon = eventParam.stringParam;
				}
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
