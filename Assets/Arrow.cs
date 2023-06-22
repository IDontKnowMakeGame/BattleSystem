using Actors.Characters;
using UnityEngine;
using DG.Tweening;
using Core;
using Managements.Managers;
using Actors.Characters.Player;
using Actors.Characters.Enemy;
using System.Collections;
using Acts.Characters.Player;
using System;

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

	private bool _isDestroy = false;
	private bool _isEnd = false;

	private PlayerAnimation _playerAnimation;

	private Sequence _seq;

	private Type _type;

	private void OnEnable()
	{
		this.transform.GetComponent<BoxCollider>().enabled = true;
	}

	public void Start()
	{
		if (_shootActor is EnemyActor)
			StartCoroutine(Destroy());

		if (_shootActor is PlayerActor)
			_playerAnimation = _shootActor.GetAct<PlayerAnimation>();
	}
	public static Arrow ShootArrow(Vector3 vec, Vector3 position, CharacterActor actor, float speed, float damage, int distance, bool destroy = false)
	{
		Arrow obj = Define.GetManager<ResourceManager>().Instantiate("Arrow").GetComponent<Arrow>();
		obj.transform.position = position;
		obj.transform.rotation = Quaternion.Euler(VecToRotation(vec));
		obj.Shoot(vec, position, actor, speed, damage, distance, destroy);

		return obj;
	}
	private static Vector3 VecToRotation(Vector3 vec)
	{
		if (vec.z >= Vector3.forward.z)
			return new Vector3(90, 0, 0);
		else if (vec.z <= Vector3.back.z)
			return new Vector3(90, 180f, 0);
		else if (vec.x <= Vector3.left.x)
			return new Vector3(90, -90f, 0);
		else if (vec.x >= Vector3.right.x)
			return new Vector3(90, 90f, 0);
		else
			return Vector3.zero;
	}
	public virtual void Shoot(Vector3 vec, Vector3 position, CharacterActor actor, float speed, float damage, int distance, bool destroy = false)
	{
		var map = Define.GetManager<MapManager>();
		int count = 0;
		for (count = 0; count <= distance; count++)
		{
			if (distance - 1 == count)
				break;

			if (map.GetBlock(position + (vec * count)) != null)
			{
				if (!map.GetBlock(position + (vec * count)).isWalkable && map.GetBlock(position + (vec * count)).ActorOnBlock == null)
				{
					count--;
					_isEnd = true;
					break;
				}
			}
			else
			{
				_isEnd = true;
				break;
			}

		}

		position.y = 1;

		this.transform.position = position;

		float time = count / speed;

		transform.rotation = Quaternion.Euler(VecToRotation(vec));

		_seq = DOTween.Sequence();
		_seq.Append(this.transform.DOMove(position + (vec * count), time).OnComplete(StickOnBlock));

		var rotX = transform.eulerAngles.x;
		var rotY = transform.eulerAngles.y;
		var rotZ = transform.eulerAngles.z;
		_seq.Join(DOTween.To(x => rotX = x, rotX, 150, time).SetEase(Ease.Linear).OnUpdate(() => transform.eulerAngles = new Vector3(rotX, rotY, rotZ)));
		_shootVec = vec;
		_shootActor = actor;
		_damage = damage;
		_isDestroy = destroy;
		if (_shootActor.GetAct<CharacterEquipmentAct>() != null)
			_type = _shootActor.GetAct<CharacterEquipmentAct>().CurrentWeapon.GetType();
		_seq.Play();

		if (_shootActor is PlayerActor)
			InputManager<Bow>.OnSubPress += Pull;

		if (_shootActor is PlayerActor)
			_playerAnimation = _shootActor.GetAct<PlayerAnimation>();
	}

	protected virtual void StickOnBlock()
	{
		_seq.Kill();
		_stickActor = null;
		_isStick = true;

		Define.GetManager<SoundManager>().PlayAtPoint("Sounds/Bow/BowAttack", InGame.Player.Position);
	}
	private void StickOnWall()
	{
		this.transform.position = new Vector3(this.transform.position.x, 1, this.transform.position.z);
		_isStick = true;
		_seq.Kill();

		Define.GetManager<SoundManager>().PlayAtPoint("Sounds/Bow/BowAttack", InGame.Player.Position);
	}

	protected virtual void StickActor(Collider other)
	{
		_seq.Kill();

		_stickActor = InGame.GetActor(other.gameObject.GetInstanceID()) as CharacterActor;

		Vector3 vec = other.transform.position + (this.transform.position - other.transform.position).GetDirection();
		vec.y = 1;
		this.transform.position = vec;
		this.transform.parent = other.transform;
		_stickActor?.GetAct<CharacterStatAct>()?.Damage(_damage, _shootActor);
		_isStick = true;

		Define.GetManager<SoundManager>().PlayAtPoint("Sounds/Bow/BowAttack", InGame.Player.Position);

		if (_isDestroy)
			Define.GetManager<ResourceManager>().Destroy(this.gameObject);

	}

	public void StickReBlock()
	{
		_isStick = true;
		this.transform.position = new Vector3(_stickActor.Position.x, 1, _stickActor.Position.z);
		_stickActor = null;
		Vector3 vec = new Vector3(-150, 0, 0);
		this.transform.localRotation = Quaternion.Euler(vec);
	}


	protected virtual void Pull()
	{
		if (!_canPull)
			return;

		if (_shootActor.HasAnyState()) return;

		if (_type != _shootActor.GetAct<CharacterEquipmentAct>().CurrentWeapon.GetType())
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
		_isStick = false;
		Define.GetManager<ResourceManager>().Destroy(this.gameObject);
	}
	private void OnTriggerEnter(Collider other)
	{
		CharacterActor actor = other.GetComponent<CharacterActor>();
		if ((1 << other.gameObject.layer == LayerMask.GetMask("Wall") && _isEnd) || (1 << other.gameObject.layer == LayerMask.GetMask("InteractionWall")))
		{
			if (_isDestroy)
			{
				_isStick = false;
				_seq.Kill();
				Define.GetManager<ResourceManager>().Destroy(this.gameObject);
			}
			else
				StickOnWall();
		}

		if (actor == null)
			return;

		if (actor is WallObject)
		{
			StickOnWall();
			return;
		}

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

		if (_shootActor == null)
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
		Vector3 vec = InGame.CamDirCheck(_shootVec);
		if (transform.parent == null)
		{
			Debug.Log(_playerAnimation);
			Debug.Log(_playerAnimation.GetClip("GroundPull"));
			_playerAnimation.Play("GroundPull");
		}
		else if (vec == Vector3.left)
		{
			InGame.Player.SpriteTransform.localScale = new Vector3(-2, 1, 1);
			_playerAnimation.Play("HorizontalPull");
		}
		else if (vec == Vector3.right)
		{
			InGame.Player.SpriteTransform.localScale = new Vector3(2, 1, 1);
			_playerAnimation.Play("HorizontalPull");
		}
		else if (vec == Vector3.forward)
		{
			_playerAnimation.Play("UpperPull");
		}
		else if (vec == Vector3.back)
		{
			_playerAnimation.Play("LowerPull");
		}
	}

	public void OnDisable()
	{
		_isStick = false;
		//this.transform.SetParent(null);
		//this.transform.position = Vector3.zero;
	}
}
