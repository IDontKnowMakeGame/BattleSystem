using System.Collections;
using System.Collections.Generic;
using Units.Behaviours.Base;
using Unit.Core.Weapon;
using Units.Behaviours.Unit;

public class UnitEquiq : UnitBehaviour
{
	protected WeaponType _currentWeapon;
	protected WeaponType _secoundWeapon;
	public Weapon CurrentWeapon
	{
        get
		{
            return weapons[_currentWeapon];
		}
	}

	public Dictionary<WeaponType, Weapon> weapons = new Dictionary<WeaponType, Weapon>();

	public List<Helo> _helos = new List<Helo>();
    
    public override void Awake()
    {
        weapons.Add(WeaponType.OldStraightSword, new OldStraightSword() { _thisBase = this.ThisBase });
        weapons.Add(WeaponType.OldGreatSword, new OldGreatSword() { _thisBase = this.ThisBase });
        weapons.Add(WeaponType.OldTwinSword, new OldTwinSword() { _thisBase = this.ThisBase });

        foreach (var value in weapons)
        {
            value.Value?.Awake();
        }

        foreach (var value in _helos)
        {
            value?.Awake();
        }
    }

    public override void Start()
    {
        foreach (var value in weapons)
        {
            value.Value?.Start();
        }

        foreach (var value in _helos)
        {
            value?.Start();
        }
    }
    public override void Update()
    {
        weapons[_currentWeapon]?.Update();

        foreach (var value in _helos)
        {
            value?.Update();
        }
    }
}
