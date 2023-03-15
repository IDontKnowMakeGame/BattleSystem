using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;
using Tools;

[System.Serializable]
public class PlayerAnimation : UnitAnimation
{
    [SerializeField]
    private List<WeaponClips> weaponAnimations;

    private Dictionary<int, WeaponClips> weaponAnimationDic = new Dictionary<int, WeaponClips>();
    private Dictionary<string, ClipBase> weaponClipDic = new Dictionary<string, ClipBase>();

    // ���� � ���� �ִϸ��̼�����?, �ִϸ��̼�
    public WeaponClips curWeaponClips;


    // TO DO ���߿� ID �ҷ����� ������� ����
    [SerializeField]
    private int curID = 100;

    public override void Awake()
    {
        base.Awake();

        // ���� �ִϸ��̼ǵ��� Dictionary�� ���� ����(ID�� ���Ͽ� �ҷ��� �� ����)
        foreach (WeaponClips weaponClips in weaponAnimations)
        {
            weaponAnimationDic.Add(weaponClips.WeaponID, weaponClips);
        }

        ChangeWeaponClips(curID);
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
        if(currentCoroutine != null)
            ThisBase.StopCoroutine(currentCoroutine);
        curClip = weaponClipDic[name];
        currentCoroutine = ThisBase.StartCoroutine(AnimationPlay());
    }

    // �ε����� �ִϸ��̼� ���
    public override void Play(int idx)
    {
        if (currentCoroutine != null)
            ThisBase.StopCoroutine(currentCoroutine);
        curClip = curWeaponClips.Clips[idx];
        currentCoroutine = ThisBase.StartCoroutine(AnimationPlay());
    }

    public ClipBase CurrentClip()
    {
        return curClip;
    }
}
