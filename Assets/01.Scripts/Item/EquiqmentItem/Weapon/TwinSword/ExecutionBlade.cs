using Actors.Characters;
using Acts.Characters.Player;
using Core;
using DG.Tweening;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionBlade : TwinSword
{
	private PlayerMove _playerMove;

	private Vector3 _pos;
	private Vector3 _dir;
	private Vector3 _origindir;

	private Sequence _seq;

	public override void Equiqment(CharacterActor actor)
	{
		base.Equiqment(actor);
		_playerMove = _characterActor.GetAct<PlayerMove>();
	}

	public override void Skill(Vector3 vec)
	{
		if (_isCoolTime)
			return;

		//_playerMove.distance = 5;
		_pos = _characterActor.Position;
		_origindir = vec;
		_dir = InGame.CamDirCheck(_origindir);
		Define.GetManager<EventManager>().TriggerEvent(EventFlag.PlayTimeLine, new EventParam() { stringParam = "Execute" ,intParam = 1});

		_characterActor.AddState(CharacterState.Skill);
		_characterActor.StartCoroutine(WaitMove());
	}

	private IEnumerator WaitMove()
	{
		yield return new WaitForSeconds(0.83f/5);
		_characterActor.RemoveState(CharacterState.Skill);
		GameObject obj = Define.GetManager<ResourceManager>().Instantiate("Dust");
		obj.transform.position = _characterActor.Position + Vector3.up / 2 + _dir / 2;
		obj.transform.localRotation = Quaternion.LookRotation(_dir);

		int count = 1;
		for(count = 1; count < 5; count++)
		{
			if(InGame.GetBlock(_characterActor.Position + _origindir * count) != null)
			{
				if (InGame.GetBlock(_characterActor.Position + _origindir * count).ActorOnBlock == null && !InGame.GetBlock(_characterActor.Position + _origindir * count).isWalkable)
					break;
			}
			else
			{
				count--;
				break;
			}
		}

		float speed = 0.5f;
		_seq = DOTween.Sequence();
		Debug.Log((_origindir * count));
		Debug.Log((_origindir));
		Debug.Log(count);
		Debug.Log(_characterActor.Position);
		_seq.Append(_characterActor.transform.DOMove(_characterActor.Position + Vector3.up + (_origindir * count), speed).OnComplete(() =>
		{
			Vector3 left = Mathf.Abs(_dir.x) > Mathf.Abs(_dir.z) ? Vector3.back : Vector3.left;
			Vector3 right = Mathf.Abs(_dir.x) > Mathf.Abs(_dir.z) ? Vector3.forward : Vector3.right;

			GameObject obj = Define.GetManager<ResourceManager>().Instantiate("BladeHint");
			obj.transform.position = _pos + _dir + left / 2;
			obj.transform.localRotation = UnityEngine.Quaternion.LookRotation(_dir);
			GameObject obj2 = Define.GetManager<ResourceManager>().Instantiate("BladeHint");
			obj2.transform.position = _pos + _dir + right / 2;
			obj2.transform.localRotation = UnityEngine.Quaternion.LookRotation(_dir);

			for (int i = 1; i <= 5; i++)
			{
				InGame.Attack(_pos + (_dir * i) + left, Vector3.one, info.Atk, i / 10, _characterActor);
				InGame.Attack(_pos + (_dir * i) + right, Vector3.one, info.Atk, i / 10, _characterActor);
			}
			_playerMove.distance = 1;
			_isCoolTime = true;

			Define.GetManager<EventManager>().TriggerEvent(EventFlag.StopScreenEffect, new EventParam());
		}));

		_seq.Play();
	}
}
