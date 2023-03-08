using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClipBase
{
    public string name;
    public int nextIdx = -1;
    public Texture2D texture;
    public int fps;
    public bool isLoop;
    public float delay;
}
