using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Sentence
{
    public string name;
    public string sentence;
    public List<string> choiceText;
}

[CreateAssetMenu(menuName ="SO/Dialog")]
public class DialogDataSO : ScriptableObject
{
    public List<Sentence> sentences;
}
