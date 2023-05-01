using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatEffect : MonoBehaviour
{
	[SerializeField]
	private GameObject targetObject;

	[SerializeField]
	private GameObject effectObject;

	[SerializeField]
	private int maxParticle;

	[SerializeField]
	private float sizeMin;

	[SerializeField]
	private float sizeMax;

	public void Init(GameObject obj)
	{
		targetObject = obj;
		Instantiate();
	}

	private void Instantiate()
	{
		for(int i =0; i<maxParticle; i++)
		{
			Vector3 vec = this.transform.position + (Random.insideUnitSphere * 3) + Vector3.up;
			GameObject obj = GameObject.Instantiate(effectObject);
			obj.transform.position = vec;
			float size = Random.Range(sizeMin, sizeMax);
			obj.transform.localScale = new Vector3(size, size, size);
			obj.GetComponentInChildren<EffectObject>().Init(targetObject);
		}
	}
}
