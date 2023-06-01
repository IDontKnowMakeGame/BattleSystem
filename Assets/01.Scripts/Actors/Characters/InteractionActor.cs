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
        characterDetect.EnterDetect += ShowInteration;
        characterDetect.ExitDetect += HideInteration;

        InputManager<Weapon>.OnInteractionPress += Interact;
        InputManager<Weapon>.OnInteractionPress += HideInteration;
    }
    public void ShowInteration(Vector3 vec)
    {
        UIManager.Instance.InGame.ShowInteraction();
    }
    public void HideInteration()
    {
        UIManager.Instance.InGame.HideInteraction();
    }
    public void HideInteration(Vector3 vec)
    {
        UIManager.Instance.InGame.HideInteraction();
    }

    public void RemoveInteration()
    {
        characterDetect.EnterDetect -= ShowInteration;
        characterDetect.ExitDetect -= HideInteration;
        HideInteration(Vector3.zero);
    }

    public virtual void Interact()
    {
        if (InGame.Player.Position.IsNeighbor(Position) == false) return;

        //TODO : 상호작용
        InputManager<Weapon>.OnInteractionPress -= Interact;
	}
}
