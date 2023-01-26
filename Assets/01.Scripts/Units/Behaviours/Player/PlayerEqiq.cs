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
			Define.GetManager<InputManager>().ChangeInGameAction(InputTarget.ChangeWeapon, InputStatus.Press, TestChangeWeapon);
			_currentWeapon = WeaponType.OldGreatSword;
			_secoundWeapon = WeaponType.OldStraightSword;
			base.Start();
		}
		private void ChangeWeapon()
		{
			WeaponType type = _currentWeapon;
			_currentWeapon = _secoundWeapon;
			_secoundWeapon = type;
		}

		private void TestChangeWeapon()
		{
			_currentWeapon = (WeaponType)(count++ % ((int)WeaponType.OldSpear + 1));
		}
	}
}
