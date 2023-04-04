using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;


namespace Acts.Characters.Enemy
{
    [System.Serializable]
    public class EnemyAnimation : UnitAnimation
    {
        [SerializeField]
        private WeaponClips curClips;

        private Dictionary<string, ClipBase> weaponClipDic = new Dictionary<string, ClipBase>();

        public override void Awake()
        {
            base.Awake();

            foreach (ClipBase clip in curClips.Clips)
            {
                weaponClipDic.Add(clip.name, clip);
            }
        }

        public override void Play(string name)
        {
            if (!weaponClipDic.ContainsKey(name)) return;
            if (currentCoroutine != null)
                ThisActor.StopCoroutine(currentCoroutine);
            curClip = weaponClipDic[name];
            currentCoroutine = ThisActor.StartCoroutine(AnimationPlay());
        }

        // 인덱스로 애니메이션 재생
        public override void Play(int idx)
        {
            if (currentCoroutine != null)
                ThisActor.StopCoroutine(currentCoroutine);
            curClip = curClips.Clips[idx];
            currentCoroutine = ThisActor.StartCoroutine(AnimationPlay());
        }
    }
}
