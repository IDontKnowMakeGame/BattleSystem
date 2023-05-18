using Core;
using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;


[Serializable]
public struct HaloAnimationContainor
{
	public ItemID id;
	public HaloAnimation[] animatoins;
}

[Serializable]
public struct HaloAnimation
{
	public HaloAnimationState state;
	public Texture texture;
	public Material[] materials;
	
	public float[] delay;
	public int fps => delay.Length;
	public bool isLoop;
}

public class HaloRenderer : MonoBehaviour
{
	[SerializeField]
	private List<HaloAnimator> _haloAnimators = new List<HaloAnimator>();

	[SerializeField]
	private HaloAnimationContainor[] _haloAnimationInfos;

	private Dictionary<ItemID, HaloAnimationContainor> _animations = new Dictionary<ItemID, HaloAnimationContainor>();

	private	Stack<HaloAnimationContainor> me = new Stack<HaloAnimationContainor>();


	[SerializeField]
	private int maxCount = 0;
	private int count = 0;

	private Vector3 vec = new Vector3(0, 0.2f, 0);

	private void Awake()
	{
		foreach (HaloAnimationContainor halo in _haloAnimationInfos)
		{
			_animations.Add(halo.id, halo);
		}
		vec = this.transform.localPosition;
	}

	private void Start()
	{
		foreach (HaloAnimator halo in _haloAnimators)
		{
			if (halo.State == HaloAnimationState.None)
				halo.AnimatorStop();
		}
	}

	private void Update()
	{
		//float x =;
		//float z = ;

		Vector3 camDic = InGame.CamDirCheck(Vector3.back) / 5;
		this.transform.localPosition = vec + camDic;
		if(camDic.x >= 0.2 || camDic.x <= -0.2)
		this.transform.localRotation = Quaternion.Euler(new Vector3(0, camDic.x > 0.2f ? 90 : -90, 0));
		else
			this.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

	}

	#region Stack으로 계속 추가
	public void EquipmentHalo(ItemID id)
	{
		if (count >= maxCount)
			return;
		if (_haloAnimators[count].State == HaloAnimationState.None)
			_haloAnimators[count].AnimatorStart();
		count++;

		me.Clear();
		for(int i = _haloAnimators.Count-1; i >= 0; i--)
		{
			me.Push(_haloAnimators[i].animationsInfo);
		}

		me.Push(_animations[id]);

		int max = me.Count;
		for (int i = 0; i< max-1; i++)
		{
			HaloAnimationContainor meme = me.Pop();
			_haloAnimators[i].animationsInfo = meme;
		}
	}

	public void UnEqupmentHalo()
	{
		if (_haloAnimators[count].State != HaloAnimationState.None)
			_haloAnimators[count].AnimatorStop();

		count--;
	}
	#endregion

	#region List를 사용하여 해결
	public void Equipment(ItemID id, int index)
	{
		_haloAnimators[index].animationsInfo = _animations[id];
		if (_haloAnimators[index].State == HaloAnimationState.None)
			_haloAnimators[index].AnimatorStart();
	}

	public void UnEquipment(int index)
	{
		_haloAnimators[index].AnimatorStop();
	}
	#endregion

	public void SetHalo(ItemID id)
	{
		Debug.Log((ItemID)id);
		Debug.Log(_haloAnimators[0].animationsInfo.animatoins[0].materials[0].name);
		_haloAnimators[0].animationsInfo = _animations[id];
		_haloAnimators[0].SetTexture();
	}

	public void DelHalo()
	{
		_haloAnimators[0].DelTexture();
	}
}
