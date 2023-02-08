using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public enum ArrowType
{
	BasicArrow,
}
public class BaseArrow : MonoBehaviour
{
	[SerializeField]
	private ArrowType _arrowType;

	private Dictionary<ArrowType, Arrow> _arrows;

	public Vector3 dir;
	public Vector3 pos;
	public float speed;

	private void Start()
	{
		this.gameObject.transform.DOMove(pos + (dir * 5), speed);
	}
}
