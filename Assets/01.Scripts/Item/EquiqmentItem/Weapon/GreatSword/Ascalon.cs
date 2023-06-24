using Actors.Characters;
using Actors.Characters.Player;
using Acts.Characters.Player;
using Core;
using Managements;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ascalon : GreatSword
{
	private GameObject _obj = null;

	private IEnumerator cor = null;

	public override void Skill(Vector3 vec)
	{
		if (_isCoolTime)
			return;
		if (_characterActor == null)
			return;

		_isCoolTime = true;

		GameObject obj = GameManagement.Instance.GetManager<ResourceManager>().Instantiate("Hit-AscalonEffect");
		obj.transform.position = _characterActor.Position + Vector3.up;
		obj.GetComponent<EffectDestory>().destroyEvent = () =>
		{
			_obj = GameManagement.Instance.GetManager<ResourceManager>().Instantiate("AscalonEffect");
			_obj.transform.SetParent(_characterActor.transform);
			_obj.transform.localPosition = Vector3.zero;
		};

		Define.GetManager<SoundManager>().PlayAtPoint("Sounds/GreatSword/AscalonSkill(2)", this._characterActor.transform.position);

		_stat.PercentAtk(30);

		cor = EffectTimer();
		_characterActor?.StartCoroutine(cor);

		PlayerAttack.OnSkillEnd += SkillEnd;
		InputManager<GreatSword>.OnClickPress += RemainVector;
	}

	private IEnumerator EffectTimer()
	{
		yield return new WaitForSeconds(info.CoolTime);
		_obj.transform.SetParent(null);
		GameManagement.Instance?.GetManager<ResourceManager>()?.Destroy(_obj);
		_stat?.PercentAtk(-30);
		PlayerAttack.OnSkillEnd -= SkillEnd;
		_characterActor.StopCoroutine(cor);
	}

	private void RemainVector(Vector3 vec)
	{
		if (!_isCoolTime)
			return;

		_remainVec = DirReturn(vec);
		Debug.Log(_remainVec);
	}

	private Vector3 _remainVec;

	public override void Update()
	{
		base.Update();
	}

	public override void UnEquipment(CharacterActor actor)
	{
		base.UnEquipment(actor);

		PlayerAttack.OnSkillEnd -= SkillEnd;
		InputManager<GreatSword>.OnClickPress -= RemainVector;
		if(_isCoolTime)
		{
			_stat?.PercentAtk(-30);
		}
		if(cor != null)
			_characterActor.StopCoroutine(cor);

		if (_obj)
		{
			_obj.transform.SetParent(null);
			GameManagement.Instance.GetManager<ResourceManager>().Destroy(_obj);
		}
	}

	private void SkillEnd(int id)
	{
		if (id != _characterActor.UUID)
			return;

		if (_obj)
		{
			_obj.transform.SetParent(null);
			GameManagement.Instance.GetManager<ResourceManager>().Destroy(_obj);
		}

		_stat.PercentAtk(-30);
		PlayerAttack.OnSkillEnd -= SkillEnd;
		PlayerAttack.OnAttackEnd -= SkillEnd;
		GameObject obj = GameManagement.Instance.GetManager<ResourceManager>().Instantiate("Dragon Slayer's Realm");
		_characterActor.StopCoroutine(cor);
		obj.transform.position = _characterActor.Position + InGame.CamDirCheck(_remainVec) + (Vector3.up / 2);
		obj.GetComponent<DragonRealm>().Init(AscalonData.duration, AscalonData.decrease);

		Define.GetManager<SoundManager>().PlayAtPoint("Sounds/GreatSword/AscalonSkill(1)", this._characterActor.transform.position);
	}
}
