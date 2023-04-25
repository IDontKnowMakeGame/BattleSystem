using Actors.Characters.NPC;
using Core;
using System.Collections;
using System.Collections.Generic;
using Tool.Data.Json;
using UnityEngine;

public class AbandonedAngel : NPCActor
{
    [SerializeField]
    private ItemStoreTableSO table;
    [SerializeField]
    private DialogueData[] dialogueList;

    private QuestData questData;

    protected override void Init()
    {
        base.Init();

        questData = JsonManager.LoadJsonFile<QuestData>(Application.streamingAssetsPath + "/SAVE/NPC/Quest", GetType().Name);
        SaveQuestData();
    }

    public void SaveQuestData()
    {
        string json = JsonManager.ObjectToJson(questData);
        JsonManager.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/NPC/Quest", GetType().Name,json);
    }
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
