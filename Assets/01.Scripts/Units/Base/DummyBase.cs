using System.Collections;
using System.Collections.Generic;
using Units.Base.Unit;
using Units.Behaviours.Unit;
using UnityEngine;

public class DummyBase : EnemyBase
{
    [SerializeField] private UnitStat _thisStat;
    [SerializeField] private CharacterRender _thisRender;
    public override UnitStat ThisStat => _thisStat;

    protected override void Init()
    {
        AddBehaviour(_thisStat);
        AddBehaviour(_thisRender);
        base.Init();
    }
}
