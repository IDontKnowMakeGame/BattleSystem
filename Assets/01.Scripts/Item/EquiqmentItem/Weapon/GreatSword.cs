using Actors.Bases;
using Actors.Characters;
using Managements.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GreatSword : Weapon
{
	public Vector3 _currrentVector;
	public float timer;
	public override void Init()
	{
		InputManager<GreatSword>.OnAttackPress += AttakStart;
		InputManager<GreatSword>.OnAttackHold += Hold;
		InputManager<GreatSword>.OnAttackRelease += AttackRealease;
	}

	public virtual void AttakStart(Vector3 vec)
	{
		//TODO ���⼭ HOLD��� ������Ʈ�� ���� �����ش�.
		//if(_playerActor.)
		_currrentVector = vec;
	}
	public virtual void Hold(Vector3 vec)
	{
		if (timer >= info.Ats)
		{
			//if (/*&& !characterBase.State.HasFlag(BaseState.Attack)*/)
			// characterBase.State.AddState(BaseState.Attack)
			//TODO ���⼭ ������Ʈ�� �߰����ش�.
			return;
		}

		if (timer >= info.Ats)
			return;

		timer += Time.deltaTime;
	}
	public virtual void AttackRealease(Vector3 vec)
	{	
		_attackInfo.SizeX = 1;	
		_attackInfo.SizeZ = 1;
		_attackInfo.AddDir(_attackInfo.DirTypes(_currrentVector));

		timer = 0;
		_currrentVector = Vector3.zero;
		//TODO ���⼭ HOLD��� ������Ʈ�� ���� �����ش�.
	}
	public override void Equiqment(CharacterActor actor)
	{
		base.Equiqment(actor);
	}
}
