using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            LoadingSceneController.Instnace.StartCoroutine(LoadingSceneController.Instnace.LoadScene("InGame"));
        }
    }
}
