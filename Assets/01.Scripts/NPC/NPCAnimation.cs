using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acts.Characters;

namespace Acts.Characters.NPC
{
    [System.Serializable]
    public class NPCAnimation : UnitAnimation
    {
        [SerializeField]
        private WeaponClips npcClips;

        public override void Start()
        {
            base.Start();

            Play(0);
        }

        public override void Play(int idx)
        {
            curClip = npcClips.Clips[idx];
            currentCoroutine = ThisActor.StartCoroutine(AnimationPlay());
        }
    }
}
