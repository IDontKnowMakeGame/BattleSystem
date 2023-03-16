using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

[CreateAssetMenu(fileName = "ClipsData", menuName = "ScriptableObject/ClipsData")]
public class WeaponClips : ScriptableObject
{
    [SerializeField]
    private int weaponID;
    public int WeaponID => weaponID;

    [SerializeField]
    private List<ClipBase> clips;

    public List<ClipBase> Clips => clips;
}