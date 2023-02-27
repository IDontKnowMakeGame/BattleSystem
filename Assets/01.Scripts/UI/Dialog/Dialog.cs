using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    private void Start()
    {
        ShowDialog();
    }

    public void ShowDialog(/*Sentence data*/)
    {
        Core.Define.GetManager<UIManager>().CreateDialog("asd");
        Core.Define.GetManager<UIManager>().CreateChoiceBox("asd", ShowDialog2 );
        Core.Define.GetManager<UIManager>().CreateChoiceBox("asd", () => { Debug.Log("¿Ã∞‘ µ 22?"); });
    }
    public void ShowDialog2(/*Sentence data*/)
    {
        Core.Define.GetManager<UIManager>().CreateDialog("zxc");
        Core.Define.GetManager<UIManager>().CreateChoiceBox("zxc", () => { Debug.Log("¿Ã∞‘ µ ?"); });
        Core.Define.GetManager<UIManager>().CreateChoiceBox("zxc", () => { Debug.Log("¿Ã∞‘ µ 22?"); });
    }
}
