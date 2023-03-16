using Actors.Characters;
using Acts.Base;
using Core;
using Data;
using System.Collections.Generic;
using UnityEngine;

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
	#endregion

	#region Halo
	[SerializeField]
	protected List<ItemID> _halos = new List<ItemID>();
	public ItemInfo HaloInfo
	{
		get
		{
			foreach(var info in _halos)
			{
				_halo += _useHalo[info].info;
			}
			return _halo;
		}
	}
	private ItemInfo _halo;
	protected Dictionary<ItemID, Halo> _useHalo = new Dictionary<ItemID, Halo>();
	#endregion

	//ETC
	private CharacterActor _characterController;
	//private CharacterStatAct _characterStat;

	public override void Awake()
	{
		base.Awake();
		_characterController = ThisActor as CharacterActor;
		//_characterStat = _characterController.GetAct<CharacterStatAct>();
	}

	public override void Start()
	{
		base.Start();
	}

	/// <summary>
	/// Weapon�� �ٲ� �� ���� �Լ��̴�.
	/// </summary>
	public void Change()
	{
		CurrentWeapon?.UnEquipment(_characterController);
		SecoundWeapon?.Equiqment(_characterController);

		ItemID weapon = _firstWeapon;
		_firstWeapon = _secondWeapon;
		_secondWeapon = weapon;


	}

	/// <summary>
	/// ������������� Equiqment�� �� ������� ���� ���ſ� �ٲܰŸ� ���ش�.
	/// </summary>
	protected void EquipmentWeapon()
	{
		//TODO ���⼭ EventParam�� �޾��ִµ� �׶� ���⼭ �������� ������ �ε����� ���� ������ �־��ش�.
		if(_firstWeapon == ItemID.None /*&& evnetParam.intparam == 1*/)
		{
			//firstWeapon = Datamanger.Instnace.firstWeapon;
			CurrentWeapon.Equiqment(_characterController);
		}

		if(_secondWeapon == ItemID.None /*&& evnetParam.intparam == 2*/)
		{
			//secoundWeapon = Datamanger.Instnace.firstWeapon;
		}

		CurrentWeapon.UnEquipment(_characterController);
		//firstWeapon = DataManager.Instance.firstWeaopn;
		//secondWeapon = DataManager.Instance.secoundWeaopn;
		CurrentWeapon.Equiqment(_characterController);
	}

	protected void EquipmentHalo()
	{
		//TODO ���⼭ ���Ϸθ� �����ش�.
		//_halos.add
	}
}
