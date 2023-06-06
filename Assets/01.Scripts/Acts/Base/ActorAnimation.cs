using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using System.Collections;
using Acts.Base;
using UnityEngine.Rendering;

namespace Acts.Characters
{
	public class UnitAnimation : Act
	{
		public ClipBase curClip;

		[SerializeField]
		private Renderer renderer;

		private Material baseMaterial;

		public int index = 0;
		public bool isFinished = false;

		protected Coroutine currentCoroutine;

		public override void Start()
		{
			base.Start();
			baseMaterial = renderer.materials[0];
		}


		public IEnumerator AnimationPlay()
		{
			index = -1;
			isFinished = false;
			curClip.OnEnter?.Invoke();
			while (true)
			{
				yield return new WaitForSeconds(curClip.delay);

				index++;

				if (index == curClip.fps)
				{
					curClip.OnExit?.Invoke();
					isFinished = true;
					index = -1;
					break;
				}

				if (curClip.events != null)
				{
					if (curClip.events.Count >= curClip.fps)
						curClip.events[index]?.Invoke();
				}

				var offset = ((float)curClip.texture.width / curClip.fps) / curClip.texture.width;

				baseMaterial.SetTexture("_BaseMap", curClip.texture);
				if(curClip.normal != null)
					baseMaterial.SetTexture("_NormalMap", curClip.normal);
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
					currentCoroutine = ThisActor.StartCoroutine(AnimationPlay());
				else if (curClip.nextIdx != -1)
				{
					Play(curClip.nextIdx);
				}
				else
					currentCoroutine = null;
				yield return null;
			}
		}

		public virtual void Play(string name)
		{

		}

		public virtual void Play(int idx)
		{

		}

		public virtual ClipBase GetClip(string name)
		{
			return curClip;
		}
	}
}