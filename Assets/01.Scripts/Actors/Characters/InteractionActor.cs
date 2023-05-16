using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Managements.Managers;
using Actors.Characters;
using System;
using Acts.Characters;
using UnityEngine.Serialization;

public class InteractionActor : CharacterActor
{
    [SerializeField] protected CharacterDetect characterDetect;
    protected override void Init()
    {
        base.Init();
        characterDetect = AddAct<CharacterDetect>();
        InputManager<Weapon>.OnInteractionPress += Interact;
    }

    public virtual void Interact()
    {
        if (InGame.Player.Position.IsNeighbor(Position) == false) return;

        //TODO : 상호작용
        InputManager<Weapon>.OnInteractionPress -= Interact;
	}
}
