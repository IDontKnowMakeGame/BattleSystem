using UnityEngine;
using Core;

public abstract class WeaponAnimator
{
    protected Clips curClips; 

    protected Vector3 setDir;
    protected bool moving = false;
    protected bool attack = false;
    protected bool skill = false;
    protected bool charge = false;
    protected bool weaponChange = false;
    protected bool lastChange = false;
    protected Transform playerSprite;

    public Vector3 SetDir
    {
        get => setDir;
        set
        {
            setDir = value;
        }
    }
    public bool Moving
    {
        get => moving;
        set => moving = value;
    }    
    public bool Attack
    {
        get => attack;
        set => attack = value;
    }
    public bool Skill
    {
        get => skill;
        set => skill = value;
    }
    public bool ChangeWeapon
    {
        get => weaponChange;
        set => weaponChange = value;
    }

    public bool Charge
    {
        get => charge;
        set => charge = value;
    }

    public bool LastChange
    {
        get => lastChange;
        set => lastChange = value;
    }

    public void ResetParameter()
    {
        setDir = Vector3.zero;
        moving = false;
        attack = false;
        skill = false;
        weaponChange = false;
        charge = false;
    }

    public virtual void Init()
    {
        lastChange = false;
        playerSprite = InGame.PlayerBase.GetComponentInChildren<MeshRenderer>().transform;
        ResetParameter();
    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual int AnimationCheck()
    {
        return 0;
    }
}
