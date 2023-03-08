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

    // ���� � ���� �ִϸ��̼�����?, �ִϸ��̼�
    private WeaponClips curWeaponClips;


    // TO DO ���߿� ID �ҷ����� ������� ����
    [SerializeField]
    private int curID = 100;

    public override void Start()
    {
        // ���� �ִϸ��̼ǵ��� Dictionary�� ���� ����(ID�� ���Ͽ� �ҷ��� �� ����)
        foreach (WeaponClips weaponClips in weaponAnimations)
        {
            weaponAnimationDic.Add(weaponClips.WeaponID, weaponClips);
        }

        ChangeWeaponClips(curID);
        Play("Idle");
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
        if(PlayCoroutine != null)
            ThisBase.StopCoroutine(PlayCoroutine);

        curClip = weaponClipDic[name];
        PlayCoroutine =  ThisBase.StartCoroutine(AnimationPlay());
    }

    // �ε����� �ִϸ��̼� ���
    public override void Play(int idx)
    {
        if (PlayCoroutine != null)
            ThisBase.StopCoroutine(PlayCoroutine);

        curClip = curWeaponClips.Clips[idx];
        PlayCoroutine =  ThisBase.StartCoroutine(AnimationPlay());
    }
}
