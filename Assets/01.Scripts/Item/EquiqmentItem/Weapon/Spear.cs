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
		if(!_isAttack /*&& 스테이트가 없을 때*/)
		{
			_isAttack = true;
			_characterActor.StartCoroutine(AttackCorutine(vec));
		}
		else if(_isAttack/*&& 스테이트가 있을 때*/)
		{
			_isAttack = false;
			_characterActor.StartCoroutine(AttackUpCorutine(vec));
		}
	}

	public virtual IEnumerator AttackCorutine(Vector3 vec)
	{
		_attackInfo.AddDir(_attackInfo.DirTypes(vec));
		yield return new WaitForSeconds(info.Ats);
		//여기서 스테이트를 실행시켜준다.
	}

	public virtual IEnumerator AttackUpCorutine(Vector3 vec)
	{
		_attackInfo.RemoveDir(_attackInfo.DirTypes(vec));
		yield return new WaitForSeconds(info.Afs);
		//여기서 스테이트를 초기화..
	}
}
