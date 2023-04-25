using Actors.Characters.NPC;
using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbandonedAngel : NPCActor
{
    [SerializeField]
    private ItemStoreTableSO table;
    [SerializeField]
    private DialogueData[] dialogueList;

    public override void Interact()
    {
        if (InGame.Player.Position.IsNeighbor(Position) == false) return;
        base.Interact();
       
        Talking();
    }

    public void Talking()
    {
        Debug.Log(gameObject.name);
        UIManager.Instance.Dialog.StartListeningDialog(dialogueList[0]);

        UIManager.Instance.Dialog.AddChoiceBox("상점", StoreBtn);
        UIManager.Instance.Dialog.AddChoiceBox("돌아가기", BackBtn);
    }

    public void StoreBtn()
    {
        UIManager.Instance.ItemStore.ShowItemStore(table);
    }
         
    public void BackBtn()
    {
        UIManager.Instance.Dialog.FlagDialogue(false);
    }
}
