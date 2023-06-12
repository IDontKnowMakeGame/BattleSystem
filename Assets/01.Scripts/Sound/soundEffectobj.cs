using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundEffectobj : MonoBehaviour
{
    WaitForSeconds wait = new WaitForSeconds(2f);

    public void PlayEffect(AudioClip audioClip, float _pitch, float volume)
    {
        if (this.gameObject == null) return;
        AudioSource audioSource = this.GetComponent<AudioSource>();
        audioSource.clip = null;
        audioSource.pitch = _pitch;
        audioSource.PlayOneShot(audioClip, volume);
        StartCoroutine(PushSoundObj());
    }

    private IEnumerator PushSoundObj()
    {
        yield return wait;
        if (this.gameObject != null && Define.GetManager<PoolManager>() != null)
        {
            Define.GetManager<PoolManager>().Push(this.gameObject.GetComponent<Poolable>());
        }
    }

}
