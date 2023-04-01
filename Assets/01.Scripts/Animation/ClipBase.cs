using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    [Serializable]
    public class ClipBase
    {
        public string name;
        public int nextIdx = -1;
        public Texture2D texture;
        public int fps;
        public bool isLoop;
        public float delay;
        public float scaleX = 1;
        public List<Action> events = null;

        public void SetEventOnFrame(int frame, Action action)
        {
            if (events == null)
            {
                events = new List<Action>(new Action[fps]);
            }
            if (events.Count < fps)
            {
                events = new List<Action>(new Action[fps]);
            }
            events[frame] = action;
        }
        
        public void ClearEvent()
        {
            events = new();
        }

        public void ChangeClip(ClipBase clipBase)
        {
            texture = clipBase.texture;
            fps = clipBase.fps;
            delay = clipBase.delay;
        }
    }
}