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

        castReadyQuestActions.Add(QuestName.FallenAngelSupplyThing, AcceptFallenAngelSupplyThingQuestBtn);
        castClearQuestActions.Add(QuestName.FallenAngelSupplyThing, ClearFallenAngelSupplyThingQuestBtn);

        castReadyQuestActions.Add(QuestName.FallenAngelCarryingThing, AcceptFallenAngelCarryingThingQuestBtn);
        castClearQuestActions.Add(QuestName.FallenAngelCarryingThing, ClearFallenAngelCarryingThingQuestBtn);

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
       
        if(questData.firstTalked)
            Talking();
        else
            FirstTalking();
    }

    public void Talking()
    {
        Debug.Log(gameObject.name);

        ReadyBtnQuest();
        UIManager.Instance.Dialog.AddChoiceBox("물건을 사고싶다.", StoreBtn);


        UIManager.Instance.Dialog.StartListeningDialog(dialogueList[0]);
    }
    public void FirstTalking()
    {
        Debug.Log(gameObject.name);
        ReadyBtnQuest();
        UIManager.Instance.Dialog.AddChoiceBox("물건을 사고싶다.", StoreBtn);

        UIManager.Instance.Dialog.StartListeningDialog(dialogueList[8]);

        Define.GetManager<DataManager>().AddItemInInventory(ItemID.Torch, 30);

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
                if (castReadyQuestActions.ContainsKey(info.questName)==false)
                    Debug.LogError($"Not Have Dictionary info QuestInfo : {info.questName}");

                UIManager.Instance.Dialog.AddChoiceBox(info.btnName, castReadyQuestActions[info.questName],true);
            }

        }
    }

    #region Btn
    public void StoreBtn()
    {
        UIManager.Instance.ItemStore.ItemSet(table);
        UIManager.Instance.PadeInOut.Pade(PadeType.padeUp, UIManager.Instance.ItemStore.Show);
    }
    //================================================================================================
    public void AcceptFallenAngelSupplyThingQuestBtn()
    {
        UIManager.Instance.Dialog.StartListeningDialog(dialogueList[1]);
        Define.GetManager<DataManager>().OpenQuest(QuestName.FallenAngelSupplyThing);

        //Define.GetManager<DataManager>().AddItemInInventory(questData.quests[0].questGiveItem[0], 1);
    }

    public void ClearFallenAngelSupplyThingQuestBtn()
    {
        UIManager.Instance.Dialog.StartListeningDialog(dialogueList[2]);
        Define.GetManager<DataManager>().ClearQuest(QuestName.FallenAngelSupplyThing);
        Define.GetManager<DataManager>().ReadyQuest(QuestName.FallenAngelCarryingThing);

        Define.GetManager<DataManager>().AddItemInInventory(questData.quests[0].rewords[0], 1);
    }
    //================================================================================================
    public void AcceptFallenAngelCarryingThingQuestBtn()
    {

        UIManager.Instance.Dialog.AddChoiceBox("잘 쓰고 있다.", () =>
        {
            FallenAngelCarryingThingQuestBtn();
            UIManager.Instance.Dialog.StartListeningDialog(dialogueList[4],false);
        });
        UIManager.Instance.Dialog.AddChoiceBox("아니", () =>
        {
            FallenAngelCarryingThingQuestBtn();
            UIManager.Instance.Dialog.StartListeningDialog(dialogueList[5], false);
        });

        UIManager.Instance.Dialog.StartListeningDialog(dialogueList[3],false);
        Define.GetManager<DataManager>().OpenQuest(QuestName.FallenAngelCarryingThing);

        
    }
    public void FallenAngelCarryingThingQuestBtn()
    {
        UIManager.Instance.Dialog.AddChoiceBox("...", () =>
        {
            Define.GetManager<DataManager>().AddItemInInventory(questData.quests[1].questGiveItem[0], 1);
            UIManager.Instance.Dialog.StartListeningDialog(dialogueList[6]);
        });
    }

    public void ClearFallenAngelCarryingThingQuestBtn()
    {
        UIManager.Instance.Dialog.StartListeningDialog(dialogueList[7]);
        Define.GetManager<DataManager>().ClearQuest(QuestName.FallenAngelCarryingThing);

        Define.GetManager<DataManager>().AddItemInInventory(questData.quests[1].rewords[0], 1);
    }
    //================================================================================================

    public void BackBtn()
    {
        UIManager.Instance.Dialog.FlagDialogue(false);
    }

    #endregion
}
