using Managements.Managers;
using System.Collections;
using UnityEngine;

public class Spear : Weapon
{
	private bool _isAttack;
	public override void Init()
	{
		InputManager<StraightSword>.OnAttackPress += Attack;
	}

	public override void LoadWeaponClassLevel()
	{

	}

	public override void LoadWeaponLevel()
	{

	}

	public virtual void Attack(Vector3 vec)
	{
		if(!_isAttack /*&& ������Ʈ�� ���� ��*/)
		{
			_isAttack = true;
			_characterActor.StartCoroutine(AttackCorutine(vec));
		}
		else if(_isAttack/*&& ������Ʈ�� ���� ��*/)
		{
			_isAttack = false;
			_characterActor.StartCoroutine(AttackUpCorutine(vec));
		}
	}

	public virtual IEnumerator AttackCorutine(Vector3 vec)
	{
		_attackInfo.AddDir(_attackInfo.DirTypes(vec));
		yield return new WaitForSeconds(info.Ats);
		//���⼭ ������Ʈ�� ��������ش�.
	}

	public virtual IEnumerator AttackUpCorutine(Vector3 vec)
	{
		_attackInfo.RemoveDir(_attackInfo.DirTypes(vec));
		yield return new WaitForSeconds(info.Afs);
		//���⼭ ������Ʈ�� �ʱ�ȭ..
	}
}
