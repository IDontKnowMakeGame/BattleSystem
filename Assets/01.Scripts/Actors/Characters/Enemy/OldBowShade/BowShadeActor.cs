using Actors.Characters.Enemy;
using Acts.Characters;
using AI.States;
using Core;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BowShadeActor : EnemyActor
{
	protected override void Init()
	{
		base.Init();
		AddAct(_enemyAi);
	}

	protected override void Start()
	{
		base.Start();
		ShootState state = _enemyAi.GetState<ShootState>();
		Debug.Log(_characterEquipment.CurrentWeapon);
		_characterEquipment.CurrentWeapon.Equiqment(this);
		Debug.Log(":");
		state.OnEnter += () =>
		{
			Shoot();
		};
	}

	private void Shoot()
	{
		var dir = InGame.Player.Position - Position;
		Bow bow = _characterEquipment.CurrentWeapon as Bow;
		dir.y = 0;
		bow.Shoot(InGame.CamDirCheck(dir));
	}
}
