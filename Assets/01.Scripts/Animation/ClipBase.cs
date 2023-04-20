using System;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

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
        public List<Action> events = null;
        public Action OnExit;

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

            Debug.Log(frame);
        }

        public void ClearEvent()
        {
			Debug.Log("Clear");
			events = new();
        }

        public void ChangeClip(ClipBase clipBase)
        {
            if (clipBase != null)
            {
                texture = clipBase.texture;
                fps = clipBase.fps;
                delay = clipBase.delay;
            }
        }
    }
}