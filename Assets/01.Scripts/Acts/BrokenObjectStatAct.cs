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

	public override void Damage(float damage, Actor actor)
	{
		ChangeStat.hp -= damage - (damage * (Half / 100));
		if (ChangeStat.hp <= 0)
		{
			Die();
		}
	}

	public override void Die()
	{
		ThisActor.GetComponentInChildren<Arrow>()?.StickReBlock();
		ThisActor.transform.DetachChildren();
		GameObject obj = GameObject.Instantiate(_brokenObject);
		obj.transform.position = ThisActor.transform.position + Vector3.up;
		GameManagement.Instance.GetManager<ResourceManager>().Destroy(ThisActor.gameObject);
	}
}
