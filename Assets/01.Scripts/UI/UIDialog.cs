using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIDialog : UIBase
{
    private VisualTreeAsset choiceBoxTmep;

    private VisualElement visualImage;
    private VisualElement choicePanel;
    private VisualElement messageBox;

    private Label message;
    private Label nameText;

    private Queue<string> msgLine = new Queue<string>();
    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_Dialog");
        choiceBoxTmep = Define.GetManager<ResourceManager>().Load<VisualTreeAsset>("UIDoc/DialogChoiceBox");
        
        visualImage = _root.Q<VisualElement>("Visual");
        choicePanel = _root.Q<VisualElement>("ChoicePanel");
        messageBox = _root.Q<VisualElement>("MessageBox");
        messageBox.RegisterCallback<ClickEvent>(e =>
        {
            NextMessage();
        });

        message = _root.Q<Label>("Message");
        nameText = _root.Q<Label>("NameText");
    }
    public void FlagDialogue(bool value)
    {
        if (value)
            _root.style.display = DisplayStyle.Flex;
        else
            _root.style.display = DisplayStyle.None;
    }
    public void FlagChoicePanel(bool value)
    {
        if (value)
            choicePanel.style.display = DisplayStyle.Flex;
        else
            choicePanel.style.display = DisplayStyle.None;
    }
    public void StartListeningDialog(DialogueData dialogue)
    {
        FlagDialogue(true);
        FlagChoicePanel(false);
        choicePanel.Clear();

        nameText.text = dialogue.name;
        //ChangeVisualImage(dialogue.name);

        this.msgLine.Clear();
        foreach (string msg in dialogue.sentence)
            this.msgLine.Enqueue(msg);

        NextMessage();
    }
    public void NextMessage()
    {
        SetMessageBoxText(msgLine.Dequeue());

        if (msgLine.Count <= 0)
            EndMessage();
    }
    public void EndMessage()
    {
        FlagChoicePanel(true);
    }
    public void SetMessageBoxText(string message)
    {
        this.message.text = message;
    }
    public void SetBoxNameText(string name) 
    {
        this.nameText.text = name;
    }
    public void AddChoiceBox(string name,Action action)
    {
        VisualElement choiceBox = choiceBoxTmep.Instantiate();
        choiceBox.Q<Label>().text = name;

        choiceBox.RegisterCallback<ClickEvent>(e =>
        {
            action();
            FlagDialogue(false);
        });

        choicePanel.Add(choiceBox);
    }
    public void ClearChoiceBox()
    {
        choicePanel.Clear();
    }
    public void ChangeVisualImage(string name)
    {
        visualImage.style.backgroundImage = new StyleBackground(Define.GetManager<ResourceManager>().Load<Sprite>(name));
    }
    public void ChangeVisualImage(Sprite sprite)
    {
        visualImage.style.backgroundImage = new StyleBackground(sprite);
    }
}
