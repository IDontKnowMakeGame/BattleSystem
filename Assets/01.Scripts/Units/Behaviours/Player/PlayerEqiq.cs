using Units.Behaviours.Unit;
using System;
using Core;
using Managements.Managers;
using Unit.Core.Weapon;
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

			Define.GetManager<InputManager>().AddInGameAction(InputTarget.TestChangeWeapon, InputStatus.Press, TestChangeWeapon);
			Define.GetManager<InputManager>().AddInGameAction(InputTarget.ChangeWeapon, InputStatus.Press, ChangeWeapon);
			Define.GetManager<InputManager>().ChangeInGameAction(InputTarget.WeaponOnOff, InputStatus.Press, WeaponOnOff);

			EventManager.StartListening(EventFlag.WeaponChange, ChangeWeapon);
			base.Start();
		}

        public override void OnDisable()
        {
			EventManager.StopListening(EventFlag.WeaponChange, ChangeWeapon);
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

			CurrentWeapon.Reset();
			string temp = _currentWeapon;
			_currentWeapon = _secoundWeapon;
			_secoundWeapon = temp;
			CurrentWeapon.ChangeKey();

			playerAttack.ChangeDelay(CurrentWeapon.WeaponStat.Afs);
			unitAnimation.ChangeClips(animationClip.GetClip(WeaponAnimation()));
			unitAnimation.ChangeState(10);
			EventManager.TriggerEvent(EventFlag.WeaponSwap, new EventParam());
		}

		public void ChangeWeapon(EventParam eventParam)
		{
			switch(eventParam.intParam)
			{
				case 1:
					_currentWeapon = eventParam.stringParam;
					break;
				case 2:
					_secoundWeapon = eventParam.stringParam;
					break;
			}
			CurrentWeapon?.ChangeKey();
		}
		private void TestChangeWeapon()
		{
			count++;
			count = count % 5;
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
