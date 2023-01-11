using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Manager;

public class DamagePopUp : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro num;


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

	public void DamageText(int text, Vector3 pos)
	{
		num.alpha = 1;
		Vector2 vec = Random.insideUnitCircle;
		transform.position = new Vector3(pos.x, Random.Range(pos.y, pos.y + 1f), pos.z);

		if (text >= 10)
			num.color = Color.red;
		else
			num.color = Color.yellow;

		num.text = string.Format(text.ToString());
		Sequence mySequence = DOTween.Sequence();
		int a = vec.x > 0 ? 1 : -1;
		mySequence.Append(transform.DOMoveX((a * xPower + vec.x) + transform.position.x, HoriSpeed).SetEase(Ease.Linear));
		mySequence.Join(transform.DOMoveY(Mathf.Abs(transform.position.y) + yPower, PowerSpeed).SetEase(Ease.Linear)).AppendCallback(() => {
			num.DOFade(0, 0.35f);
		});
		mySequence.Append(transform.DOMoveY(Down, DownSpeed).SetEase(Ease.Linear)).AppendCallback(() => { GameManagement.Instance.GetManager<ResourceManagers>().Destroy(this.gameObject); });
	}

}
