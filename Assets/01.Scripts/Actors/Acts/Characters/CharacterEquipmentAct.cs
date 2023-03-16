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
				_useWeapon.Add(firstWeapon, ItemManager.manager.weapons[firstWeapon]);
				weapon = _useWeapon[firstWeapon]; 
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
				_useWeapon.Add(secondWeapon, ItemManager.manager.weapons[secondWeapon]);
				weapon = _useWeapon[secondWeapon];
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
		CurrentWeapon.info.Atk = 1000;
		Debug.Log(CurrentWeapon.info.Atk);
	}

	/// <summary>
	/// Weapon을 바꿀 때 쓰는 함수이다.
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
	/// 비어져있을때는 Equiqment를 안 비어있을 때는 뺄거와 바꿀거를 해준다.
	/// </summary>
	protected void EquiqmentItem()
	{
		//TODO 여기서 EventParam을 받아주는데 그때 여기서 변경해줄 무기의 인덱스와 무기 종류를 넣어준다.
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
