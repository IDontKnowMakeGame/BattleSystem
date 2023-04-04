using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Actors.Bases;
using Acts.Characters;
using Core;
public class DamagePopUp : Actor
{
	[SerializeField]
	private TextRenderer _textRenderer;

	[SerializeField]
	private TextMeshPro num;

	[SerializeField]
	private Color _basiccolor;

	[SerializeField]
	private Color _criticalColor;


	[SerializeField]
	private float yPower;
	[SerializeField]
	private float xPower;
	[SerializeField]
	private float PowerSpeed;

	[SerializeField]
	private float Down;
	[SerializeField]
	private float DownSpeed;

	[SerializeField]
	private float HoriSpeed;

	protected override void Init()
	{
		AddAct(_textRenderer);
	}

	public void DamageText(int text, Vector3 pos)
	{
		num.alpha = 1;
		Vector2 vec = Random.insideUnitCircle;
		transform.position = new Vector3(pos.x, Random.Range(pos.y, pos.y + 1f), pos.z);
		//transform.localEulerAngles = new Vector3(transform.rotation.x - 45, transform.rotation.y, transform.rotation.z);
		if (text >= 50)
			num.color = _criticalColor;
		else
			num.color = _basiccolor;

		num.text = string.Format(text.ToString());
		Sequence mySequence = DOTween.Sequence();
		int a = vec.x > 0 ? 1 : -1;
		mySequence.Append(transform.DOMoveX((a * xPower + vec.x) + transform.position.x, HoriSpeed).SetEase(Ease.Linear));
		mySequence.Join(transform.DOMoveY(Mathf.Abs(transform.position.y) + yPower, PowerSpeed).SetEase(Ease.Linear)).AppendCallback(() => {
			num.DOFade(0, 0.35f);
		});
		mySequence.Append(transform.DOMoveY(Down, DownSpeed).SetEase(Ease.Linear)).AppendCallback(() => { Define.GetManager<ResourceManager>().Destroy(this.gameObject); });
	}

}
