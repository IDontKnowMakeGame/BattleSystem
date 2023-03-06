using Core;
using System.Collections;
using System.Collections.Generic;
using Units.Base.Interactable;
using UnityEngine;

public class AbandonedAngel : InteractableUnitBase
{
    [SerializeField]
    private ItemStoreTableSO _itemTable;
    public override void Interact()
    {
        if (IsInteracted) return;
        if (DetectCondition.Invoke(Position))
        {
            // TODO: Interact
            Define.GetManager<UIManager>().ShowItemStore(_itemTable);
            Debug.Log("Interact");
        }
    }
    protected override void OnDetect()
    {
        if (IsDetected) return;
        if (DetectCondition.Invoke(Position) == false) return;
        IsDetected = true;
        Debug.Log("Detect");
    }

    protected override void OnLostDetect()
    {
        if (IsDetected == false) return;
        if (DetectCondition.Invoke(Position)) return;
        IsDetected = false;
        Debug.Log("Lost Detect");
    }
}
