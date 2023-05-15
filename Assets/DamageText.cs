using Actors.Bases;
using Core;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;

public class DamageText : Actor
{
	[SerializeField]
	private TextRenderer _textRenderer;

	[SerializeField]
	private VisualEffect _effect;

	[SerializeField]
	private TextMeshPro num;

	[SerializeField]
	private Material ma;

	[SerializeField]
	private Color _basiccolor;

	[SerializeField]
	private Color _criticalColor;

	public float sizeUptime;

	public float sizeDowntime;

	public float xTime;

	public float fadeTime;

	public float upValue;

	public float upTime;


	private Sequence _seq;

	protected override void Init()
	{
		AddAct(_textRenderer);
	}

	public void PopUp(int text, Vector3 pos, Vector3 dir)
	{
		_effect.Play();

		Vector3 vec = Random.insideUnitSphere/3 + pos;
		vec.y = vec.y < 1 ? 1 : vec.y;
		transform.position = new Vector3(vec.x, vec.y, vec.z);

		if (text >= 50)
			num.color = _criticalColor;
		else
			num.color = _basiccolor;

		num.text = string.Format(text.ToString());

		_seq = DOTween.Sequence();
		this.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		_seq.Append(this.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), sizeUptime).SetEase(Ease.InQuint));
		_seq.Join(this.transform.DOMove(vec - dir, xTime));
		_seq.Append(this.transform.DOScale(new Vector3(1, 1, 1), sizeDowntime).SetEase(Ease.InQuint));
		_seq.Append(this.transform.DOMoveY(vec.y + upValue, upTime)).OnComplete(() => Define.GetManager<ResourceManager>().Destroy(this.gameObject));
		_seq.Join(num.DOFade(0, fadeTime));
	}
}
