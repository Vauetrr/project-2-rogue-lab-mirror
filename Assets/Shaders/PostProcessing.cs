using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessing : MonoBehaviour
{
    [SerializeField] private Material lowHealth;
    [SerializeField] private Texture2D ditherTex;
    [SerializeField] private float ditherAlpha = 0.1f;
    [SerializeField] private float ditherCatchRadius = 1.0f;
    private Dictionary<Transform, Shader> occluders = new Dictionary<Transform, Shader>();

    void Update()
    {
        int layerMask = (1 << 6) | (1 << 8); // only check for collisions with "walkable" or "dither"
        var dir = (transform.parent.position + new Vector3(0, 5.0f, 0)) - transform.position;
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, ditherCatchRadius, dir, dir.magnitude, layerMask);
        Dictionary<Transform, Shader> newOccluders = new Dictionary<Transform, Shader>();

        // fade out each occluder
        foreach (RaycastHit hit in hits)
        {
            if (occluders.ContainsKey(hit.transform))
            {
                // still occluding, so don't un-fade this occluder
                newOccluders.Add(hit.transform, occluders[hit.transform]);
                occluders.Remove(hit.transform);
            } 
            else
            {
                Renderer rend = hit.transform.GetComponent<Renderer>();
                if (rend)
                {
                    var mat = rend.material;
                    newOccluders.Add(hit.transform, mat.shader);
                    mat.shader = Shader.Find("Custom/Dither");
                    mat.SetTexture("_DitherPattern", ditherTex);
                    StartCoroutine(Fade(mat));
                }
            }
        }

        // un-fade old occluders
        foreach (KeyValuePair<Transform, Shader> occluder in occluders)
        {
            StartCoroutine(Unfade(occluder));
        }
        occluders = newOccluders;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var tempText = RenderTexture.GetTemporary(source.width, source.height);
        Graphics.Blit(source, tempText, lowHealth, 0);
        Graphics.Blit(tempText, source, lowHealth, 1);
        Graphics.Blit(source, destination, lowHealth, 2);
        RenderTexture.ReleaseTemporary(tempText);
    }

    IEnumerator Fade(Material mat) {
        var alpha = mat.GetFloat("_Alpha");
        for (float a = alpha; a >= ditherAlpha; a -= Time.deltaTime)
        {
            mat.SetFloat("_Alpha", a);
            yield return null;
        }
    }

    IEnumerator Unfade(KeyValuePair<Transform, Shader> occluder)
    {
        var rend = occluder.Key.GetComponent<Renderer>();
        if (rend)
        {
            var mat = rend.material;
            var alpha = mat.GetFloat("_Alpha");
            for (float a = alpha; a < 1.0; a += Time.deltaTime)
            {
                mat.SetFloat("_Alpha", a);
                yield return null;
            }
            mat.shader = occluder.Value;
        }
    }
}
