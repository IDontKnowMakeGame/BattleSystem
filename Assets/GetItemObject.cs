using Core;
using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GetItemInfo
{
	public ItemID _id;
	public int _count;
	public bool _isWeapon;
}
public class GetItemObject : InteractionActor
{
	[SerializeField]
	private ItemID _id;

	[SerializeField]
	private int _count;

	[SerializeField]
	private bool _isWeapon;

	public bool canInteraction = true;

	protected override void Awake()
	{
		base.Awake();
		IsUpdatingPosition = false;
	}

	public void Init(ItemID id, int count, bool weapon)
	{
		_id = id;
		_count = count;
		_isWeapon = weapon;
		canInteraction = true;
	}
	public void Init(GetItemInfo info)
	{
		_id = info._id;
		_count = info._count;
		_isWeapon = info._isWeapon;
		canInteraction = true;
	}

	public override void Interact()
	{
		if (!canInteraction)
			return;
		if (InGame.Player.Position.IsNeighbor(Position) == false) return;

		base.Interact();

		Define.GetManager<DataManager>().AddItemInInventory(_id, _count);
		Define.GetManager<ResourceManager>().Destroy(this.gameObject);
	}
}
