using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    private DialogDataSO data;

    private Dictionary<string, Action> buttonAction;

    public void BindingButtonAction(string choiceText, Action action)
    {
        
    }
}
