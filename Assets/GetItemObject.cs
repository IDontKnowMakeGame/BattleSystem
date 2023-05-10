using Actors.Bases;
using Actors.Characters;
using Core;
using Data;
using Managements.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

	public static int count = 0;

	private static List<GetItemObject> obj = new List<GetItemObject>();

	protected override void Awake()
	{
		base.Awake();
		IsUpdatingPosition = false;
		//weight = Random.Range(0, 1000);
		obj.Add(this);
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
		if (count == 1)
			return;

		if (!canInteraction)
			return;

		if (InGame.Player.Position.IsNeighbor(Position) == false && Define.GetManager<MapManager>().GetBlock(Position)?.ActorOnBlock != InGame.Player) return;

		count++;
		canInteraction = false;

		StartCoroutine(GetObjectTime());
	}


	public IEnumerator GetObjectTime()
	{
		yield return new WaitForSeconds(0.1f);
		count = 0;
		Define.GetManager<DataManager>().AddItemInInventory(_id, _count);
		Define.GetManager<ResourceManager>().Destroy(this.gameObject);
	}
}
