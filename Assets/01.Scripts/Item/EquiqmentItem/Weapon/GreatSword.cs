using Actors.Bases;
using Actors.Characters;
using Managements.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSword : Weapon
{
	public Vector3 _currrentVector;
	public float timer;
	public override void Init()
	{
		//여기서 인풋 매니져에 관한 것을 넣어준다.
	}

	public virtual void AttakStart(Vector3 vec)
	{
		//TODO 여기서 HOLD라는 스테이트를 실행 시켜준다.
		//if(_playerActor.)
		_currrentVector = vec;
	}
	public virtual void Hold()
	{
		if (timer >= info.Ats)
			return;

		timer += Time.deltaTime;
	}
	public virtual void AttackRealease()
	{
		timer = 0;
		_currrentVector = Vector3.zero;
		//TODO 여기서 HOLD라는 스테이트를 제거 시켜준다.
	}
	public override void Equiqment(CharacterActor actor)
	{
		base.Equiqment(actor);
		
	}
}
