using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponContainer", menuName = "SO/Container")]
public class WeaponContainer : ScriptableObject
{
	public List<WeaponSO> weapons;

	[SerializeField]
	private SwordType _swordType;

	private WeaponSO _currentWeapon;

	public WeaponSO CurrentWeapon => _currentWeapon;

	private int index;

	public void OnValidate()
	{
		_currentWeapon = weapons[(int)_swordType];
	}

	public void ChangeWeapon()
	{
		SetWeapon(weapons[index++ % weapons.Count]);
	}

	public void SetWeapon(WeaponSO weapon)
	{
		_currentWeapon = weapon;
		Debug.Log(_currentWeapon.type);
	}
}
