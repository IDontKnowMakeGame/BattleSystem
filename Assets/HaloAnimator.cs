using ArrayExtensions;
using Data;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum HaloAnimationState
{
	None,
	Stop,
	Idle,
	Attack,
	Move,
}

public class HaloAnimator : MonoBehaviour
{
	private Renderer _currentRenderer;

	private HaloAnimationContainor _haloAnimationsInfo;

	public HaloAnimationContainor animationsInfo { get { return _haloAnimationsInfo; } set { _haloAnimationsInfo = value; } }

	public ItemID rederId => _haloAnimationsInfo.id;

	private HaloAnimation _currentAnimation;

	#region State 변수들
	private HaloAnimationState _state = HaloAnimationState.None;
	private HaloAnimationState _currentState
	{
		get
		{
			return _state;
		}
		set
		{
			_state = value;

			if (_state == HaloAnimationState.None || _state == HaloAnimationState.Stop)
				return;

			foreach (HaloAnimation animation in _haloAnimationsInfo.animatoins)
			{
				if (animation.state == _state)
				{
					_currentAnimation = animation;
					StartCoroutine(AnimationPlay());
					Debug.Log("AnimationPlay");
					break;
				}
			}
			//여기서 바꿔주는거 실행
		}
	}
	public HaloAnimationState State => _currentState;
	#endregion

	[NonSerialized]
	public int index = 0;
	[NonSerialized]
	public bool isFinished = false;

	public Coroutine currentCoroutine;

	[SerializeField]
	private float maxYvalue = 0f;
	[SerializeField]
	private float minYvalue = 0f;
	[SerializeField]
	private float yTimer = 0f;
	[SerializeField]
	private float waitTimer = 0f;

	private bool isloop = false;

	#region LifeCycle
	public void Awake()
	{
		_currentRenderer = GetComponentInChildren<Renderer>();
		_currentRenderer.gameObject.SetActive(false);
	}

	public void OnDisable()
	{
		if (currentCoroutine != null)
			StopCoroutine(currentCoroutine);
		if (sequence != null)
			sequence.Kill();
	}
	#endregion

	public IEnumerator AnimationPlay()
	{
		index = -1;
		isFinished = false;

		while (true)
		{
			if (index != -1)
				yield return new WaitForSeconds(_currentAnimation.delay[index]);

			index++;
			if (index == _currentAnimation.fps)
			{
				index = -1;
				isFinished = true;
				break;
			}

			var offset = ((float)_currentAnimation.texture.width / _currentAnimation.fps) / _currentAnimation.texture.width;
			_currentAnimation.materials[0].SetTexture("_BaseMap", _currentAnimation.texture);
			_currentAnimation.materials[0].SetTextureOffset("_BaseMap", Vector2.right * (offset * index));
			_currentAnimation.materials[0].SetTextureScale("_BaseMap", new Vector2(offset, 1f));
			_currentAnimation.materials[0].SetTexture("_MainTex", _currentAnimation.texture);
			_currentAnimation.materials[0].SetTextureOffset("_MainTex", Vector2.right * (offset * index));
			_currentAnimation.materials[0].SetTextureScale("_MainTex", new Vector2(offset, 1f));
			_currentAnimation.materials[0].SetTexture("_MainTex", _currentAnimation.texture);
			_currentAnimation.materials[0].SetVector("_Offset", Vector2.right * (offset * index));
			_currentAnimation.materials[0].SetVector("_Tiling", new Vector2(offset, 1f));

			_currentRenderer.materials = _currentAnimation.materials;
		}

		if (isFinished)
		{
			if (_currentAnimation.isLoop)
				currentCoroutine = StartCoroutine(AnimationPlay());
			else
				currentCoroutine = null;
			yield return null;
		}
	}

	#region 애니메이션 제어 로직

	public void PlayAnimation(HaloAnimationState state)
	{
		_currentState = state;
	}

	public void StopAnimation()
	{
		_currentState = HaloAnimationState.None;
	}

	public void AnimatorStart()
	{
		PlayAnimation(HaloAnimationState.Idle);
		_currentRenderer.gameObject.SetActive(true);
	}
	public void AnimatorStop()
	{
		StopAnimation();
		_currentRenderer.gameObject.SetActive(false);
	}

	#endregion

	Sequence sequence = null;

	public void SetTexture()
	{
		//Debug.Log(_currentRenderer.gameObject.activeSelf);
		_currentRenderer.gameObject.SetActive(true);
		_currentRenderer.materials = _haloAnimationsInfo.animatoins[0].materials;
		//_currentRenderer.

		Vector3 vec = this.transform.localPosition;
		vec.y = minYvalue;
		this.transform.localPosition = vec;

		sequence = DOTween.Sequence();
		sequence.Append(this.transform.DOLocalMoveY(maxYvalue, yTimer).SetEase(Ease.InOutSine));
		sequence.SetLoops(-1,LoopType.Yoyo);
		sequence.Play();
	}

	public void DelTexture()
	{
		sequence?.Kill();
		Debug.Log(_currentRenderer.gameObject.activeSelf);
		_currentRenderer.gameObject.SetActive(false);
	}
}
