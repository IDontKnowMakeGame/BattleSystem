using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Base.Interactable;

public class InteractableTorch : InteractableUnitBase
{
    GameObject torchLight;

    private bool torchOn = false;

    public override void Interact()
    {
        if (IsInteracted || torchOn) return;

        if (DetectCondition.Invoke(Position))
        {
            torchLight = Core.Define.GetManager<ResourceManagers>().Instantiate("Torchlight");
            torchLight.transform.position = SpawnPos;
            torchOn = true;
        }
    }

    protected override void OnDetect()
    {

    }

    protected override void OnLostDetect()
    {

    }
}
