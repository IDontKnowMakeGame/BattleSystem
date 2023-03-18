using Actors.Bases;
using Acts.Characters;
using Core;
using Data;
using UnityEngine;

namespace Actors.Characters
{
    public class CharacterActor : Actor
    {
        [SerializeField] private CharacterRender _characterRender;
        [SerializeField] private CharacterEquipmentAct _characterEquipment;
        [SerializeField] private CharacterStatAct _characterStat;

        public Weapon currentWeapon;
		protected override void Init()
        {
            base.Init();
            AddAct(_characterRender);
            AddAct(_characterEquipment);
            AddAct(_characterStat);
        }

        protected override void Awake()
        {
            InGame.AddActor(this);
            base.Awake();
        }

        protected override void Start()
        {
            InGame.SetActorOnBlock(this, Position);
            base.Start();

        }
    }
}