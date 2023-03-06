using Core;
using Managements.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using Unit.Core.Weapon;
using Units.Base.Interactable;
using Units.Base.Player;
using Units.Behaviours.Unit;
using UnityEngine;

public class WeaponBase : InteractableUnitBase
{
	[SerializeField]
	private string _weaponEnum;
	public override void Interact()
	{
		if (IsInteracted) return;
		if (DetectCondition.Invoke(Position))
		{
			IsInteracted = true;
			Type type = Type.GetType(_weaponEnum.ToString());
			Weapon weapon = Activator.CreateInstance(type) as Weapon;
			weapon._thisBase = InGame.PlayerBase;
			InGame.PlayerBase.GetBehaviour<UnitEquiq>().InsertWeapon(_weaponEnum, weapon);
			this.gameObject.SetActive(false);
		}
	}
}
