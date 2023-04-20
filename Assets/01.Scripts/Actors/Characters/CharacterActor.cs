using System;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using Actors.Bases;
using Acts.Characters;
using Core;
using Data;
using Managements.Managers;
using UnityEngine;

namespace Actors.Characters
{
	[Flags]
	public enum CharacterState
	{
		None = 0,
		Move = 1 << 0,
		Attack = 1 << 1,
		Skill = 1 << 2,
		Hold = 1 << 3,
		StopMove = 1 << 4,
		Stun = 1 << 5,
		Equip = 1 << 6,
		KnockBack = 1 << 7,
		Everything = ~None,
	}
	public class CharacterActor : Actor
	{
		[SerializeField] private CharacterRender _characterRender;
		[SerializeField] private CharacterState _characterState;
		[SerializeField] private bool _isBlocking = false;
		public Weapon currentWeapon;
		protected override void Init()
		{
			base.Init();
			AddAct(_characterRender);
		}

		protected override void Start()
		{
			if (_isBlocking)
			{
				UpdatePosition();
				InGame.SetActorOnBlock(this);
			}
			base.Start();
		}

		protected override void Update()
		{
			if (HasCCState()) return;
			base.Update();
		}

		public void AddState(CharacterState state)
		{
			_characterState |= state;
		}

		public void RemoveState(CharacterState state)
		{
			_characterState &= ~state;
		}

		public bool HasState(CharacterState state)
		{
			return (_characterState & state) != CharacterState.None;
		}

		public bool HasAnyState()
		{
			return (_characterState & CharacterState.Everything) != CharacterState.None;
		}
		
		public bool HasCCState()
		{
			if (HasState(CharacterState.Stun))
				return true;
			if (HasState(CharacterState.KnockBack))
				return true;
			return false;
		}

		public void Stun(float delay)
		{
			StartCoroutine(StunCoroutine(delay));
		}
		
		private IEnumerator StunCoroutine(float delay)
		{
			AddState(CharacterState.Stun);
			yield return new WaitForSeconds(delay);
			RemoveState(CharacterState.Stun);
		}

		protected override void UpdatePosition()
		{
			base.UpdatePosition();
			var map = Define.GetManager<MapManager>();
			var block = map.GetBlock(Position);
			if (block != null)
			{
				var target = map.GetBlock(Position).ActorOnBlock;
				if (target)
					if(this != target)
						target.GetAct<CharacterMove>()?.KnockBack(2);
			}
			InGame.SetActorOnBlock(this);
		}
	}
}