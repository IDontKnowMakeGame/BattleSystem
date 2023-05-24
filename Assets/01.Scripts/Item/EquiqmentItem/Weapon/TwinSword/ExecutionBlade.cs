using Actors.Characters;
using Acts.Characters.Player;
using Core;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionBlade : TwinSword
{
	private PlayerMove _playerMove;

	private Vector3 _pos;
	private Vector3 _dir;

	public override void Equiqment(CharacterActor actor)
	{
		base.Equiqment(actor);
		_playerMove = _characterActor.GetAct<PlayerMove>();
	}

	public override void Skill(Vector3 vec)
	{
		//if (_isCoolTime)
		//	return;

		_playerMove.distance = 5;
		_playerMove.Translate(vec);
		PlayerMove.OnMoveEnd += OnEnd;
		_pos = _characterActor.Position;
		_dir = InGame.CamDirCheck(vec);
	}

	private void OnEnd(int id, Vector3 vec)
	{
		if (id != _characterActor.UUID)
			return;
		Vector3 left = Mathf.Abs(_dir.x) > Mathf.Abs(_dir.z) ? Vector3.back : Vector3.left;
		Vector3 right = Mathf.Abs(_dir.x) > Mathf.Abs(_dir.z) ? Vector3.forward : Vector3.right;

		GameObject obj = Define.GetManager<ResourceManager>().Instantiate("BladeHint");
		obj.transform.position = _pos + _dir + left/2;
		obj.transform.localRotation = UnityEngine.Quaternion.LookRotation(vec);
		GameObject obj2 = Define.GetManager<ResourceManager>().Instantiate("BladeHint");
		obj2.transform.position = _pos + _dir + right/2;
		obj2.transform.localRotation = UnityEngine.Quaternion.LookRotation(vec);

		Debug.Log(vec);
		for (int i = 1; i <= 5; i++)
		{
			InGame.Attack(_pos + (_dir * i) + left, Vector3.one, info.Atk, i / 10, _characterActor);
			InGame.Attack(_pos + (_dir * i) + right, Vector3.one, info.Atk, i / 10, _characterActor);
		}
		_playerMove.distance = 1;
		_isCoolTime = true;
		PlayerMove.OnMoveEnd -= OnEnd;
	}
}
