using Actors.Bases;
using Actors.Characters.Player;
using Acts.Base;
using Acts.Characters;
using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAction : InteractionActor
{
	[SerializeField]
	private UnitAnimation _unitAnimation;

	[SerializeField]  
	private float speed = 0f;

	protected override void Awake()
	{
		base.Awake();
		AddAct(_unitAnimation);
	}

	public override void Interact()
	{
		base.Interact();
		_unitAnimation.Play("DiePull");
		_unitAnimation.curClip.SetEventOnFrame(_unitAnimation.curClip.fps - 1, Die);
		InGame.Player.gameObject.SetActive(false);
	}

	private void Die()
	{
		InGame.Player.gameObject.SetActive(true);
		var particle = Define.GetManager<ResourceManager>().Instantiate("DeathParticle", transform);
		particle.transform.localPosition = Vector3.zero;
		var anchorTrm = transform.Find("Anchor");
		var modelTrm = anchorTrm.Find("Model");
		var scale = modelTrm.localScale;
		var rotation = anchorTrm.transform.rotation;
		var particleAnchorTrm = particle.transform.Find("Anchor");
		var particleModelTrm = particleAnchorTrm.Find("Model");
		particleAnchorTrm.rotation = rotation;
		particleModelTrm.localScale = scale;
		particle.transform.SetParent(null);

		gameObject.SetActive(false);
	}
}
