using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
public class TaintedSword : BaseStraightSword
{
	public override void Start()
	{
		base.Start();
		GetWeaponStateData("oldSword");
		_inputManager.ChangeInGameAction(InputTarget.Skill, InputStatus.Press, () => Skill(Vector3.zero));
	}
	protected override void Skill(Vector3 vec)
	{
		if (_isCoolTime)
			return;

		if (isSkill)
			return;

		_thisBase.StartCoroutine(SoulRealm());
	}

	private IEnumerator SoulRealm()
	{
		isSkill = true;
		yield return new WaitForSeconds(0.5f);
		float atk = _weaponStats.Atk;
		_weaponStats.Atk = atk * 1.5f;
		_unitAttack.Attack(Vector3.forward);
		_unitAttack.Attack(Vector3.back);
		_unitAttack.Attack(Vector3.left);
		_unitAttack.Attack(Vector3.right);
		_unitAttack.Attack(Vector3.forward + Vector3.left);
		_unitAttack.Attack(Vector3.forward + Vector3.right);
		_unitAttack.Attack(Vector3.back + Vector3.left);
		_unitAttack.Attack(Vector3.back + Vector3.right);
		_weaponStats.Atk = atk;
		isSkill = false;
	}
}
