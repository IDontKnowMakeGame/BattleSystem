using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSetting : MonoBehaviour
{
	[SerializeField]
	private Texture2D cursorTexture;

	private void Awake()
	{
		DontDestroyOnLoad(this);
	}

	private void Start()
	{
		Cursor.SetCursor(cursorTexture,Vector3.zero, CursorMode.Auto);
	}
}
