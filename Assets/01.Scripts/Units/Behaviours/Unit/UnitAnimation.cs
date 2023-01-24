using System.Collections.Generic;
using Tools;
using UnityEngine;
using Behaviour = Units.Behaviours.Base.Behaviour;

namespace Units.Behaviours.Unit
{
    [System.Serializable]
    public class UnitAnimation : Behaviour
    {
        
        public List<AnimeClip> clips = new();
        private int index = 0;
        private float time = 0f;
        private bool isFinished = false;
        public Renderer renderer;
        private Material material;
        public int state = 0;

        public override void Start()
        {
            material = renderer.material;
        }

        public override void Update()
        {
            if (isFinished && !clips[state].isLoop) return;
            isFinished = false;
            time += Time.deltaTime;

            if (time >= clips[state].delay)
            {
                time = 0f;
                index = (index + 1) % clips[state].fps;

                var offset = ((float)clips[state].texture.width / clips[state].fps) / clips[state].texture.width;
                material.SetTexture("_BaseMap", clips[state].texture);
                material.SetTextureOffset("_BaseMap", Vector2.right * (offset * index));
                material.SetTextureScale("_BaseMap", new Vector2(offset, 1f));
                material.SetTexture("_MainTex", clips[state].texture);
                material.SetTextureOffset("_MainTex", Vector2.right * (offset * index));
                material.SetTextureScale("_MainTex", new Vector2(offset, 1f));
                renderer.material = material;
                if (index == 0)
                    isFinished = true;
            }
        }

        public void ChangeState(int value)
        {
            state = value;
            index = 0;
            time = 0f;
        }
    }
}