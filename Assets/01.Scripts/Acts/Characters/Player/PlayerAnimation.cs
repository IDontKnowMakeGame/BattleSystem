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

        // ���� � ���� �ִϸ��̼�����?, �ִϸ��̼�
        public WeaponClips curWeaponClips;

        public override void Awake()
        {
            base.Awake();

            // ���� �ִϸ��̼ǵ��� Dictionary�� ���� ����(ID�� ���Ͽ� �ҷ��� �� ����)
            foreach (WeaponClips weaponClips in weaponAnimations)
            {
                weaponAnimationDic.Add(weaponClips.WeaponID, weaponClips);
            }
        }

        public override void Start()
        {
            base.Start();
        }

        // id�� ���� ���� �ִϸ����͸� �ٲ�
        public void ChangeWeaponClips(int id)
        {
            curWeaponClips = weaponAnimationDic[id];
            SetweaponClipDic();
        }

        // name�� key�� �޾� name�� ���� clip�� ã�� �� �ְ� ��
        public void SetweaponClipDic()
        {
            weaponClipDic.Clear();

            foreach (ClipBase clip in curWeaponClips.Clips)
            {
                weaponClipDic.Add(clip.name, clip);
            }
        }

        // �̸����� �ִϸ��̼� ���
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

        // �ε����� �ִϸ��̼� ���
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
