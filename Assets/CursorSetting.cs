using Actors.Characters;
using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CursorSetting : MonoBehaviour
{
	[SerializeField]
	private DecalProjector _decal;

	[SerializeField]
	private CharacterActor _actor;

	private void Update()
	{
		if (_actor.HasState(CharacterState.Attack) || _actor.HasState(CharacterState.Hold) || _actor.HasState(CharacterState.Skill))
			return;

		Vector3 vec = Input.mousePosition;
		Vector3 dir = InGame.CamDirCheck(Weapon.DirReturn(vec));

		if (Mathf.Abs(dir.x) > 0)
			_decal.gameObject.transform.localRotation = Quaternion.Euler(0, 0, dir.x > 0 ? 0 : 180);
		else if(Mathf.Abs(dir.z) > 0)
			_decal.gameObject.transform.localRotation = Quaternion.Euler(0, 0, dir.z > 0 ? 90 : 270);
		//_decal[(int)DirReturn(dir)].gameObject.SetActive(true);
	}
}
