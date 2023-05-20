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
    private VisualElement nameBox;

    private Label message;
    private Label nameText;

    private Coroutine textLineCoroutine = null;
    private string currentTextLine = "";
    private bool isPlayLineText = false;
    private bool isPlayDialog = false;

    private Queue<string> msgLine = new Queue<string>();
    public override void Init()
    {
        _root = UIManager.Instance._document.rootVisualElement.Q<VisualElement>("UI_Dialog");
        choiceBoxTmep = Define.GetManager<ResourceManager>().Load<VisualTreeAsset>("UIDoc/DialogChoiceBox");
        
        visualImage = _root.Q<VisualElement>("Visual");
        choicePanel = _root.Q<VisualElement>("ChoicePanel");
        nameBox = _root.Q<VisualElement>("NameBox");
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
    public void StartListeningDialog(DialogueData dialogue,bool isEndMessage = true,bool isObject = false)
    {
        Debug.Log("StartListeningDialog");
        if (isPlayDialog) return;

        if (isObject)
        {
            visualImage.style.display = DisplayStyle.None;
            nameBox.style.display= DisplayStyle.None;
        }
        else
        {
            visualImage.style.display = DisplayStyle.Flex;
            nameBox.style.display = DisplayStyle.Flex;
        }

        if(isEndMessage)
            AddChoiceBox("돌아가기", ()=> { });

        FlagDialogue(true);
        FlagChoicePanel(false);
        isPlayDialog = true;

        nameText.text = dialogue.name;
        //ChangeVisualImage(dialogue.name);

        this.msgLine.Clear();
        foreach (string msg in dialogue.sentence)
            this.msgLine.Enqueue(msg);

        NextMessage();
    }
    public IEnumerator SetTextLine(string textline)
    {
        SetMessageBoxText("");
        char[] texts = textline.ToCharArray();
        isPlayLineText = true;
        for (int i =0;i< texts.Length;i++)
        {
            this.message.text += texts[i];
            yield return new WaitForSeconds(0.05f);
        }
        isPlayLineText = false;
    }
    public void NextMessage()
    {
        if (isPlayDialog == false) return;
        if(isPlayLineText)
        {
            UIManager.Instance.StopCoroutine(textLineCoroutine);
            SetMessageBoxText(currentTextLine);
            isPlayLineText = false;
            return;
        }
        if (msgLine.Count <= 0)
        {
            //FlagDialogue(false);
            EndMessage();
            return;
        }

        currentTextLine = this.msgLine.Dequeue();
        textLineCoroutine = UIManager.Instance.StartCoroutine(SetTextLine(currentTextLine));
    }
    public void EndMessage()
    {
        //isPlayDialog = false;
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
        if (isPlayDialog) return;

        VisualElement choiceBox = choiceBoxTmep.Instantiate();
        choiceBox.Q<Label>().text = name;

        choiceBox.RegisterCallback<ClickEvent>(e =>
        {
            BackBtn();
            action();
        });

        choicePanel.Add(choiceBox);
    }
    public void BackBtn()
    {
        isPlayDialog = false;
        choicePanel.Clear();
        FlagDialogue(false);
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
