using Actors.Characters;
using Actors.Characters.Player;
using Acts.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipmentAct : Act
{
	public Weapon CurrentWeapon
	{
		get
		{
			if (firstWeapon == ItemId.None)
				return null;

			Weapon weapon;
			_useWeapon.TryGetValue(firstWeapon, out weapon);
			if(weapon == null)
			{
				weapon = ItemManager.manager.weapons[firstWeapon];
				_useWeapon.Add(firstWeapon, ItemManager.manager.weapons[firstWeapon]);
			}

			return weapon;
		}
	}
	public Weapon SecoundWeapon
	{
		get
		{
			if (secondWeapon == ItemId.None)
				return null;

			Weapon weapon;
			_useWeapon.TryGetValue(secondWeapon, out weapon);
			if (weapon == null)
			{
				weapon = ItemManager.manager.weapons[secondWeapon];
				_useWeapon.Add(secondWeapon, ItemManager.manager.weapons[secondWeapon]);
			}

			return weapon;
		}
	}

	protected Dictionary<ItemId,Weapon> _useWeapon = new Dictionary<ItemId,Weapon>();

	protected ItemId firstWeapon;
	protected ItemId secondWeapon;

	private CharacterActor _characterController;

	public override void Awake()
	{
		base.Awake();
		_characterController = ThisActor as CharacterActor;
	}

	public override void Start()
	{
		base.Start();
		_characterController.Weapon = CurrentWeapon;
	}

	/// <summary>
	/// Weapon�� �ٲ� �� ���� �Լ��̴�.
	/// </summary>
	public void Change()
	{
		CurrentWeapon?.UnEquipment();
		SecoundWeapon?.Equiqment();

		ItemId weapon = firstWeapon;
		firstWeapon = secondWeapon;
		secondWeapon = weapon;

		_characterController.Weapon = CurrentWeapon;
	}

	/// <summary>
	/// ������������� Equiqment�� �� ������� ���� ���ſ� �ٲܰŸ� ���ش�.
	/// </summary>
	protected void EquiqmentItem()
	{
		//TODO ���⼭ EventParam�� �޾��ִµ� �׶� ���⼭ �������� ������ �ε����� ���� ������ �־��ش�.
		if(firstWeapon == ItemId.None /*&& evnetParam.intparam == 1*/)
		{
			//firstWeapon = Datamanger.Instnace.firstWeapon;
			CurrentWeapon.Equiqment();
		}

		if(secondWeapon == ItemId.None /*&& evnetParam.intparam == 2*/)
		{
			//secoundWeapon = Datamanger.Instnace.firstWeapon;
		}

		CurrentWeapon.UnEquipment();
		//firstWeapon = DataManager.Instance.firstWeaopn;
		//secondWeapon = DataManager.Instance.secoundWeaopn;
		CurrentWeapon.Equiqment();
	}
}
