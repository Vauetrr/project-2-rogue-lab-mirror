using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessing : MonoBehaviour
{
    [SerializeField] private Material postprocessMaterial;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var tempText = RenderTexture.GetTemporary(source.width, source.height);
        Graphics.Blit(source, tempText, postprocessMaterial, 0);
        Graphics.Blit(tempText, source, postprocessMaterial, 1);
        Graphics.Blit(source, destination, postprocessMaterial, 2);
        RenderTexture.ReleaseTemporary(tempText);
    }
}
