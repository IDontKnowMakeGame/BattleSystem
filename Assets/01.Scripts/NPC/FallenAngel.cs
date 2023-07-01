using Actors.Characters.NPC;
using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using Tool.Data.Json;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class FallenAngel : NPCActor
{
    [SerializeField]
    private DialogueData[] dialogueList;

    
    private QuestData questData;

    private Dictionary<QuestName, Action> castReadyQuestActions = new Dictionary<QuestName, Action>();
    private Dictionary<QuestName, Action> castClearQuestActions = new Dictionary<QuestName, Action>();

    protected override void Init()
    {
        base.Init();
        
        questData = JsonManager.LoadJsonFile<QuestData>(Application.streamingAssetsPath+"/SAVE/NPC/Quest",GetType().Name);

        castReadyQuestActions.Add(QuestName.FirstFloorBossKill, AcceptBossKillQuestBtn);

        castClearQuestActions.Add(QuestName.FirstFloorBossKill, ClearBossKillQuestBtn);

        SaveQuestData();
    }
    public void SaveQuestData()
    {
        string json = JsonManager.ObjectToJson(questData);
        JsonManager.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/NPC/Quest", GetType().Name, json);
    }

    public override void Interact()
    {
        if (InGame.Player.Position.IsNeighbor(Position) == false) return;

        if (questData.firstTalked)
            Talking();
        else
            FirstTalking();
    }

    public void Talking()
    {
        Debug.Log(gameObject.name);
        ReadyBtnQuest();
        UIManager.Instance.Dialog.AddChoiceBox("���� ��ȭ", StoreBtn);
        //UIManager.Instance.Dialog.AddChoiceBox("���ư���", BackBtn);

        UIManager.Instance.Dialog.StartListeningDialog(dialogueList[0]);
    }
    public void FirstTalking()
    {
        Debug.Log(gameObject.name);
        ReadyBtnQuest();
        UIManager.Instance.Dialog.AddChoiceBox("���� ��ȭ", StoreBtn);

        UIManager.Instance.Dialog.StartListeningDialog(dialogueList[3]);

        questData.firstTalked = true;
        SaveQuestData();
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
                if (castClearQuestActions.ContainsKey(info.questName) == false)
                    Debug.LogError($"Not Have Dictionary info QuestInfo : {info.questName}");

                UIManager.Instance.Dialog.AddChoiceBox(info.clearBtnName, castClearQuestActions[info.questName],true);
            }
            //ready Quest
            if (Define.GetManager<DataManager>().IsReadyQuest(info.questName) == true)
            {
                Debug.Log($"ready Quest Name {info.questName}");
                if (castReadyQuestActions.ContainsKey(info.questName) == false)
                    Debug.LogError($"Not Have Dictionary info QuestInfo : {info.questName}");

                UIManager.Instance.Dialog.AddChoiceBox(info.btnName, castReadyQuestActions[info.questName],true);
            }
        }
    }

    public void StoreBtn()
    {
        UIManager.Instance.Smithy.Show();
    }

    public void BackBtn()
    {
        UIManager.Instance.Dialog.FlagDialogue(false);
    }

    public void AcceptBossKillQuestBtn()
    {
        UIManager.Instance.Dialog.StartListeningDialog(dialogueList[1]);
        Define.GetManager<DataManager>().OpenQuest(QuestName.FirstFloorBossKill);

        Define.GetManager<DataManager>().AddItemInInventory(questData.quests[0].questGiveItem[0], 1);
    }

    public void ClearBossKillQuestBtn()
    {
        UIManager.Instance.Dialog.StartListeningDialog(dialogueList[2]);
        Define.GetManager<DataManager>().ClearQuest(QuestName.FirstFloorBossKill);

        Define.GetManager<DataManager>().AddItemInInventory(questData.quests[0].rewords[0],1);
    }
}
