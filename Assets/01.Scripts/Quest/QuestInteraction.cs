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

    protected override void Init()
    {
        base.Init();

        if(DataManager.PlayerOpenQuestData_.openQuestList.Contains(QuestName.FallenAngelCarryingThing) == false)
        {
            RemoveInteration();
        }
    }

    public override void Interact()
    {
        if (InGame.Player.Position.IsNeighbor(Position) == false) return;

        if(DataManager.HaveQuestItem(needItem))
        {
            Debug.Log("��ȣ�ۿ� ����!!");
            QuestManager.Instance.CheckDeliveryQuestMission(currentQuest);
        }
    }
}
