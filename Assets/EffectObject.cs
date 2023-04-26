using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : MonoBehaviour
{
	[NonSerialized]
	public GameObject targetObject;

	[SerializeField]
	private float minSpeed;

	[SerializeField]
	private float maxSpeed;

	public void Init(GameObject obj)
	{
		targetObject = obj;
	}
}
