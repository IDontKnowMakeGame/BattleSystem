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
		WaitState idlestate = _enemyAi.GetState<WaitState>();
		_characterEquipment.CurrentWeapon.Equiqment(this);
		state.OnEnter += () =>
		{
			Shoot(state);
		};
		idlestate.OnEnter += () =>
		{
			_enemyAnimation.Play("Idle");
		};
	}

	private void Shoot(ShootState state)
	{
		var dir = InGame.Player.Position - Position;
		if (dir.x != 0 && dir.z != 0)
		{
			state?.OnExit?.Invoke();
			return;
		}
		Bow bow = _characterEquipment.CurrentWeapon as Bow;
		dir.y = 0;
		bow.Shoot(InGame.CamDirCheck(dir.normalized));

		Vector3 vec = dir.x < 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
		this.transform.localScale = vec;
	}
}
