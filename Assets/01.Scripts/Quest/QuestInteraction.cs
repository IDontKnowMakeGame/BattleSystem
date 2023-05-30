using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Data;

public class QuestInteraction : InteractionActor
{
    [SerializeField]
    private QuestName currentQuest;
    [SerializeField]
    private ItemID needItem;

    public override void Interact()
    {
        if (InGame.Player.Position.IsNeighbor(Position) == false) return;

        if(DataManager.HaveQuestItem(needItem))
        {
            Debug.Log("상호작용 성공!!");
            QuestManager.Instance.CheckDeliveryQuestMission(currentQuest);
        }
    }
}
