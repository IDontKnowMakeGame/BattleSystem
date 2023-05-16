using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Managements.Managers;
using Actors.Characters;
using System;
using Acts.Characters;

public class InteractionActor : CharacterActor
{
    protected override void Init()
    {
        base.Init();
        AddAct<CharacterDetect>();
        InputManager<Weapon>.OnInteractionPress += Interact;
    }

    public virtual void Interact()
    {
        if (InGame.Player.Position.IsNeighbor(Position) == false) return;

        //TODO : 상호작용
        InputManager<Weapon>.OnInteractionPress -= Interact;
	}
}
