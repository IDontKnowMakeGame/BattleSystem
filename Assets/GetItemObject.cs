using Actors.Bases;
using Actors.Characters;
using Core;
using Data;
using Managements.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

	private IEnumerator cor;

	protected override void Awake()
	{
		base.Awake();
		IsUpdatingPosition = false;
		//weight = Random.Range(0, 1000);
		obj.Add(this);

		characterDetect.EnterDetect += ShowInteration;
		characterDetect.ExitDetect += HideInteration;

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
	public void ShowInteration(Vector3 vec)
	{
		UIManager.Instance.InGame.ShowInteraction();
	}
	public void HideInteration(Vector3 vec)
	{
		UIManager.Instance.InGame.HideInteraction();
	}
	public override void Interact()
	{
		if (count == 1)
			return;

		if (!canInteraction)
			return;

		if (InGame.Player == null) return;
		if (Define.GetManager<MapManager>() == null) return;
		
		if (InGame.Player.Position.IsNeighbor(Position) == false && Define.GetManager<MapManager>().GetBlock(Position)?.ActorOnBlock != InGame.Player) return;

		count++;
		canInteraction = false;
		characterDetect.EnterDetect -= ShowInteration;
		characterDetect.ExitDetect -= HideInteration;
		HideInteration(Vector2.zero);

		cor = GetObjectTime();

		StartCoroutine(cor);
	}


	public IEnumerator GetObjectTime()
	{
		yield return new WaitForSeconds(0.1f);
		count = 0;
		if (Define.GetManager<DataManager>() != null)
		{
			Define.GetManager<DataManager>().AddItemInInventory(_id, _count);
			Define.GetManager<ResourceManager>().Destroy(this.gameObject);
		}
	}

	protected override void OnDisable()
	{
		if (cor != null)
			StopCoroutine(cor);
	}
}
