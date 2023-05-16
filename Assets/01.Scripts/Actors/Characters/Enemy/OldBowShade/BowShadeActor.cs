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
	[SerializeField]
	private float _dmage;

	[SerializeField]
	private float _speed;

	[SerializeField]
	private int _range;


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
		var dir = this.transform.position-InGame.Player.transform.position;
		if (dir.x != 0 && dir.z != 0)
		{
			state?.OnExit?.Invoke();
			return;
		}
		Bow bow = _characterEquipment.CurrentWeapon as Bow;
		dir.y = 0;
		bow.isDestroy = true;
		bow.Damage = _dmage;
		bow.Speed = _speed;
		bow.Range = _range;
		bow.IsScale = false;
		Debug.Log(dir);
		Debug.Log(InGame.CamDirCheck(dir.normalized));
		bow.Shoot(InGame.CamDirCheck(dir.normalized));

		Vector3 vector = this.transform.localScale;
		Vector3 vec = InGame.CamDirCheck(dir).x < 0 ? new Vector3(vector.x, vector.y, vector.z) : new Vector3(Mathf.Abs(vector.x) * -1, vector.y, vector.z);
		this.transform.localScale = vec;
	}
}
