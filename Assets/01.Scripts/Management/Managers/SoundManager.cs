using Core;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Manager
{
    AudioSource[] audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    GameObject soundObj;

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
        soundObj = Define.GetManager<ResourceManager>().Load<GameObject>("Prefabs/SoundEffectObj");
        Define.GetManager<PoolManager>().CreatePool(soundObj, 70);
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

    public void PlayAtPoint(AudioClip audioClip, Transform _transform, float pitch = 1.0f)
    {
        if (audioClip == null) return;

        var go = Define.GetManager<PoolManager>().Pop(soundObj, _transform);
        go.gameObject.GetComponent<soundEffectobj>().PlayEffect(audioClip, pitch);

    }

    public void PlayAtPoint(string path, Transform _transform, float pitch = 1.0f)
    {
        AudioClip clip = GetOrAddAudioClip(path, Define.Sound.Effect);
        PlayAtPoint(clip, _transform, pitch);
    }

    public void PlayAtPoint(AudioClip audioClip, Vector3 _vec, float pitch = 1.0f)
    {
        if (audioClip == null) return;

        var go = Define.GetManager<PoolManager>().Pop(soundObj);
        go.transform.position = _vec;
        go.gameObject.GetComponent<soundEffectobj>().PlayEffect(audioClip, pitch);

    }

    public void PlayAtPoint(string path, Vector3 _vec, float pitch = 1.0f)
    {
        AudioClip clip = GetOrAddAudioClip(path, Define.Sound.Effect);
        PlayAtPoint(clip, _vec, pitch);
    }

    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null) return;

        if(type == Define.Sound.Bgm)
        {
            AudioSource audio = audioSources[(int)Define.Sound.Bgm];
            audio.pitch = pitch;
            audio.clip = audioClip;
            audio.Play();
        }
        else
        {
            AudioSource audio = audioSources[(int)Define.Sound.Effect];
            audio.pitch = pitch;
            audio.PlayOneShot(audioClip);
        }
    }

    public void Play(string path,Define.Sound type = Define.Sound.Effect, float pitch = 1)
    {
        AudioClip clip = GetOrAddAudioClip(path, type);
        Play(clip, type, pitch);
    }

    private AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        if (path.Contains("Sounds/") == false)
        {
            path = $"Sounds/{path}";
        }
        AudioClip clip = null;

        if(type == Define.Sound.Bgm)
        {
            clip = Define.GetManager<ResourceManager>().Load<AudioClip>(path);
        }
        else
        {
            if(_audioClips.TryGetValue(path, out clip) == false)
            {
                clip = Define.GetManager<ResourceManager>().Load<AudioClip>(path);
                _audioClips.Add(path, clip);
            }
        }

        if(clip == null)
        {
            Debug.Log($"{path} missing");
        }

        return clip;
    }
}
