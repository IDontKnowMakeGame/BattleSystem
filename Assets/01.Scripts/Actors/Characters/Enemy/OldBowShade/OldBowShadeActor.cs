using Actors.Characters.Enemy;
using Acts.Characters;
using AI.States;
using Core;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OldBowShadeActor : EnemyActor
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
			_enemyAnimation.Play("JumpAttack");
			_enemyAnimation.curClip.SetEventOnFrame(_enemyAnimation.curClip.fps - 1, Shoot);
		};
	}

	private void Shoot()
	{
		var dir = InGame.Player.Position - Position;
		Debug.Log(_characterEquipment.CurrentWeapon);
		Bow bow = _characterEquipment.CurrentWeapon as Bow;
		bow.Shoot(dir.normalized);
	}
}
