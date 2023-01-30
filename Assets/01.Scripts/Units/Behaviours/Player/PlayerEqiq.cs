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
			base.Start();
		}

		private void TestChangeWeapon()
		{
			count = count++ % 5;
			int dicCount = 0;
			foreach(var a in weapons)
			{
				dicCount++;
				if(dicCount == count)
				{
					_currentWeapon = a.Key;
				}
			}
		}
	}
}
