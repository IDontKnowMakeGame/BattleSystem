using Actors.Characters;
using UnityEngine;
using DG.Tweening;
using Core;
using Managements.Managers;
using Actors.Characters.Player;
using Actors.Characters.Enemy;
using System.Collections;

public class Arrow : MonoBehaviour
{
	[SerializeField]
	private float DestroyTime;

	private CharacterActor _shootActor;
	private CharacterActor _stickActor;

	private Vector3 _shootVec;
	private float _damage;
	private bool _isStick = false;
	private bool _canPull = false;

	public void Start()
	{
		if (_shootActor is EnemyActor)
			StartCoroutine(Destroy());
	}
	public static void ShootArrow(Vector3 vec, Vector3 position, CharacterActor actor, float speed, float damage, int distance)
	{
		Arrow obj = Define.GetManager<ResourceManager>().Instantiate("Arrow").GetComponent<Arrow>();
		obj.Shoot(vec, position, actor, speed, damage, distance);
	}
	public virtual void Shoot(Vector3 vec, Vector3 position, CharacterActor actor, float speed, float damage, int distance)
	{
		position.y = 1;

		int count = 0;
		this.transform.position = position;

		var map = Define.GetManager<MapManager>();

		for(count = 0; count < distance; count++)
		{
			if (map.GetBlock((position - Vector3.up) + (vec * count)) == null)
			{
				count -= 1;
				break;
			}
		}

		float time = count / speed;

		this.transform.DOMove(position + (vec * count), time).OnComplete(StickOnBlock);
		_shootVec = vec;
		_shootActor = actor;
		_damage = damage;

		if (_shootActor is PlayerActor)
			InputManager<Bow>.OnSubPress += Pull;
	}

	protected virtual void StickOnBlock()
	{
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
		Bow bow = _shootActor.GetAct<PlayerEquipment>().CurrentWeapon as Bow;
		bow.isShoot = false;

		_stickActor?.GetAct<CharacterStatAct>().Damage(_damage / 2, _shootActor);

		InputManager<Bow>.OnSubPress -= Pull;
		Define.GetManager<ResourceManager>().Destroy(this.gameObject);
	}

	private IEnumerator Destroy()
	{
		yield return new WaitForSeconds(DestroyTime);
		Define.GetManager<ResourceManager>().Destroy(this.gameObject);
	}
	private void OnTriggerEnter(Collider other)
	{
		CharacterActor actor = other.GetComponent<CharacterActor>();
		if (actor == null)
			return;

		if (_shootActor.UUID != actor.UUID && !_isStick)
		{
			StickActor(other);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		CharacterActor actor = other.GetComponent<CharacterActor>();
		if (actor == null)
			return;

		if (_shootActor.UUID == actor.UUID && _isStick)
		{
			_canPull = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		CharacterActor actor = other.GetComponent<CharacterActor>();
		if (actor == null)
			return;

		if (_shootActor.UUID == actor.UUID && _isStick)
		{
			_canPull = false;
		}
	}
}
