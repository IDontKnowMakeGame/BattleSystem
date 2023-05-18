using Core;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Manager
{
    AudioSource[] audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public override void Awake()
    {
        GameObject root = GameObject.Find("@Sound");
        if(root == null)
        {
            root = new GameObject("@Sound");
        }
        Object.DontDestroyOnLoad(root);

        string[] soundName = System.Enum.GetNames(typeof(Define.Sound));
        for(int i = 0; i < soundName.Length - 1; i++)
        {
            GameObject go = new GameObject { name = soundName[i] };
            audioSources[i] = go.AddComponent<AudioSource>();
            go.transform.parent = root.transform;
        }

        audioSources[(int)Define.Sound.Bgm].loop = true;
    }

    public void Clear()
    {
       foreach(AudioSource audio in audioSources)
       {
            audio.clip = null;
            audio.Stop();
       }
       _audioClips.Clear();
    }

}
