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

	public void Init(GameObject obj)
	{
		targetObject = obj;
	}

	void Update()
    {
		float random = Random.Range(minSpeed, maxSpeed);
		if (targetObject == null)
			Define.GetManager<ResourceManager>().Destroy(this.gameObject);

		this.transform.position = Vector3.Slerp(this.transform.position, targetObject.transform.position, Time.deltaTime * random);
		this.transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.zero, Time.deltaTime * random);

		var dir = targetObject.transform.position - this.transform.position;
		var dz = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
		var euler = Quaternion.Euler(0, 0, dz);
		this.transform.rotation = euler;
	}
}
