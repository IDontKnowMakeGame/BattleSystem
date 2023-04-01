using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;
using Core;

namespace Acts.Characters.Player
{
    [System.Serializable]
    public class PlayerAnimation : UnitAnimation
    {
        [SerializeField]
        private List<WeaponClips> weaponAnimations;

        private Dictionary<int, WeaponClips> weaponAnimationDic = new Dictionary<int, WeaponClips>();
        private Dictionary<string, ClipBase> weaponClipDic = new Dictionary<string, ClipBase>();

        // 현재 어떤 무기 애니메이션인지?, 애니메이션
        public WeaponClips curWeaponClips;

        public override void Awake()
        {
            base.Awake();

            // 무기 애니메이션들을 Dictionary를 통해 관리(ID를 통하여 불려올 수 있음)
            foreach (WeaponClips weaponClips in weaponAnimations)
            {
                weaponAnimationDic.Add(weaponClips.WeaponID, weaponClips);
            }
        }

        public override void Start()
        {
            base.Start();
        }

        // id를 통해 무기 애니메이터를 바꿈
        public void ChangeWeaponClips(int id)
        {
            curWeaponClips = weaponAnimationDic[id];
            SetweaponClipDic();
        }

        // name을 key로 받아 name을 통해 clip을 찾을 수 있게 함
        public void SetweaponClipDic()
        {
            weaponClipDic.Clear();

            foreach (ClipBase clip in curWeaponClips.Clips)
            {
                weaponClipDic.Add(clip.name, clip);
            }
        }

        // 이름으로 애니메이션 재생
        public override void Play(string name)
        {
            if (!weaponClipDic.ContainsKey(name)) return;
            if (currentCoroutine != null)
                ThisActor.StopCoroutine(currentCoroutine);
            curClip = weaponClipDic[name];
            currentCoroutine = ThisActor.StartCoroutine(AnimationPlay());
            InGame.Player.SpriteTransform.localScale = new Vector3(weaponClipDic[name].scaleX, 1, 1);
        }

        public ClipBase GetClip(string name)
        {
            if (!weaponClipDic.ContainsKey(name)) return null;
            return weaponClipDic[name];
        }

        // 인덱스로 애니메이션 재생
        public override void Play(int idx)
        {
            if (currentCoroutine != null)
                ThisActor.StopCoroutine(currentCoroutine);
            curClip = curWeaponClips.Clips[idx];
            currentCoroutine = ThisActor.StartCoroutine(AnimationPlay());
        }

        public ClipBase CurrentClip()
        {
            return curClip;
        }

        public override void OnDisable()
        {
            currentCoroutine = null;
            foreach (var item in weaponClipDic.Values)
            {
                item.ClearEvent();
            }
            base.OnDisable();
        }
    }
}
