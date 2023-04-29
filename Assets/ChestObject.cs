using Core;
using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ChestObject : InteractionActor
{
	private bool isOpen = false;

	[SerializeField]
	private GetItemObject _itemObject;

	private PlayableDirector direction;

	protected override void Start()
	{
		_itemObject.canInteraction = false;
		direction = GetComponent<PlayableDirector>();
	}

	public override void Interact()
	{
		base.Interact();
		if (isOpen)
			return;

		Debug.Log("open");
		direction.Play();
		isOpen = true;
	}

	public void ChestEnd()
	{
		_itemObject.canInteraction = true;
	}
}
