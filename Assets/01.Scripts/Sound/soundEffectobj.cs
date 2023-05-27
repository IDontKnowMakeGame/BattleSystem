using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundEffectobj : MonoBehaviour
{
    public void PlayEffect(AudioClip audioClip, float _pitch, float volume)
    {
        AudioSource audioSource = this.GetComponent<AudioSource>();
        audioSource.clip = null;
        audioSource.pitch = _pitch;
        audioSource.PlayOneShot(audioClip, volume);
        StartCoroutine(PushSoundObj(audioClip.length));
    }

    private IEnumerator PushSoundObj(float len)
    {
        yield return new WaitForSeconds(len + 0.5f);
        Define.GetManager<PoolManager>().Push(this.gameObject.GetComponent<Poolable>());
    }

}
