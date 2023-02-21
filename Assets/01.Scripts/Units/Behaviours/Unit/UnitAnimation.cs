using System.Collections.Generic;
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
        private Material baseMaterial;
        private Material whiteMaterial;
        public int state = 0;

        public override void Start()
        {
            baseMaterial = renderer.materials[0];
            whiteMaterial = renderer.materials[1];

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

                if (_clips[state].events != null)
                {
                    if(_clips[state].events.Count >= _clips[state].fps)
                    {
                        _clips[state].events[index]?.Invoke();
                    }
                }
                
                var offset = ((float)_clips[state].texture.width / _clips[state].fps) / _clips[state].texture.width;
                baseMaterial.SetTexture("_BaseMap", _clips[state].texture);
                baseMaterial.SetTextureOffset("_BaseMap", Vector2.right * (offset * index));
                baseMaterial.SetTextureScale("_BaseMap", new Vector2(offset, 1f));
                baseMaterial.SetTexture("_MainTex", _clips[state].texture);
                baseMaterial.SetTextureOffset("_MainTex", Vector2.right * (offset * index));
                baseMaterial.SetTextureScale("_MainTex", new Vector2(offset, 1f));
                
                
                whiteMaterial.SetTexture("_MainTex", _clips[state].texture);
                whiteMaterial.SetVector("_Offset", Vector2.right * (offset * index));
                whiteMaterial.SetVector("_Tiling", new Vector2(offset, 1f));
                
                renderer.material = baseMaterial;
            }
        }

        public void ChangeState(int value)
        {
            List<AnimeClip> _clips = clips.clips;
            if (_clips[state].changeAble || isFinished)
            {
                isFinished = false;
                state = value;
                index = -1;
                time = 0f;
            }
        }

        public void ChangeClips(Clips changeClips)
        {
            clips = changeClips;
            isFinished = false;
            state = 0;
            index = -1;
            time = 0f;
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

        public int CurState()
        {
            return state;
        }
        
        public bool IsFinished()
        {
            return isFinished;
        }
    }
}