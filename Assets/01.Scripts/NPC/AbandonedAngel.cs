using Actors.Characters.NPC;
using Core;
using System;
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

    private Dictionary<QuestName,Action> castReadyQuestActions = new Dictionary<QuestName,Action>();
    private Dictionary<QuestName, Action> castClearQuestActions = new Dictionary<QuestName, Action>();

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
       
        Talking();
    }

    public void Talking()
    {
        Debug.Log(gameObject.name);
        UIManager.Instance.Dialog.StartListeningDialog(dialogueList[0]);

        ReadyBtnQuest();

        UIManager.Instance.Dialog.AddChoiceBox("상점", StoreBtn);
        UIManager.Instance.Dialog.AddChoiceBox("돌아가기", BackBtn);
    }
    private void ReadyBtnQuest()
    {
        
        foreach (QuestInfo info in questData.quests)
        {
            Debug.Log($"Quest Name {info.questName}");
            //clear Quest
            if (Define.GetManager<DataManager>().IsReadyClearQuest(info.questName) == true)
            {
                Debug.Log($"Clear Quest Name {info.questName}");
                if (castClearQuestActions.ContainsKey(info.questName))
                    Debug.LogError($"Not Have Dictionary info QuestInfo : {info.questName}");

                UIManager.Instance.Dialog.AddChoiceBox(info.btnName, castClearQuestActions[info.questName]);
            }
            //ready Quest
            if (Define.GetManager<DataManager>().IsReadyQuest(info.questName) == true)
            {
                Debug.Log($"ready Quest Name {info.questName}");
                if (castReadyQuestActions.ContainsKey(info.questName))
                    Debug.LogError($"Not Have Dictionary info QuestInfo : {info.questName}");

                UIManager.Instance.Dialog.AddChoiceBox(info.btnName, castReadyQuestActions[info.questName]);
            }

        }
    }

    #region Btn
    public void StoreBtn()
    {
        UIManager.Instance.ItemStore.ShowItemStore(table);
    }
         
    public void BackBtn()
    {
        UIManager.Instance.Dialog.FlagDialogue(false);
    }

    #endregion
}
