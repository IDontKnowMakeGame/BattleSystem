using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusicOn : MonoBehaviour
{
	public void MusicOn(int i)
	{
		Define.GetManager<SoundManager>().Play("BackGround/BossMusic" + i, Define.Sound.Bgm);
	}
}
