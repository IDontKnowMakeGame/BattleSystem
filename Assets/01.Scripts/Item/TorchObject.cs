using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors.Characters.NPC;
using Core;
using System;
using Acts.Characters;

public class TorchObject : InteractionActor
{
    [SerializeField] private GameObject torchLight;

    CharacterDetect detect;

    private bool isOn = false;
    protected override void Start()
    {
        Floor floor = DataManager.MapData_.currentFloor;
        isOn = Define.GetManager<DataManager>().IsOnCrital(Int32.Parse(gameObject.name), floor);
        if (isOn)
        {
            torchLight.SetActive(true);
            return;
        }

        detect = GetAct<CharacterDetect>();
        detect.EnterDetect += ShowDetect;
        detect.ExitDetect += HideDetect;
    }
    public void ShowDetect(Vector3 vec)
    {
        Debug.Log($"{gameObject.name}Enter Cristal Zone");
        UIManager.Instance.InGame.ShowInteraction();
    }
    public void HideDetect(Vector3 vec)
    {
        UIManager.Instance.InGame.HideInteraction();
    }
    public override void Interact()
    {
        if (InGame.Player.Position.IsNeighbor(Position) == false || isOn) return;

        base.Interact();
        torchLight.SetActive(true);
        Define.GetManager<DataManager>().OnCrital(Int32.Parse(gameObject.name));
        detect.EnterDetect -= ShowDetect;
        detect.ExitDetect -= HideDetect;
        HideDetect(Vector3.zero);
    }
}
