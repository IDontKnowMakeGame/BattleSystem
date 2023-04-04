using Actors.Characters;
using UnityEngine;
using DG.Tweening;
using Core;
using Managements.Managers;
using Actors.Characters.Player;
using Actors.Characters.Enemy;
using System.Collections;
using Acts.Characters.Player;

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

	private PlayerAnimation _playerAnimation;

	public void Start()
	{
		if (_shootActor is EnemyActor)
			StartCoroutine(Destroy());

		if (_shootActor is PlayerActor)
			_playerAnimation = InGame.Player.GetAct<PlayerAnimation>();
	}
	public static void ShootArrow(Vector3 vec, Vector3 position, CharacterActor actor, float speed, float damage, int distance)
	{
		Arrow obj = Define.GetManager<ResourceManager>().Instantiate("Arrow").GetComponent<Arrow>();
		obj.transform.rotation = Quaternion.Euler(VecToRotation(vec));
		obj.Shoot(vec, position, actor, speed, damage, distance);
	}
	private static Vector3 VecToRotation(Vector3 vec)
	{
		if (vec == Vector3.forward)
			return new Vector3(90, 0, 0);
		else if (vec == Vector3.back)
			return new Vector3(90, 180f, 0);
		else if (vec == Vector3.left)
			return new Vector3(90, -90f, 0);
		else if (vec == Vector3.right)
			return new Vector3(90, 90f, 0);
		else
			return Vector3.zero;
	}
	public virtual void Shoot(Vector3 vec, Vector3 position, CharacterActor actor, float speed, float damage, int distance)
	{
		var map = Define.GetManager<MapManager>();
		int count = 0;
		Debug.Log(position);
		for (count = 0; count < distance; count++)
		{
			if (map.GetBlock(position + (vec * count)) == null || (!map.GetBlock(position + (vec * count)).isWalkable && map.GetBlock(position + (vec * count)).ActorOnBlock == null))
			{
				count -= 1;
				break;
			}
		}
		position.y = 1;

		this.transform.position = position;

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
		Quaternion quater = this.transform.localRotation;
		Vector3 vec = quater.eulerAngles;
		vec.x = 150;
		this.transform.rotation = Quaternion.Euler(vec);
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

		if (_shootActor.HasState(CharacterState.Move))
			return;

		_isStick = false;
		_canPull = false;
		Bow bow = _shootActor.GetAct<PlayerEquipment>().CurrentWeapon as Bow;
		bow.isShoot = false;

		_stickActor?.GetAct<CharacterStatAct>().Damage(_damage / 2, _shootActor);

		PullAnimation();

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

	private void PullAnimation()
	{
		Vector3 dir = (transform.position - InGame.Player.Position).normalized;
		dir.y = 0;

		Debug.Log(dir);

		if (dir == Vector3.left)
		{
			InGame.Player.SpriteTransform.localScale = new Vector3(-2, 1, 1);
			_playerAnimation.Play("VerticalPull");
		}
		else if (dir == Vector3.right)
		{
			InGame.Player.SpriteTransform.localScale = new Vector3(2, 1, 1);
			_playerAnimation.Play("VerticalPull");
		}
		else if (dir == Vector3.forward)
		{
			_playerAnimation.Play("UpperPull");
		}
		else
		{
			_playerAnimation.Play("LowerPull");
		}
	}
}
