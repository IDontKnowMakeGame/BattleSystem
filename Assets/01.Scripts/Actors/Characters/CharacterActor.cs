using Actors.Bases;
using Acts.Characters;
using Core;
using UnityEngine;

namespace Actors.Characters
{
    public class CharacterActor : Actor
    {
        [SerializeField] private CharacterRender _characterRender;

		public Weapon Weapon = null;
		protected override void Init()
        {
            base.Init();
            AddAct(_characterRender);
            AddAct<CharacterEquipmentAct>();
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