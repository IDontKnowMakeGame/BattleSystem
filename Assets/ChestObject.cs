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
	[SerializeField]
	private ParticleSystem _particle;

	private PlayableDirector direction;

	protected override void Start()
	{
		_itemObject.canInteraction = false;
		direction = GetComponent<PlayableDirector>();
		var particle = _particle.main;
		particle.loop = true;
        //Define.GetManager<DataManager>()

        if (Define.GetManager<DataManager>().IsOpenChest(int.Parse(gameObject.name), DataManager.MapData_.currentFloor))
        {
            _itemObject.gameObject.SetActive(false);
            _particle.gameObject.SetActive(false);
            direction.Play();
            isOpen = true;
			RemoveInteration();
            return;
        }

    }
    public override void Interact()
	{
		if (isOpen) return;
        if (InGame.Player.Position.IsNeighbor(Position) == false) return;
		base.Interact();
		var particle = _particle.main;
		particle.loop = false;
		//_particle.gameObject.SetActive(false);
		direction.Play();
		isOpen = true;

		Define.GetManager<DataManager>().OpenChest(int.Parse(gameObject.name));
		Define.GetManager<SoundManager>().Play("Sounds/Effect/ChestOpen", Define.Sound.Effect);
		characterDetect.EnterDetect -= ShowInteration;
        characterDetect.ExitDetect -= HideInteration;
		HideInteration(Vector2.zero);
    }

	public void ChestEnd()
	{
		_itemObject.canInteraction = true;
	}
}
