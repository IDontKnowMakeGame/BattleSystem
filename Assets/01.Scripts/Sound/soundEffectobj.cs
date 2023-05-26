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
        Invoke("EndClip", audioClip.length);
    }

    private void EndClip()
    {
        Define.GetManager<PoolManager>().Push(this.gameObject.GetComponent<Poolable>());
    }
}
