using System;
using UnityEngine;

namespace Tools
{
    [Serializable]
    public class AnimeClip
    {
        public string name;
        public Texture2D texture;
        public int fps;
        public bool isLoop;
        public float delay;
    }
}