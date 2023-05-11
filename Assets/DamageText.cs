using Core;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro num;

	[SerializeField]
	private Color _basiccolor;

	[SerializeField]
	private Color _criticalColor;

	public float sizeUptime;

	public float waitTime;

	public float fadeTime;

	public float upValue;

	public float upTime;


	private Sequence _seq;

	public void PopUp(int text, Vector3 pos)
	{
		//_seq.Append()
		//num.alpha = 1;
		Vector3 vec = Random.insideUnitSphere + pos;
		transform.position = new Vector3(vec.x, vec.y, vec.z);

		if (text >= 50)
			num.color = _criticalColor;
		else
			num.color = _basiccolor;

		num.text = string.Format(text.ToString());

		_seq = DOTween.Sequence();
		this.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		_seq.Append(this.transform.DOScale(new Vector3(1, 1, 1), sizeUptime));
		_seq.AppendInterval(waitTime);
		_seq.Append(this.transform.DOMoveY(upValue, upTime)).OnComplete(() => Define.GetManager<ResourceManager>().Destroy(this.gameObject));
		_seq.Join(num.DOFade(0, fadeTime));
	}
}
