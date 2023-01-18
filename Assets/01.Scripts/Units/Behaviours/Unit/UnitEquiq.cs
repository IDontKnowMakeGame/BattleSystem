using System.Collections;
using System.Collections.Generic;
using Units.Behaviours.Base;
using Unit.Core.Weapon;
using Units.Behaviours.Unit;

public class UnitEquiq : UnitBehaviour
{
	protected Weapon _currentWeapon;
	public Weapon CurrentWeapon => _currentWeapon;


	public Dictionary<WeaponType, Weapon> weapons;

	public List<Helo> _helos;

	public override void Start()
	{

	}
}
