using Core;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Manager
{
    AudioSource[] audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    AudioMixer audioMix;
    AudioMixerGroup[] audioMixerGroups;
    Dictionary<string, AudioClipInfo> _audioClips = new Dictionary<string, AudioClipInfo>();
    Dictionary<string, AudioSource> _playingAudioSources = new Dictionary<string, AudioSource>();

    GameObject soundObj;
    bool isFadeout;

    public override void Awake()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject("@Sound");
        }
        Object.DontDestroyOnLoad(root);

        string[] soundName = System.Enum.GetNames(typeof(Define.Sound));
        if (root.GetComponentsInChildren<AudioSource>().Length > 0)
        {
            audioSources = (AudioSource[])root.GetComponentsInChildren<AudioSource>();
        }
        else
        {
            for (int i = 0; i < soundName.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundName[i] };
                audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }
        }

        audioSources[(int)Define.Sound.Bgm].loop = true;
        soundObj = Define.GetManager<ResourceManager>().Load<GameObject>("Prefabs/SoundEffectObj");
        audioMix = soundObj.GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer;
        audioMixerGroups = audioMix.FindMatchingGroups("Master");
        audioSources[(int)Define.Sound.Bgm].outputAudioMixerGroup = audioMixerGroups[1];
        audioSources[(int)Define.Sound.Effect].outputAudioMixerGroup = audioMixerGroups[2];
        Define.GetManager<PoolManager>().CreatePool(soundObj, 50);
    }

    public override void Update()
    {
        foreach (AudioClipInfo info in _audioClips.Values)
        {
            List<SEInfo> newList = new List<SEInfo>();
            foreach (SEInfo seInfo in info.playingList)
            {
                seInfo.curTime = seInfo.curTime - Time.deltaTime;
                if (seInfo.curTime > info.clip.length - 0.05f)
                {
                    newList.Add(seInfo);
                }
                else
                {
                    info.stockList.Add(seInfo.index, seInfo);
                }
            }
            info.playingList = newList;
        }
        if(isFadeout)
        {
            BGMFadeOut();
        }
    }

    public void Clear()
    {
        foreach (AudioSource audio in audioSources)
        {
            audio.clip = null;
            audio.Stop();
        }
        _audioClips.Clear();
    }

    //게임 오브젝트에 자식으로 붙여서 계속 소리 낼 때 사용하는 함수
    private void PlayAtPoint(AudioClipInfo audioClipInfo, Transform _transform, float pitch = 1.0f)
    {
        if (audioClipInfo.clip == null) return;

        var go = Define.GetManager<PoolManager>().Pop(soundObj, _transform);
        if (_playingAudioSources.ContainsKey(audioClipInfo.clip.name))
        {
            _playingAudioSources.Remove(audioClipInfo.clip.name);
        }
        _playingAudioSources.Add(audioClipInfo.clip.name, go?.GetComponent<AudioSource>());
        SetClipInfo(audioClipInfo, 1, pitch, go.gameObject);

    }
    //포지션 값으로 소리날 위치 정하는 함수
    private void PlayAtPoint(AudioClipInfo audioClipInfo, Vector3 _vec, float pitch = 1.0f)
    {
        if (audioClipInfo.clip == null) return;
        var go = Define.GetManager<PoolManager>().Pop(soundObj);
        if (_playingAudioSources.ContainsKey(audioClipInfo.clip.name))
        {
            _playingAudioSources.Remove(audioClipInfo.clip.name);
        }
        _playingAudioSources.Add(audioClipInfo.clip.name, go?.GetComponent<AudioSource>());
        go.transform.position = _vec;
        SetClipInfo(audioClipInfo, 1, pitch, go.gameObject);
    }
    //반복되는 사운드 이펙트
    private void PlayAtPoint(AudioClipInfo audioClipInfo, Vector3 _vec, bool isLoop, float pitch = 1.0f)
    {
        if (audioClipInfo.clip == null) return;
        var go = Define.GetManager<PoolManager>().Pop(soundObj);
        if (_playingAudioSources.ContainsKey(audioClipInfo.clip.name))
        {
            _playingAudioSources.Remove(audioClipInfo.clip.name);
        }
        _playingAudioSources.Add(audioClipInfo.clip.name, go?.GetComponent<AudioSource>());
        go.transform.position = _vec;
        SetClipInfo(audioClipInfo, 2, pitch, go.gameObject, isLoop);
    }

    /// <summary>
    /// 게임 오브젝트에 자식으로 붙여서 계속 소리 낼 때 사용하는 함수
    /// </summary>
    /// <param name="path"></param>
    /// <param name="_transform"></param>
    /// <param name="pitch"></param>
    public void PlayAtPoint(string path, Transform _transform, float pitch = 1.0f)
    {
        AudioClipInfo info = GetOrAddAudioClip(path, Define.Sound.Effect);
        PlayAtPoint(info, _transform, pitch);
    }

    /// <summary>
    /// 포지션 값으로 소리날 위치 정하는 함수
    /// </summary>
    /// <param name="path"></param>
    /// <param name="_vec"></param>
    /// <param name="pitch"></param>
    public void PlayAtPoint(string path, Vector3 _vec, float pitch = 1.0f)
    {
        AudioClipInfo info = GetOrAddAudioClip(path, Define.Sound.Effect);
        PlayAtPoint(info, _vec, pitch);
    }
    /// <summary>
    /// 반복되는 사운드 이펙트쓰
    /// </summary>
    /// <param name="path"></param>
    /// <param name="_vec"></param>
    /// <param name="pitch"></param>
    public void PlayAtPoint(string path, Vector3 _vec, bool isLoop, float pitch = 1.0f)
    {
        AudioClipInfo info = GetOrAddAudioClip(path, Define.Sound.Effect);
        PlayAtPoint(info, _vec, isLoop ,pitch);
    }

    private void Play(AudioClipInfo audioClipinfo, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (audioClipinfo.clip == null) return;

        if (type == Define.Sound.Bgm)
        {
            AudioSource audio = audioSources[(int)Define.Sound.Bgm];
            audio.pitch = pitch;
            audio.clip = audioClipinfo.clip;
            if (audio.isPlaying) audio.Stop();
            audio.volume = 1;
            audio.Play();
        }
        else
        {
            AudioSource audio = audioSources[(int)Define.Sound.Effect];
            audio.pitch = pitch;
            SetClipInfo(audioClipinfo, 0, 1);
        }
    }

    /// <summary>
    /// 위치가 필요없는 효과음이나 배경음악 실행 함수
    /// </summary>
    /// <param name="path"></param>
    /// <param name="type"></param>
    /// <param name="pitch"></param>
    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1)
    {
        AudioClipInfo info = GetOrAddAudioClip(path, type);
        Play(info, type, pitch);
    }

    //사운드 클립이 Dictionary에 있다면 가져오고, 없다면 리소스에서 로드
    private AudioClipInfo GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        if (path.Contains("Sounds/") == false)
        {
            path = $"Sounds/{path}";
        }

        if (!_audioClips.ContainsKey(path))
        {
            _audioClips.Add(path, new AudioClipInfo(5, 1));
        }

        AudioClipInfo info = _audioClips[path];

        if (info.clip == null)
            info.clip = Define.GetManager<ResourceManager>().Load<AudioClip>(path);

        if (info.clip == null)
        {
            //Debug.Log($"{path} missing");
        }
        return info;
    }

    private void SetClipInfo(AudioClipInfo info, int index, float pitch = 1f, GameObject go = null, bool isLoop = false)
    {
        float len = info.clip.length;

        if (info.stockList.Count > 0)
        {
            SEInfo seInfo = info.stockList.Values[0];
            seInfo.curTime = len;
            info.playingList.Add(seInfo);
            info.stockList.Remove(seInfo.index);

            switch (index)
            {
                case 0:
                    audioSources[(int)Define.Sound.Effect].PlayOneShot(info.clip);
                    break;
                case 1:
                    go.GetComponent<soundEffectobj>().PlayEffect(info.clip, pitch, seInfo.volume);
                    break;
                case 2:
                    go.GetComponent<soundEffectobj>().PlayEffectLoop(info.clip, pitch, seInfo.volume);
                    break;
                default:
                    Debug.Log("Not allowed index");
                    break;
            }
        }
    }

    /// <summary>
    /// 마스터 볼륨 조절 함수
    /// </summary>
    /// <param name="value"></param>
    public void SetMasterVolume(float value)
    {
        if(audioMix)
        {
            audioMixerGroups[0].audioMixer.SetFloat("MasterVolume", value);
        }
    }

    /// <summary>
    /// 배경음악 볼륨 조절 함수
    /// </summary>
    /// <param name="value"></param>
    public void SetBGmVolume(float value)
    {
        if (audioMix)
        {
            audioMixerGroups[1].audioMixer.SetFloat("BGMVolume", value);
        }
    }

    /// <summary>
    /// 효과음 볼륨 조절 함수
    /// </summary>
    /// <param name="value"></param>
    public void SetSFXVolume(float value)
    {
        if (audioMix)
        {
            audioMixerGroups[2].audioMixer.SetFloat("SoundEffectVolume", value);
        }
    }

    public void StopSound(Define.Sound type)
    {
        AudioSource audio = audioSources[(int)type];
        if(type == Define.Sound.Effect)
        {
            if (audio.isPlaying) audio.Stop();
        }
        else
        {
            isFadeout = true;
        }
    }

    public void StopSound(string Clipname)
    {
        if(_playingAudioSources[Clipname])
        {
            _playingAudioSources[Clipname].Stop();
        }
    }
    private void BGMFadeOut()
    {
        AudioSource audio = audioSources[(int)Define.Sound.Bgm];
        audio.volume -= Time.deltaTime;
        if(audio.volume <= 0.01f)
        {
            if (audio.isPlaying) audio.Stop();
            isFadeout = false;
        }
    }
}

