using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public static VideoManager Instance { get; set; }

    [SerializeField]
    private VideoClip[] videoClips = null;

    private VideoPlayer player;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Mutiple VideoManager");
        }
        Instance = this;

        player = GetComponent<VideoPlayer>();
    }

    public void ChangeVideo(int num)
    {
        if (videoClips != null)
        {
            player.clip = videoClips[num-3];
            player.Play();
        }
    }
}
