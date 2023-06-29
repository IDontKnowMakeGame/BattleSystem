using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Managements.Managers;
using Actors.Characters;
using System;
using Acts.Characters;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class InteractionActor : CharacterActor
{
    [SerializeField] protected CharacterDetect characterDetect;
    [SerializeField] protected bool canInteract = true;
    [SerializeField] protected UnityEvent onInteract = new UnityEvent();
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
        if (UIManager.Instance != null)
            UIManager.Instance.InGame.HideInteraction();
    }
    public void HideInteration(Vector3 vec)
    {
        if(UIManager.Instance != null)
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
        
        InputManager<Weapon>.OnInteractionPress -= Interact;
        if (canInteract)
        {
            onInteract?.Invoke();
        }
        //TODO : 상호작용
	}
}
