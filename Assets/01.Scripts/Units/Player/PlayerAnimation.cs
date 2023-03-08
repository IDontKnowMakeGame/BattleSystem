using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAnimation : CharacterAnimation
{
    [SerializeField]
    private List<WeaponClips> weaponAnimations;

    private Dictionary<int, WeaponClips> weaponAnimationDic = new Dictionary<int, WeaponClips>();
    private Dictionary<string, ClipBase> weaponClipDic = new Dictionary<string, ClipBase>();

    // 현재 어떤 무기 애니메이션인지?, 애니메이션
    private WeaponClips curWeaponClips;


    // TO DO 나중에 ID 불려오는 방식으로 저장
    [SerializeField]
    private int curID = 100;

    public override void Start()
    {
        // 무기 애니메이션들을 Dictionary를 통해 관리(ID를 통하여 불려올 수 있음)
        foreach (WeaponClips weaponClips in weaponAnimations)
        {
            weaponAnimationDic.Add(weaponClips.WeaponID, weaponClips);
        }

        ChangeWeaponClips(curID);
        Play("Idle");
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
        if(PlayCoroutine != null)
            ThisBase.StopCoroutine(PlayCoroutine);

        curClip = weaponClipDic[name];
        PlayCoroutine =  ThisBase.StartCoroutine(AnimationPlay());
    }

    // 인덱스로 애니메이션 재생
    public override void Play(int idx)
    {
        if (PlayCoroutine != null)
            ThisBase.StopCoroutine(PlayCoroutine);

        curClip = curWeaponClips.Clips[idx];
        PlayCoroutine =  ThisBase.StartCoroutine(AnimationPlay());
    }
}
