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
		///if (InGame.Player.Position.IsNeighbor(InGame.Player.Position) == false) return;
		
		return true;
	}
}
