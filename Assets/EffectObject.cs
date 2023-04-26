using Core;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EffectObject : MonoBehaviour
{
	[NonSerialized]
	public GameObject targetObject;

	[SerializeField]
	private float minSpeed;

	[SerializeField]
	private float maxSpeed;

	public GameObject obj => targetObject;

	public void Init(GameObject obj)
	{
		targetObject = obj;
	}

	public void Update()
	{
		float random = Random.Range(minSpeed, maxSpeed);
		if (targetObject == null)
			Define.GetManager<ResourceManager>().Destroy(this.gameObject);

		this.transform.position = Vector3.Slerp(this.transform.position, targetObject.transform.position, Time.deltaTime * random);
		this.transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.zero, Time.deltaTime * random);
	}

	public void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == targetObject)
		{
			Define.GetManager<ResourceManager>().Destroy(this.gameObject);
		}
	}
}
