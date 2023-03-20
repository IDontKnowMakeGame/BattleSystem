using Core;
using Managements.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerEquipment : CharacterEquipmentAct
{
	public override void Start()
	{
		base.Start();
		InputManager<Weapon>.OnChangePress += Change;
	}

	public override void Update()
	{
		CurrentWeapon?.Update();
	}
}
