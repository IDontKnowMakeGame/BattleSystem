using System;
using UnityEngine;

namespace Tools
{
    [Serializable]
    public class AnimeClip
    {
        public string name;
        public int nextIdx = -1;
        public Texture2D texture;
        public int fps;
        public bool isLoop;
        public float delay;
    }
}