using System.Collections;
using System.Numerics;
using Actors;
using Actors.Bases;
using Actors.Characters;
using Acts.Base;

public class CharacterGrogy : Act
{
	private CharacterActor _actor;
	public override void Start()
	{
		base.Start();
		_actor = ThisActor as CharacterActor;
	}
	public void Stun(float duration = 1)
	{
		_actor.AddState(CharacterState.Stun);
	}

	public void NuckBack(Vector3 dir, float power, float duration = 1)
	{
		_actor.AddState(CharacterState.NuckBack);
	}
}
