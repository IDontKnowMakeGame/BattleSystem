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
        public List<Action> events = null;

        public void SetEventOnFrame(int frame, Action action)
        {
            Debug.Log(action);
            if(events == null)
            {
                events = new List<Action>(new Action[fps]);
            }
            if(events.Count < fps)
            {
                events = new List<Action>(new Action[fps]);
            }
            Debug.Log(events.Count);
            Debug.Log(fps);
            events[frame] = action;
        }
    }
}