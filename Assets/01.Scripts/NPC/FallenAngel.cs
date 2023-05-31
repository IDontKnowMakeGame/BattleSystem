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

        Talking();
    }

    public void Talking()
    {
        Debug.Log(gameObject.name);

        UIManager.Instance.Dialog.AddChoiceBox("무기 강화", StoreBtn);
        //UIManager.Instance.Dialog.AddChoiceBox("돌아가기", BackBtn);

        ReadyBtnQuest();

        UIManager.Instance.Dialog.StartListeningDialog(dialogueList[0]);
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

                UIManager.Instance.Dialog.AddChoiceBox(info.clearBtnName, castClearQuestActions[info.questName]);
            }
            //ready Quest
            if (Define.GetManager<DataManager>().IsReadyQuest(info.questName) == true)
            {
                Debug.Log($"ready Quest Name {info.questName}");
                if (castReadyQuestActions.ContainsKey(info.questName) == false)
                    Debug.LogError($"Not Have Dictionary info QuestInfo : {info.questName}");

                UIManager.Instance.Dialog.AddChoiceBox(info.btnName, castReadyQuestActions[info.questName]);
            }
        }
    }

    public void StoreBtn()
    {
        UIManager.Instance.Smithy.ShowSmithy();
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
