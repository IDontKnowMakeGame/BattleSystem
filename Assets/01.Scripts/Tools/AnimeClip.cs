using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        public bool changeAble = true;
        public List<UnityEvent> events;
    }
}