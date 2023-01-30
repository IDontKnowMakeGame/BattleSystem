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
		public override void Start()
		{
			Define.GetManager<InputManager>().ChangeInGameAction(InputTarget.TestChangeWeapon, InputStatus.Press, TestChangeWeapon);
			Define.GetManager<InputManager>().ChangeInGameAction(InputTarget.ChangeWeapon, InputStatus.Press, ChangeWeapon);
			Define.GetManager<InputManager>().ChangeInGameAction(InputTarget.WeaponOnOff, InputStatus.Press, WeaponOnOff);
			base.Start();
		}

		private void WeaponOnOff()
		{
			CurrentWeapon.Reset();
			isOff = !isOff;
		}
		private void ChangeWeapon()
		{
			string temp = _currentWeapon;
			_currentWeapon = _secoundWeapon;
			_secoundWeapon = temp;
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
					_currentWeapon = a.Key;
					return;
				}
			}
		}
	}
}
