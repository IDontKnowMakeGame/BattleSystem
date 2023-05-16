using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private GameObject soundObjs;
    private List<GameObject> usingSounds = new List<GameObject>();
    
    PoolManager pool;

    private void Awake()
    {
        pool = Define.GetManager<PoolManager>();
        pool.CreatePool(soundObjs, 10);
    }

    public void CreateSoundEffect()
    {

    }
}
