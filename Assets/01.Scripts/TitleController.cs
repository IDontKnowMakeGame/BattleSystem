using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    private bool enableLoad = false;

    void Update()
    {
        if(enableLoad && Input.anyKeyDown)
        {
            LoadingSceneController.Instnace.LoadScene("Lobby");
        }
    }

    public void SetEnableLoad()
    {
        enableLoad = true;
    }
}
