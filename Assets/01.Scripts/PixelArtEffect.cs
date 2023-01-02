using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelArtEffect : MonoBehaviour
{
    public Material material;

    private void Awake()
    {
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}
