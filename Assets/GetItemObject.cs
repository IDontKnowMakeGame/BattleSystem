using Core;
using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemObject : InteractionActor
{
	[SerializeField]
	private ItemID _id;

	[SerializeField]
	private int _count;

	[SerializeField]
	private bool _isWeapon;

	public bool canInteraction = true;

	protected override void Init()
	{

	}

	public void Init(ItemID id, int count, bool weapon)
	{
		_id = id;
		_count = count;
		_isWeapon = weapon;
	}

	public override void Interact()
	{
		if (!canInteraction)
			return;
		//여기다가 추가
		Define.GetManager<ResourceManager>().Destroy(this.gameObject);
	}
}
