using Actors.Characters.NPC;
using Core;
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

    protected override void Init()
    {
        base.Init();

        questData = JsonManager.LoadJsonFile<QuestData>(Application.streamingAssetsPath+"/NPC/Quest",GetType().Name);
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

        UIManager.Instance.Dialog.AddChoiceBox("무기 강화", StoreBtn);
        UIManager.Instance.Dialog.AddChoiceBox("돌아가기", BackBtn);
    }

    public void StoreBtn()
    {
        UIManager.Instance.Smithy.ShowSmithy();
    }

    public void BackBtn()
    {
        UIManager.Instance.Dialog.FlagDialogue(false);
    }
}
