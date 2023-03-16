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
		//���⼭ ��ǲ �Ŵ����� ���� ���� �־��ش�.
	}

	public virtual void AttakStart(Vector3 vec)
	{
		//TODO ���⼭ HOLD��� ������Ʈ�� ���� �����ش�.
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
		//TODO ���⼭ HOLD��� ������Ʈ�� ���� �����ش�.
	}
	public override void Equiqment(CharacterActor actor)
	{
		base.Equiqment(actor);
		
	}
}
