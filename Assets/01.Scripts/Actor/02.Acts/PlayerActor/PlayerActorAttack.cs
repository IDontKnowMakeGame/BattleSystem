using Actor.Bases;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerActorAttack : ActorAttack
{
	private PlayerController _playerController;

	private Weapon currentWeapon => _playerController.weapon;
	public void Start()
	{
		_playerController = _controller as PlayerController;


	}
	public void SetAttack(Vector3 vec, ItemInfo weapon)
	{
		if (currentWeapon is BaseStraightSword)
			StraightSword(vec, weapon);
		else if (currentWeapon is BaseGreatSword)
			GreatSword(vec, weapon);
		else if (currentWeapon is BaseTwinSword)
			TwinSword(vec, weapon);
		else if (currentWeapon is BaseSpear)
			Spear(vec, weapon);
		else if (currentWeapon is BaseBow)
			Bow(vec, weapon);
	}

	public void StraightSword(Vector3 vec, ItemInfo weapon)
	{

	}
	public void GreatSword(Vector3 vec, ItemInfo weapon)
	{

	}
	public void TwinSword(Vector3 vec, ItemInfo weapon)
	{

	}
	public void Bow(Vector3 vec, ItemInfo weapon)
	{

	}
	public void Spear(Vector3 vec, ItemInfo weapon)
	{

	}
}
