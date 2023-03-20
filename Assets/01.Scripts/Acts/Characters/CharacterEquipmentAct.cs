using Actors.Characters;
using Acts.Base;
using Core;
using Data;
using System.Collections.Generic;
using UnityEngine;
using Acts.Characters.Player;
using Actors.Characters.Player;
using System;

[System.Serializable]
public class CharacterEquipmentAct : Act
{
	#region Weapon
	[SerializeField]
	protected ItemID _firstWeapon;
	[SerializeField]
	protected ItemID _secondWeapon;
	public Weapon CurrentWeapon
	{
		get
		{
			if (_firstWeapon == ItemID.None)
				return null;


			_useWeapon.TryGetValue(_firstWeapon, out _characterController.currentWeapon);
			if (_characterController.currentWeapon == null)
			{
				var weapon = Define.GetManager<ItemManager>().weapons[_firstWeapon];
				var clone = ObjectExtensions.Copy(weapon);
				_useWeapon.Add(_firstWeapon, clone);
				_characterController.currentWeapon = _useWeapon[_firstWeapon];
			}

			if (_isPlayer)
				_characterController.currentWeapon.isEnemy = false;

			return _characterController.currentWeapon;
		}
	}
	public Weapon SecoundWeapon
	{
		get
		{
			if (_secondWeapon == ItemID.None)
				return null;

			Weapon weapon;
			_useWeapon.TryGetValue(_secondWeapon, out weapon);
			if (weapon == null)
			{
				weapon = Define.GetManager<ItemManager>().weapons[_secondWeapon];
				var clone = ObjectExtensions.Copy(weapon);
				_useWeapon.Add(_secondWeapon, clone);
				weapon = _useWeapon[_secondWeapon];
			}

			if (_isPlayer)
				weapon.isEnemy = false;

			return weapon;
		}
	}
	protected Dictionary<ItemID, Weapon> _useWeapon = new Dictionary<ItemID, Weapon>();
	#endregion

	#region Halo
	[SerializeField]
	protected List<ItemID> _halos = new List<ItemID>();
	public ItemInfo HaloInfo
	{
		get
		{
			foreach (var info in _halos)
			{
				_halo += _useHalo[info].info;
			}
			return _halo;
		}
	}
	private ItemInfo _halo;
	protected Dictionary<ItemID, Halo> _useHalo = new Dictionary<ItemID, Halo>();
	#endregion

	[SerializeField]
	protected bool _isPlayer = false;
	//ETC
	protected CharacterActor _characterController;

	public override void Start()
	{
		_characterController = ThisActor as CharacterActor;
		CurrentWeapon?.Equiqment(_characterController);
	}

	/// <summary>
	/// Weapon을 바꿀 때 쓰는 함수이다.
	/// </summary>
	public virtual void Change()
	{
		CurrentWeapon?.UnEquipment(_characterController);
		SecoundWeapon?.Equiqment(_characterController);

		ItemID weapon = _firstWeapon;
		_firstWeapon = _secondWeapon;
		_secondWeapon = weapon;
	}

	protected virtual void EquipmentHalo()
	{
		//TODO 여기서 헤일로를 더해준다.
		//_halos.add
	}
}
