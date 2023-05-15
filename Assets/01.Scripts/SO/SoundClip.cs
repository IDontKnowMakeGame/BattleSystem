using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "soundClipSO", menuName = "ScriptableObject/sounds")]

public class SoundClip : ScriptableObject
{
    [SerializeField] private List<AudioClip> audioClips;
    
}
