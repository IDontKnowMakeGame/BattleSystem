using System;
using System.Collections.Generic;
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
        public bool changeAble = true;
        public List<Action> events;

        public void SetEventOnFrame(int frame, Action action)
        {
            if(events == null)
            {
                events = new List<Action>(fps);
            }
            if(events.Count < fps)
            {
                events = new List<Action>(fps);
            }
            events[frame] = action;
        }
    }
}