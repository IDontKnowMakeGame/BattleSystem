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

	private	Stack<Material> me = new Stack<Material>();

	private List<Material> _materials = new List<Material>();

	[SerializeField]
	private int maxCount = 0;
	private int count = 0;

	private void Awake()
	{
		foreach (HaloMaterial halo in materials)
		{
			_haloMaterial.Add(halo.id, halo.material);
		}

		for (int i = 0; i < renderers.Length; i++)
		{
			_materials.Add(renderers[i].material);
		}
	}

	private void Start()
	{
		if (renderers[count].material == null)
			renderers[count].gameObject.SetActive(false);
	}

	#region Stack으로 계속 추가
	public void EquipmentHalo(ItemID id)
	{
		if (count >= maxCount)
			return;
		if (!renderers[count].gameObject.activeSelf)
			renderers[count].gameObject.SetActive(true);
		count++;

		me.Clear();
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

	public void UnEqupmentHalo()
	{
		if (renderers[count].gameObject.activeSelf)
			renderers[count].gameObject.SetActive(false);

		count--;
	}
	#endregion

	#region List를 사용하여 해결
	public void Equipment(ItemID id, int index)
	{
		if (!renderers[index].gameObject.activeSelf)
			renderers[index].gameObject.SetActive(true);

		_materials[index] = _haloMaterial[id];
	}

	public void UnEquipment(int index)
	{
		renderers[index].gameObject.SetActive(false);
	}
	#endregion
}
