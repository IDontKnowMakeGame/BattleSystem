using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct HaloMaterial
{
	public ItemID id;
	public Material material;
}

public class HaloRenderer : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer[] renderers;

	[SerializeField]
	private HaloMaterial[] materials;

	private Dictionary<ItemID, Material> _haloMaterial = new Dictionary<ItemID, Material>();

	[SerializeField]
	private int maxCount = 0;
	private int count = 0;

	private void Awake()
	{
		foreach (HaloMaterial halo in materials)
		{
			_haloMaterial.Add(halo.id, halo.material);
		}
	}

	private void Start()
	{
		if (renderers[count].material == null)
			renderers[count].gameObject.SetActive(false);
	}

	public void EquipmentHalo(ItemID id)
	{
		if (count >= maxCount)
			return;
		if (!renderers[count].gameObject.activeSelf)
			renderers[count].gameObject.SetActive(true);
		count++;

		Stack<Material> me = new Stack<Material>();

		for(int i = renderers.Length-1; i >= 0; i--)
		{
			me.Push(renderers[i].material);
		}

		me.Push(_haloMaterial[id]);

		int max = me.Count;
		for (int i = 0; i< max-1; i++)
		{
			Material meme = me.Pop();
			renderers[i].material = meme;
		}
	}

	public void UnEqupmentHalo(ItemID id)
	{
		//currentRenderer.gameObject.SetActive(false);
	}
}
