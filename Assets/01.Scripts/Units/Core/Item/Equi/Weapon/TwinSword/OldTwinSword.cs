using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Managements.Managers;
using Units.Base.Unit;
public class OldTwinSword : BaseTwinSword
{
	public override void Awake()
	{
		base.Awake();
		GetWeaponStateData("oldTwinSword");
	}
	public override void Start()
	{
		base.Start();
	}

	public override void ChangeKey()
	{
		base.ChangeKey();
		_inputManager.ChangeInGameAction(InputTarget.Skill, InputStatus.Press, () => Skill(Vector3.zero));
	}
	protected override void Skill(Vector3 vec)
	{
		if (_isCoolTime)
			return;

		_thisBase.AddState(BaseState.Skill);
		_thisBase.AddState(BaseState.StopMove);

		_inputManager.ChangeInGameKey(InputTarget.UpAttack, KeyCode.W);
		_inputManager.ChangeInGameKey(InputTarget.DownAttack, KeyCode.S);
		_inputManager.ChangeInGameKey(InputTarget.LeftAttack, KeyCode.A);
		_inputManager.ChangeInGameKey(InputTarget.RightAttack, KeyCode.D);

		_inputManager.ChangeInGameAction(InputTarget.UpAttack, InputStatus.Press, ()=>SixTimeAttak(Vector3.forward));
		_inputManager.ChangeInGameAction(InputTarget.DownAttack, InputStatus.Press, ()=>SixTimeAttak(Vector3.back));
		_inputManager.ChangeInGameAction(InputTarget.LeftAttack, InputStatus.Press, ()=>SixTimeAttak(Vector3.left));
		_inputManager.ChangeInGameAction(InputTarget.RightAttack, InputStatus.Press, ()=>SixTimeAttak(Vector3.right));
	}

	private void SixTimeAttak(Vector3 dir)
	{
		_isCoolTime = true;
		for (int i = 0; i < 6; i++)
		{
			Define.GetManager<MapManager>().Damage(_thisBase.Position+dir,_unitStat.NowStats.Atk,0.5f,waitReset);
		}
	}

	private void waitReset()
	{
		_thisBase.RemoveState(BaseState.Skill);
		_thisBase.RemoveState(BaseState.StopMove);

		_inputManager.ChangeInGameKey(InputTarget.UpMove, KeyCode.UpArrow);
		_inputManager.ChangeInGameKey(InputTarget.DownMove, KeyCode.DownArrow);
		_inputManager.ChangeInGameKey(InputTarget.LeftMove, KeyCode.LeftArrow);
		_inputManager.ChangeInGameKey(InputTarget.RightMove, KeyCode.RightArrow);

		_inputManager.ChangeInGameAction(InputTarget.UpAttack, InputStatus.Press, () => Attack(Vector3.forward));
		_inputManager.ChangeInGameAction(InputTarget.DownAttack, InputStatus.Press, () => Attack(Vector3.back));
		_inputManager.ChangeInGameAction(InputTarget.LeftAttack, InputStatus.Press, () => Attack(Vector3.left));
		_inputManager.ChangeInGameAction(InputTarget.RightAttack, InputStatus.Press, () => Attack(Vector3.right));
	}
}
