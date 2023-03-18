using Core;
using Managements.Managers;
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
	public override void LoadWeaponClassLevel()
	{
		WeaponClassLevelData level = Define.GetManager<DataManager>().LoadWeaponClassLevel("GreatSword");
		switch (KillToLevel(level.killedCount))
		{
			case 1:
				//_weaponClassLevelInfo.Atk = 10;
				//_weaponClassLevelInfo.Ats -= 0.01f;
				break;
			case 2:
				//_weaponClassLevelInfo.Atk = 15;
				//_weaponClassLevelInfo.Ats -= 0.03f;
				break;
			case 3:
				//_weaponClassLevelInfo.Atk = 20;
				//_weaponClassLevelInfo.Ats -= 0.05f;
				break;
			case 4:
				//_weaponClassLevelInfo.Atk = 20;
				//_weaponClassLevelInfo.Ats -= 0.07f;
				//_weaponClassLevelInfo.Afs -= 0.01f;
				break;
			case 5:
				//_weaponClassLevelInfo.Atk = 20;
				//_weaponClassLevelInfo.Ats -= 0.07f;
				//_weaponClassLevelInfo.Afs -= 0.05f;
				break;
		}
	}

	public override void LoadWeaponLevel()
	{

	}

	public virtual void AttakStart(Vector3 vec)
	{
		//TODO 여기서 HOLD라는 스테이트를 실행 시켜준다.
		//if(_playerActor)
		_attackInfo.ResetDir();
		_currrentVector = vec;
	}
	public virtual void Hold(Vector3 vec)
	{
		if (timer >= info.Ats)
		{
			//if (/*&& !characterBase.State.HasFlag(BaseState.Attack)*/)
			// characterBase.State.AddState(BaseState.Attack)
			//TODO 여기서 스테이트를 추가해준다.
			return;
		}

		if (timer >= info.Ats)
			return;

		timer += Time.deltaTime;
	}
	public virtual void AttackRealease(Vector3 vec)
	{
		//TODO 여기에서 characterBase.State.HasFlag(BaseState.Attack) if문 넣어주기
		_attackInfo.SizeX = 1;
		_attackInfo.SizeZ = 1;
		_attackInfo.AddDir(_attackInfo.DirTypes(_currrentVector));

		timer = 0;
		_currrentVector = Vector3.zero;
		//TODO 여기서 HOLD라는 스테이트를 제거 시켜준다.
	}
}
