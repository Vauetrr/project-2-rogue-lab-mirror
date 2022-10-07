using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessing : MonoBehaviour
{
    [SerializeField] private Material lowHealth;
    [SerializeField] private Material dither;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // var tempText = RenderTexture.GetTemporary(source.width, source.height);
        // Graphics.Blit(source, tempText, lowHealth, 0);
        // Graphics.Blit(tempText, source, lowHealth, 1);
        // Graphics.Blit(source, destination, lowHealth, 2);
        // RenderTexture.ReleaseTemporary(tempText);
        Graphics.Blit(source, destination, dither);
    }
}
