using Core;
using Data;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ChestObject : InteractionActor
{
	private bool isOpen = false;

	[SerializeField]
	private GetItemObject _itemObject;

	protected override void Init()
	{
		
	}

	protected override void Start()
	{
		_itemObject.canInteraction = false;
	}

	public override void Interact()
	{
		if (isOpen)
			return;

		isOpen = true;
	}

	public void ChestEnd()
	{
		_itemObject.canInteraction = true;
	}
}
