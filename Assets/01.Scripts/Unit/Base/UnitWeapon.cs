using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using Unit.Player;
using System;
using Behaviour = Unit.Behaviour;
public enum SwordType
{
    LongSword,
    GreatSword,
    TwinSword,
    End
}
namespace Unit
{
    public class UnitWeapon : Behaviour
    {
        protected SwordType _currentSword;

        protected Dictionary<SwordType, Weapon> weaponSkills = new Dictionary<SwordType, Weapon>();
        public Weapon currentWeapon { get { return weaponSkills[_currentSword]; } }

        public override void Awake()
        {
            weaponSkills.Add(SwordType.LongSword, new LongSword() { _baseObject = thisBase });
            weaponSkills.Add(SwordType.GreatSword, new LongSword() { _baseObject = thisBase });
            weaponSkills.Add(SwordType.TwinSword, new TwinSword() { _baseObject = thisBase });

            foreach (var value in weaponSkills)
            {
                value.Value?.Awake();
            }
        }

        public override void Start()
        {
            foreach (var value in weaponSkills)
            {
                value.Value?.Start();
            }
        }
        public override void Update()
        {
            weaponSkills[_currentSword]?.Update();
        }
    }
}
