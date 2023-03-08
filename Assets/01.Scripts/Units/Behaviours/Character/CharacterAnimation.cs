using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviour = Units.Behaviours.Default.Behaviour;

public class CharacterAnimation : Behaviour
{
    protected ClipBase curClip;

    [SerializeField]
    private Renderer renderer;

    private Material baseMaterial;
    private Material whiteMaterial;

    private int index = 0;
    private bool isFinished = false;

    protected Coroutine PlayCoroutine;

    public override void Awake()
    {
        if (renderer == null)
            Debug.Log("Null");
        baseMaterial = renderer.materials[0];
        whiteMaterial = renderer.materials[1];
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


            whiteMaterial.SetTexture("_MainTex", curClip.texture);
            whiteMaterial.SetVector("_Offset", Vector2.right * (offset * index));
            whiteMaterial.SetVector("_Tiling", new Vector2(offset, 1f));

            renderer.material = baseMaterial;
        }

        if (isFinished)
        {
            if (curClip.isLoop)
                PlayCoroutine = ThisBase.StartCoroutine(AnimationPlay());
            else if (curClip.nextIdx != -1)
            {
                Play(curClip.nextIdx);
            }
            yield break;
        }
    }

    public virtual void Play(string name)
    {

    }

    public virtual void Play(int idx)
    {

    }
}
