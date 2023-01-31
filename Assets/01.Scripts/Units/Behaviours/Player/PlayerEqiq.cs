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
			weapons[_secoundWeapon].Update();
		}
		public override void Start()
		{
			unitAnimation = ThisBase.GetBehaviour<UnitAnimation>();
			animationClip = ThisBase.GetComponent<AnimationClip>();
			unitAnimation.ChangeClips(animationClip.GetClip(WeaponAnimation()));

			Define.GetManager<InputManager>().AddInGameAction(InputTarget.TestChangeWeapon, InputStatus.Press, TestChangeWeapon);
			Define.GetManager<InputManager>().AddInGameAction(InputTarget.ChangeWeapon, InputStatus.Press, ChangeWeapon);
			Define.GetManager<InputManager>().ChangeInGameAction(InputTarget.WeaponOnOff, InputStatus.Press, WeaponOnOff);
			base.Start();
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

			unitAnimation.ChangeClips(animationClip.GetClip(WeaponAnimation()));
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
