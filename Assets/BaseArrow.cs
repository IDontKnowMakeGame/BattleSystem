using Actors.Characters;
using UnityEngine;
using DG.Tweening;
using Core;
using Managements.Managers;
using Blocks;
using Actors.Characters.Player;
using Managements;

public class Arrow : MonoBehaviour
{
	private CharacterActor _shootActor;
	private CharacterActor _stickActor;

	private Vector3 _shootVec;
	private float _damage;
	private bool _isStick = false;
	private bool _canPull = false;

	public static void ShootArrow(Vector3 vec, Vector3 position, CharacterActor actor, float speed, float damage, int distance)
	{
		Arrow obj = Define.GetManager<ResourceManager>().Instantiate("Arrow").GetComponent<Arrow>();
		obj.Shoot(vec, position, actor, speed, damage, distance);
	}
	public virtual void Shoot(Vector3 vec, Vector3 position,CharacterActor actor, float speed, float damage, int distance)
	{
		this.transform.position = position;
		this.transform.DOMove(position + (vec*distance), speed).OnComplete(StickOnBlock);
		_shootVec = vec;
		_shootActor = actor;
		_damage = damage;

		if(_shootActor is PlayerActor)
		InputManager<Bow>.OnSubPress += Pull;
	}

	protected virtual void StickOnBlock()
	{
		Block block = Define.GetManager<MapManager>().GetBlock(this.transform.position);
		this.transform.parent = block.transform;
		_isStick = true;
	}

	protected virtual void StickActor(Collider other)
	{
		this.transform.DOKill();
		this.transform.parent = other.transform;
		this.transform.localPosition = -_shootVec;
		_stickActor = InGame.GetActor(other.gameObject.GetInstanceID()) as CharacterActor;

		_stickActor.GetAct<CharacterStatAct>().Damage(_damage, _shootActor);
		_isStick = true;
	}

	protected virtual void Pull()
	{
		if (!_canPull)
			return;

		_isStick = false;
		_canPull = false;
		_stickActor.GetAct<CharacterStatAct>().Damage(_damage/2, _shootActor);

		InputManager<Bow>.OnSubPress -= Pull;
		Define.GetManager<ResourceManager>().Destroy(this.gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(_shootActor != other.GetComponent<CharacterActor>())
		{
			StickActor(other);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (_shootActor == other && _shootActor.GetType() == typeof(PlayerActor) && _isStick)
		{
			_canPull = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (_shootActor == other && _shootActor.GetType() == typeof(PlayerActor) && _isStick)
		{
			_canPull = false;
		}
	}
}
