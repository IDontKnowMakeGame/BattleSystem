using Actors.Characters.NPC;
using Core;
using Data;
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

    private StorePurchaseData storeData;
    private QuestData questData;

    private Dictionary<QuestName,Action> castReadyQuestActions = new Dictionary<QuestName,Action>();
    private Dictionary<QuestName, Action> castClearQuestActions = new Dictionary<QuestName, Action>();

    private Dictionary<ItemID,PurchaseItemInfoData> purchaseItemDict = new Dictionary<ItemID,PurchaseItemInfoData>();

    protected override void Init()
    {
        base.Init();

        storeData = JsonManager.LoadJsonFile<StorePurchaseData>(Application.streamingAssetsPath + "/SAVE/NPC/Store", GetType().Name);
        questData = JsonManager.LoadJsonFile<QuestData>(Application.streamingAssetsPath + "/SAVE/NPC/Quest", GetType().Name);

        InitPurchaseItemData();
        SaveQuestData();
    }

    public void SaveQuestData()
    {
        string json = JsonManager.ObjectToJson(questData);
        JsonManager.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/NPC/Quest", GetType().Name,json);
    }
    public void InitPurchaseItemData()
    {
        foreach(PurchaseItemInfoData data in storeData.itemList)
        {
            purchaseItemDict[data.itemID] = data;
        }
    }
    public void SavePurchaseItemData()
    {
        List<PurchaseItemInfoData> list = new List< PurchaseItemInfoData >();
        foreach(PurchaseItemInfoData data in purchaseItemDict.Values)
        {
            list.Add(data);
        }

        storeData.itemList = list;
        string json = JsonManager.ObjectToJson(storeData);
        JsonManager.SaveJsonFile(Application.streamingAssetsPath + "/SAVE/NPC/Store", GetType().Name, json);
    }

    public override void Interact()
    {
        if (InGame.Player.Position.IsNeighbor(Position) == false) return;
       
        Talking();
    }

    public void Talking()
    {
        Debug.Log(gameObject.name);
        UIManager.Instance.Dialog.AddChoiceBox("물건을 사고싶다.", StoreBtn);

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
