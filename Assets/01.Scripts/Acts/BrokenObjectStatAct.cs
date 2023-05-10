using Actors.Bases;
using Actors.Characters.Player;
using Core;
using Managements;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;

[Serializable]
public class BrokenObjectStatAct : CharacterStatAct
{
	[SerializeField]
	private GameObject _brokenObject;
	private Vector3 dir = Vector3.zero;

	public override void Damage(float damage, Actor actor)
	{
		ChangeStat.hp -= damage - (damage * (Half / 100));
		if (ChangeStat.hp <= 0)
		{
			Die();
			return;
		}
		GameObject obj = GameManagement.Instance.GetManager<ResourceManager>().Instantiate("BrokenObjectAttackParticle");
		obj.transform.position = ThisActor.transform.position;
		obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
		dir = (actor.transform.position - obj.transform.position).GetDirection();
	}

	public override void Die()
	{
		ThisActor.GetComponentInChildren<Arrow>()?.StickReBlock();
		ThisActor.transform.DetachChildren();
		GameObject obj = GameObject.Instantiate(_brokenObject);
		obj.transform.position = ThisActor.transform.position;
		obj.GetComponent<Broken>().Brokens(dir);
		GameManagement.Instance.GetManager<ResourceManager>().Destroy(ThisActor.gameObject);
	}
}
