using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossMusicOn : MonoBehaviour
{
	public void MusicOn(int i)
	{
		Define.GetManager<SoundManager>().Play("BackGround/BossMusic" + i, Define.Sound.Bgm);
	}
	public void BackgroundBGMOn()
	{
        Define.GetManager<SoundManager>().Play("Sounds/BackGround/KHJScene1BackGround", Define.Sound.Bgm);
    }
}
