using Actors.Characters;
using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Pick : UseAbleItem
{
	public override bool UseItem()
	{
		Debug.Log("Use");
		if (InGame.Player.HasState(Actors.Characters.CharacterState.Everything)) return false;

		CharacterActor[] actor = InGame.GetNearCharacterActors(InGame.Player.Position);
		foreach (CharacterActor actor2 in actor)
		{
			if (actor2?.GetComponent<IPickable>() != null)
			{
				Debug.Log("UsePick");
				actor2.GetComponent<IPickable>().Mining();
				Define.GetManager<SoundManager>().PlayAtPoint("Assets/Resources/Sounds/Effect/Broken.mp3", InGame.Player.Position);
				return true;
			}
		}

		return false;
	}
}
