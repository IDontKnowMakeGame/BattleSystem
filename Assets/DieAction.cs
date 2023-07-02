using Actors.Bases;
using Actors.Characters.Player;
using Acts.Base;
using Acts.Characters;
using Acts.Characters.Enemy;
using ArrayExtensions;
using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

[Serializable]
public class TextureSprite
{
	public string name;
	public Texture tex;
}

public class DieAction : InteractionActor
{
	[SerializeField]
	private EnemyAnimation _unitAnimation = new EnemyAnimation();

	[SerializeField]
	private float speed = 0f;

	private string objName;
	private CharacterRender _render;

	[SerializeField]
	private TextureSprite[] tx;

	protected override void Init()
	{
		base.Init();
		AddAct(_unitAnimation);
		_render = this.GetAct<CharacterRender>();
	}

	public void InitDieObj(string name)
	{
		objName = name;

		Material me = _render.Renderer.material;
		foreach(TextureSprite sprite in tx)
		{
			if(sprite.name == name)
			{
				me.SetTexture("_MainTex", sprite.tex);
				me.SetVector("_Tiling", new Vector2(1, 1f));
				break;
			}
		}
		Debug.Log(me.GetTexture("_MainTex").name);
		Debug.Log(_unitAnimation.GetClip(objName + "Idle").name);
	}

	public override void Interact()
	{
		if (InGame.Player.Position.IsNeighbor(Position) == false) return;
		base.Interact();
		Define.GetManager<EventManager>().TriggerEvent(EventFlag.PlayTimeLine, new EventParam() { stringParam = objName });
		_unitAnimation.Play(objName);
		Debug.Log(objName);
		Define.GetManager<SoundManager>().Play($"Boss/{objName}/" + objName, Define.Sound.Effect, 1);
		_unitAnimation.GetClip(objName)?.SetEventOnFrame(_unitAnimation.GetClip(objName).fps -1, Die);
		if (_unitAnimation.GetClip(objName) == null)
			return;

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