public class AudioClipInfo
{
    public SortedList<int, SEInfo> stockList = new SortedList<int, SEInfo>();
    public AudioClip clip = null;
    public List<SEInfo> playingList = new List<SEInfo>();

    public int maxSENum = 10;
    public float initVolume = 1.0f;
    public float attenuate = 0.0f;

    public AudioClipInfo(int maxSeNum, float initVolume)
    {
        this.maxSENum = maxSeNum;
        this.initVolume = initVolume;
        attenuate = CalcAttenuateRate();

        for (int i = 0; i < maxSENum; i++)
        {
            SEInfo seInfo = new SEInfo(i, 0.01f, initVolume * Mathf.Pow(attenuate, i));
            stockList.Add(seInfo.index, seInfo);
        }
    }

    private float CalcAttenuateRate()
    {
        float n = maxSENum;
        return NewtonMethod.Run(
            delegate (float p)
            {
                return (1.0f - Mathf.Pow(p, n)) / (1.0f - p) - 1.0f / initVolume;
            },
            delegate (float p)
            {
                float ip = 1.0f - p;
                float t0 = -n * Mathf.Pow(p, n - 1.0f) / ip;
                float t1 = (1.0f - Mathf.Pow(p, n)) / ip / ip;
                return t0 + t1;
            },
            0.9f, 100);
    }
}

public class SEInfo
{
    public int index;
    public float curTime;
    public float volume;

    public SEInfo(int index, float curTime, float volume)
    {
        this.index = index;
        this.curTime = curTime;
        this.volume = volume;
    }
}

public class NewtonMethod
{
    public delegate float Func(float x);

    public static float Run(Func func, Func derive, float initX, int maxLoop)
    {
        float x = initX;
        for (int i = 0; i < maxLoop; i++)
        {
            float curY = func(x);
            if (curY < 0.000001f && curY > -0.00001f)
                break;
            x = x - curY / derive(x);
        }
        return x;
    }

}

