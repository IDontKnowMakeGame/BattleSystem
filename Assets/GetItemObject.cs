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
		if (InGame.Player.Position.IsNeighbor(Position) == false) return;

		Define.GetManager<DataManager>().AddItemInInventory(_id, _count);
		Define.GetManager<ResourceManager>().Destroy(this.gameObject);
	}
}
