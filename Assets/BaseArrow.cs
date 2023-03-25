using Actors.Characters;
using UnityEngine;
using DG.Tweening;
using Core;
using Managements.Managers;
using Blocks;

public enum ArrowType
{
	BasicArrow
}

public class BaseArrow : MonoBehaviour
{
	private CharacterActor _shootActor;
	private Vector3 _shootVec;
	public virtual void Shoot(Vector3 vec, Vector3 position,CharacterActor actor, float speed)
	{
		this.transform.position = position;
		this.transform.DOMove(position + vec, speed).OnComplete(StickOnBlock);
		_shootVec = vec;
		_shootActor = actor;
	}

	protected virtual void StickOnBlock()
	{
		Block block = Define.GetManager<MapManager>().GetBlock(this.transform.position);
		this.transform.parent = block.transform;
	}

	protected virtual void StickActor(Collider other)
	{
		this.transform.DOKill();
		this.transform.parent = other.transform;
		this.transform.localPosition = -_shootVec;

		//other.GetComponent<CharacterActor>().GetAct<CharacterStat>().
	}

	protected virtual void Pull()
	{

	}

	private void OnTriggerEnter(Collider other)
	{
		if(_shootActor != other.GetComponent<CharacterActor>())
		{
			StickActor(other);
		}
	}
}
