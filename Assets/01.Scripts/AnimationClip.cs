using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationClip : MonoBehaviour
{
    [SerializeField]
    private List<Clips> animationClip;

    public Clips GetClip(int idx)
    {
        return animationClip[idx];
    }
}
