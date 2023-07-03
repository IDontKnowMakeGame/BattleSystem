using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;
using System;
using Actors.Characters;
using Core;

namespace Acts.Characters.Enemy
{
    [System.Serializable]
    public class EnemyAnimation : UnitAnimation
    {
        [SerializeField]
        protected WeaponClips curClips;

		protected Dictionary<string, ClipBase> weaponClipDic = new Dictionary<string, ClipBase>();

        public override void Start()
        {
            base.Start();
			if (curClips.Clips.Count == 0)
				return;
			foreach (ClipBase clip in curClips.Clips)
			{
				weaponClipDic.Add(clip.name, clip);
			}
			Play("Idle");
		}

        public override void Play(string name)
        {
            if (!weaponClipDic.ContainsKey(name)) return;
            if (currentCoroutine != null)
                ThisActor.StopCoroutine(currentCoroutine);
            var character = ThisActor as CharacterActor;
            if (character == null)
                return;
            if (character.HasState(CharacterState.Die))
                return;
            curClip = weaponClipDic[name];
            Define.GetManager<SoundManager>().PlayAtPoint($"Boss/{ThisActor.name}/{name}", ThisActor.Position,  1);
            currentCoroutine = ThisActor.StartCoroutine(AnimationPlay());
        }

        // 인덱스로 애니메이션 재생
        public override void Play(int idx)
        {
            if (currentCoroutine != null)
                ThisActor.StopCoroutine(currentCoroutine);
            var character = ThisActor as CharacterActor;
            if (character == null)
                return;
            if (character.HasState(CharacterState.Die))
                return;
            curClip = curClips.Clips[idx];
            currentCoroutine = ThisActor.StartCoroutine(AnimationPlay());
        }
        
        public override ClipBase GetClip(string name)
        {
            var clips = curClips.Clips;
            
            for (int i = 0; i < clips.Count; i++)
            {
                if (clips[i].name == name)
                    return clips[i];
            }

            return null;
        }
    }
}
