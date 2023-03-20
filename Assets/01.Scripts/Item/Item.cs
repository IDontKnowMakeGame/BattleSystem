using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// item의 기본적인 구조를 관장해주는 클래스
/// </summary>
public class Item
{
	public ItemInfo info;

	/// <summary>
	/// Start보다 빠르다던가 awake보다 빠르다거나
	/// </summary>
	public virtual void Init()
	{

	}
}
