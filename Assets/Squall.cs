using Actors.Characters;
using Actors.Characters.Player;
using Acts.Characters.Player;
using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squall : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private bool isDamage = false;
    private float damage = 0f;

	private Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
    {
		rb.velocity = Vector3.down * speed;
	}

	public void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			PlayerActor actor = other.GetComponent<PlayerActor>();

			if (actor != null)
				return;

			if(isDamage)
			{
				actor.GetAct<CharacterStatAct>().Damage(damage, null);
			}
			actor.GetAct<PlayerFlooding>().ChangeflooadCnt(1);
		}

		Define.GetManager<ResourceManager>().Destroy(this.gameObject);
	}
}
