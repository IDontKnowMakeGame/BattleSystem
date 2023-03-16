using Actors.Characters;
using Acts.Base;
using Core;
using Data;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterEquipmentAct : Act
{
	public Weapon CurrentWeapon
	{
		get
		{
			if (_firstWeapon == ItemID.None)
				return null;

			Weapon weapon;
			_useWeapon.TryGetValue(_firstWeapon, out weapon);
			if(weapon == null)
			{
				_useWeapon.Add(_firstWeapon, Define.GetManager<ItemManager>().weapons[_firstWeapon]);
				weapon = _useWeapon[_firstWeapon];
			}

			return weapon;
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
				_useWeapon.Add(_secondWeapon, Define.GetManager<ItemManager>().weapons[_secondWeapon]);
				weapon = _useWeapon[_secondWeapon];
			}

			return weapon;
		}
	}

	protected Dictionary<ItemID, Weapon> _useWeapon = new Dictionary<ItemID, Weapon>();
	protected Dictionary<ItemID, Halo> _useHalo = new Dictionary<ItemID, Halo>();


	[SerializeField]
	protected ItemID _firstWeapon;
	[SerializeField]
	protected ItemID _secondWeapon;
	[SerializeField]
	protected List<ItemID> _halos = new List<ItemID>();

	private CharacterActor _characterController;

	public override void Awake()
	{
		base.Awake();
		_characterController = ThisActor as CharacterActor;
	}

	public override void Start()
	{
		base.Start();
		//_characterController.WeaponStat = CurrentWeapon.WeaponInfo;
	}

	/// <summary>
	/// Weapon을 바꿀 때 쓰는 함수이다.
	/// </summary>
	public void Change()
	{
		CurrentWeapon?.UnEquipment();
		SecoundWeapon?.Equiqment();

		ItemID weapon = _firstWeapon;
		_firstWeapon = _secondWeapon;
		_secondWeapon = weapon;

		//_characterController.WeaponStat = CurrentWeapon.WeaponInfo;
	}

	/// <summary>
	/// 비어져있을때는 Equiqment를 안 비어있을 때는 뺄거와 바꿀거를 해준다.
	/// </summary>
	protected void EquipmentWeapon()
	{
		//TODO 여기서 EventParam을 받아주는데 그때 여기서 변경해줄 무기의 인덱스와 무기 종류를 넣어준다.
		if(_firstWeapon == ItemID.None /*&& evnetParam.intparam == 1*/)
		{
			//firstWeapon = Datamanger.Instnace.firstWeapon;
			CurrentWeapon.Equiqment();
		}

		if(_secondWeapon == ItemID.None /*&& evnetParam.intparam == 2*/)
		{
			//secoundWeapon = Datamanger.Instnace.firstWeapon;
		}

		CurrentWeapon.UnEquipment();
		//firstWeapon = DataManager.Instance.firstWeaopn;
		//secondWeapon = DataManager.Instance.secoundWeaopn;
		CurrentWeapon.Equiqment();
	}

	protected void EquipmentHalo()
	{
		//TODO 여기서 헤일로를 더해준다.
		//_halos.add
	}
}
