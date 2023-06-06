using Actors.Characters;
using Acts.Characters;
using Acts.Characters.Player;
using Core;
using Managements.Managers;
using UnityEngine;

public class OldStraightSword : StraightSword
{
	public override void Skill(Vector3 vec)
	{
		if (_isCoolTime)
			return;
		if (_playerActor.HasAnyState())
			return;

		_characterActor.AddState(CharacterState.Skill);
		CharacterMove.OnMoveEnd += SkillInputEnd;
		_characterActor.RemoveState(CharacterState.Skill);
		PlayerMove move = _characterActor.GetAct<PlayerMove>();
		move.IsSKill = true;
		move.SkillDir = vec;
		move.Move(_characterActor.Position + InGame.CamDirCheck(vec) * 2);

		Define.GetManager<EventManager>().TriggerEvent(EventFlag.TraillOnOff, new EventParam { intParam = 3, boolParam = true});
	}

	protected override void SkillInputEnd(int i, Vector3 vec)
	{
		if (_characterActor == null) return;

		if (i != _characterActor.UUID)
			return;

		base.SkillInputEnd(i, DirReturn(vec));
		_characterActor.GetAct<PlayerMove>().IsSKill = false;
		CharacterMove.OnMoveEnd -= SkillInputEnd;
		Define.GetManager<EventManager>().TriggerEvent(EventFlag.TraillOnOff, new EventParam { intParam = 3, boolParam = false });
	}
}
