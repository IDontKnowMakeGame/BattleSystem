﻿using System.Collections.Generic;
using Tools;
using UnityEngine;
using Behaviour = Units.Behaviours.Base.Behaviour;

namespace Units.Behaviours.Unit
{
    [System.Serializable]
    public class UnitAnimation : Behaviour
    {

        public Clips clips;

        [Header("WeaponClips[WeaponType 순서대로 넣기]")]
        [SerializeField]
        private List<Clips> weaponClips;

        private Clips basicClips;
        private int index = 0;

        private float time = 0f;
        private bool isFinished = false;
        public Renderer renderer;
        private Material material;
        public int state = 0;

        public override void Start()
        {
            material = renderer.material;

            if(clips != null)
            {
                basicClips = clips;
            }
        }

        public override void Update()
        {
            List<AnimeClip> _clips = clips.clips;
            if (isFinished && !_clips[state].isLoop)
            {
                if(_clips[state].nextIdx != -1)
                {
                    ChangeState(_clips[state].nextIdx);
                }
                return;
            }
            isFinished = false;
            time += Time.deltaTime;

            if (time >= _clips[state].delay)
            {
                time = 0f;
                index += 1;
                if (index == _clips[state].fps)
                {
                    index = -1;
                    isFinished = true;
                    return;
                }
                var offset = ((float)_clips[state].texture.width / _clips[state].fps) / _clips[state].texture.width;
                material.SetTexture("_BaseMap", _clips[state].texture);
                material.SetTextureOffset("_BaseMap", Vector2.right * (offset * index));
                material.SetTextureScale("_BaseMap", new Vector2(offset, 1f));
                material.SetTexture("_MainTex", _clips[state].texture);
                material.SetTextureOffset("_MainTex", Vector2.right * (offset * index));
                material.SetTextureScale("_MainTex", new Vector2(offset, 1f));
                renderer.material = material;
            }
        }

        public void ChangeState(int value)
        {
            isFinished = false;
            state = value;
            index = -1;
            time = 0f;
        }

        public void ChangeClips(Clips changeClips)
        {
            clips = changeClips;
        }

        public int GetFPS()
        {
            List<AnimeClip> _clips = clips.clips;
            return _clips[state].fps;
        }

        public int CurIndex()
        {
            return index;
        }


        public bool IsFinished()
        {
            return isFinished;
        }
    }
}