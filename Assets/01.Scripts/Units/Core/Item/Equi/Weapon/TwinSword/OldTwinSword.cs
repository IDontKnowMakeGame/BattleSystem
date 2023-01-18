using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit.Core.Weapon;
public class OldTwinSword : Weapon
{
	//public override void Start()
	//{
	//	base.Start();
	//	//GetWeaponStateData("twin");
	//	//_maxTime = TwinSwordData.coolTime;
	//}
	//protected override void Skill()
	//{
	//	if (_isCoolTime)
	//		return;

	//	if (_inputManager.GetKeyInput(InputManager.InputSignal.Skill))
	//	{
	//		isSkill = true;
	//		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveForward))
	//		{
	//			SixTimeAttak(Vector3.forward);
	//		}
	//		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveBackward))
	//		{
	//			SixTimeAttak(Vector3.back);
	//		}
	//		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveLeft))
	//		{
	//			SixTimeAttak(Vector3.left);
	//		}
	//		if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveRight))
	//		{
	//			SixTimeAttak(Vector3.right);
	//		}
	//	}

	//	if (_inputManager.GetKeyUpInput(InputManager.InputSignal.Skill) && !_isCoolTime)
	//	{
	//		_isCoolTime = true;
	//		SixTimeAttak(Vector3.forward);
	//	}
	//}

	//private void SixTimeAttak(Vector3 dir)
	//{
	//	_isCoolTime = true;
	//	for (int i = 0; i < 6; i++)
	//	{
	//		_unitAttack.Attack(dir, _basicData.damage, TwinSwordData.freeze);
	//	}
	//	_baseObject.StartCoroutine(waitReset());
	//}

	//private IEnumerator waitReset()
	//{
	//	yield return new WaitForSeconds(TwinSwordData.freeze);
	//	isSkill = false;
	//}
}
