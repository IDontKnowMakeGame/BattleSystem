using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor.Acts
{
    public class ActorAnimation : Act
    {
        public ClipBase curClip;

        [SerializeField]
        private Renderer renderer;

        [SerializeField]
        private Texture texture;

        private Material baseMaterial;

        public int index = 0;
        public bool isFinished = false;

        protected override void Awake()
        {
            base.Awake();

            baseMaterial = renderer.materials[0];
        }


        public IEnumerator AnimationPlay()
        {
            index = -1;
            isFinished = false;
            while (true)
            {
                yield return new WaitForSeconds(curClip.delay);

                index++;
                if (index == curClip.fps)
                {
                    index = -1;
                    isFinished = true;
                    break;
                }

                var offset = ((float)curClip.texture.width / curClip.fps) / curClip.texture.width;
                baseMaterial.SetTexture("_BaseMap", curClip.texture);
                baseMaterial.SetTextureOffset("_BaseMap", Vector2.right * (offset * index));
                baseMaterial.SetTextureScale("_BaseMap", new Vector2(offset, 1f));
                baseMaterial.SetTexture("_MainTex", curClip.texture);
                baseMaterial.SetTextureOffset("_MainTex", Vector2.right * (offset * index));
                baseMaterial.SetTextureScale("_MainTex", new Vector2(offset, 1f));


                baseMaterial.SetTexture("_MainTex", curClip.texture);
                baseMaterial.SetVector("_Offset", Vector2.right * (offset * index));
                baseMaterial.SetVector("_Tiling", new Vector2(offset, 1f));

                renderer.material = baseMaterial;
            }

            if (isFinished)
            {
                if (curClip.isLoop)
                    StartCoroutine("AnimationPlay");
                else if (curClip.nextIdx != -1)
                {
                    Play(curClip.nextIdx);
                }
            }
        }

        public virtual void Play(string name)
        {

        }

        public virtual void Play(int idx)
        {

        }
    }
}
