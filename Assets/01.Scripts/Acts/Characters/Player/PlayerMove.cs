using Acts.Characters;
using Managements.Managers;
using UnityEngine;
using Core;
using Actors.Characters.Player;
using Actors.Characters;
using System.Collections.Generic;

namespace Acts.Characters.Player
{
	[System.Serializable]
	public class PlayerMove : CharacterMove
	{
		private PlayerAnimation _playerAnimation;
		private PlayerActor _playerActor;

		private Vector3 cameraDir;
		private Vector3 playerDir;
		public Vector3 SkillDir { get; set; }

		public float distance = 1;

		private bool isSkill = false;
		public bool IsSKill
		{
			get => isSkill;
			set => isSkill = value;
		}

		private Queue<Vector3> moveDir = new Queue<Vector3>();

		[SerializeField]
		private ParticleSystem dust;

		public override void Awake()
		{
			base.Awake();
			InputManager<Weapon>.OnMovePress += EnqueMove;
		}

		public override void Start()
		{
			base.Start();
			_playerAnimation = ThisActor.GetAct<PlayerAnimation>();
			_playerActor = InGame.Player.GetComponent<PlayerActor>();

			Define.GetManager<RoomManager>().CurrentRoomSetting();
		}

		public override void Update()
		{
			base.Update();
			PopMove();
		}

		public override void Translate(Vector3 direction)
		{
			playerDir = direction;
			direction = InGame.CamDirCheck(direction);

			base.Translate(direction * distance);
		}

		public override void Move(Vector3 position)
		{

			if (isSkill)
				_isMoving = false;
			base.Move(position);
		}
		#region Test Code
		private void EnqueMove(Vector3 direction)
		{
			int check = ThisActor.GetAct<PlayerEquipment>().CurrentWeapon.WeaponInfo.Weight > 5 ? 0 : 1;
			if (moveDir.Count > check || enableQ || LoadingSceneController.Instnace.IsVisbleLoading()) return;
			moveDir.Enqueue(direction);
		}

		public void ResetMoveQueue()
		{
			moveDir.Clear();
		}

		private void PopMove()
		{
			if (ThisActor.GetAct<CharacterStatAct>().ChangeStat.hp <= 0) return;
			if (moveDir.Count > 0 && !_playerActor.HasState(~CharacterState.DontMoveAniation) && !_isMoving)
			{
				enableQ = true;
				playerDir = moveDir.Dequeue();
				Vector3 dir = InGame.CamDirCheck(playerDir);
				dust.Play();
				base.Translate(dir * distance);
			}
		}
		#endregion

		public void BowBackStep(Vector3 position)
		{
			playerDir = (position - ThisActor.Position);
			base.Move(position);
		}

		/// <summary>
		/// Player Animation Setting
		/// </summary>
		protected override void AnimationCheck()
		{
			if (isSkill)
			{
				SkillAnimation();
			}
			else
			{
				OrginalAnimation();
			}
		}

		private void OrginalAnimation()
		{
			if (_playerActor.HasState(CharacterState.DontMoveAniation))
				return;

			if (playerDir == Vector3.left)
			{
				if (_playerActor.currentWeapon is Spear == false || (_playerActor.currentWeapon as Spear).NonDir == false)
					ThisActor.SpriteTransform.localScale = new Vector3(-2, 1, 1);
				_playerAnimation.Play("HorizontalMove");
			}
			else if (playerDir == Vector3.right)
			{
				if (_playerActor.currentWeapon is Spear == false || (_playerActor.currentWeapon as Spear).NonDir == false)
				{
					ThisActor.SpriteTransform.localScale = new Vector3(2, 1, 1);
				}
				_playerAnimation.Play("HorizontalMove");
			}
			else if (playerDir == Vector3.forward)
			{
				_playerAnimation.Play("UpperMove");
			}
			else if (playerDir == Vector3.back)
			{
				_playerAnimation.Play("LowerMove");
			}
		}

		public void SkillAnimation()
		{
			//Debug.Log(SkillDir);
			if (SkillDir == Vector3.left)
			{
				ThisActor.SpriteTransform.localScale = new Vector3(-2, 1, 1);
				_playerAnimation.Play("HorizontalSkill");
			}
			else if (SkillDir == Vector3.right)
			{
				ThisActor.SpriteTransform.localScale = new Vector3(2, 1, 1);
				_playerAnimation.Play("HorizontalSkill");
			}
			else if (SkillDir == Vector3.forward)
			{
				_playerAnimation.Play("UpperSkill");
			}
			else if (SkillDir == Vector3.back)
			{
				_playerAnimation.Play("LowerSkill");
			}
		}

		protected override void MoveStop()
		{
			if (ThisActor.GetAct<CharacterStatAct>().ChangeStat.hp <= 0) return;

			if (isSkill)
			{
				isSkill = false;
			}
			else
			{
				if (!_playerActor.HasState(CharacterState.DontMoveAniation))
					_playerAnimation.Play("Idle");
			}

			MapManager _map = Define.GetManager<MapManager>();
			if (QuestManager.Instance != null)
				QuestManager.Instance.CheckRoomMission(ThisActor.Position);

			bool mode = _map.GetBlock(ThisActor.Position).isWarm;
			ThisActor.GetAct<PlayerFlooding>().ChangeWarmMode(mode);
			Define.GetManager<RoomManager>().CurrentRoomSetting();
			dust.Stop();
			base.MoveStop();
			//QuestManager.Instance.CheckRoomMission(ThisActor.Position);
		}
	}
}