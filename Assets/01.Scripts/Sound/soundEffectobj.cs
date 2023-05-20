using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundEffectobj : MonoBehaviour
{

    public void Play(AudioClip audioClip, float _pitch)
    {
        Debug.Log(audioClip);
        AudioSource audioSource = this.GetComponent<AudioSource>();
        audioSource.pitch = _pitch;
        audioSource.PlayOneShot(audioClip);
    }
}
