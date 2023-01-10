using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class IManager
{
    public GameManagement instance;
    public virtual void Init()
    {
        
    }
    
    public virtual void Awake()
    {
        Init();
    }

    public virtual void Start()
    {
        
    }

    public virtual void Update()
    {
        
    }
}
