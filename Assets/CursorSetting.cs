using Actors.Characters;
using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum CusorDirEnum
{
	Left,
	Right,
	Up,
	Down
}
public class CursorSetting : MonoBehaviour
{
	[SerializeField]
	private DecalProjector[] _decal;

	[SerializeField]
	private CharacterActor _actor;

	private void Start()
	{
		foreach (var enumdir in Enum.GetValues(typeof(CusorDirEnum)))
		{
			_decal[(int)enumdir].gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (_actor.HasState(CharacterState.Attack) || _actor.HasState(CharacterState.Hold) || _actor.HasState(CharacterState.Skill))
			return;

		Vector3 vec = Input.mousePosition;
		Vector3 dir = InGame.CamDirCheck(Weapon.DirReturn(vec));

		_decal[(int)DirReturn(dir)].gameObject.SetActive(true);

		foreach (var enumdir in Enum.GetValues(typeof(CusorDirEnum)))
		{
			if (DirReturn(dir) == (CusorDirEnum)enumdir)
				continue;

			_decal[(int)enumdir].gameObject.SetActive(false);
		}
	}

	private CusorDirEnum DirReturn(Vector3 vec)
	{
		if (Mathf.Abs(vec.x) > 0)
			return vec.x > 0 ? CusorDirEnum.Right : CusorDirEnum.Left;
		else if(Mathf.Abs(vec.z) > 0)
			return vec.z > 0 ? CusorDirEnum.Up : CusorDirEnum.Down;

		return CusorDirEnum.Up;
	}
}
