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
		if (InGame.Player.HasState(Actors.Characters.CharacterState.Everything)) return false;

		CharacterActor[] actor = InGame.GetNearCharacterActors(InGame.Player.Position);
		//if () return;
		
		return true;
	}
}
