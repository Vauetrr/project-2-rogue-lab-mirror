using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

struct Occluder
{
    public Shader shader;
    public IEnumerator fader; 
}

public class PostProcessing : MonoBehaviour
{
    [SerializeField] private Material lowHealth;
    [SerializeField] private Material depthOfField;
    [SerializeField] private Texture2D ditherTex;
    [SerializeField] private float ditherRadius = 1.0f;
    [SerializeField] private float ditherFeather = 1.0f;
    [SerializeField] private float ditherCatchRadius = 1.0f;
    private Dictionary<Transform, Occluder> occluders = new Dictionary<Transform, Occluder>();

    void OnEnable()
    {
        Debug.Log("Enable postprocessing");
        Scene scene = SceneManager.GetActiveScene();
        switch (scene.name)
        {
            case "tutorial":
                depthOfField.SetFloat("_BlurSize", 0.015f);
                depthOfField.SetFloat("_Samples", 2f);
                depthOfField.SetFloat("_StdDeviation", 0.3f);
                depthOfField.SetFloat("_FocDis", 45f);
                depthOfField.SetFloat("_FocRng", 76f);
                break;
            case "Test":
                depthOfField.SetFloat("_BlurSize", 0.015f);
                depthOfField.SetFloat("_Samples", 2f);
                depthOfField.SetFloat("_StdDeviation", 0.006f);
                depthOfField.SetFloat("_FocDis", 42f);
                depthOfField.SetFloat("_FocRng", 100f);
                break;
            default:
                depthOfField.SetFloat("_BlurSize", 0.01f);
                depthOfField.SetFloat("_Samples", 2f);
                depthOfField.SetFloat("_StdDeviation", 0.02f);
                depthOfField.SetFloat("_FocDis", 42f);
                depthOfField.SetFloat("_FocRng", 26f);
                break;
        }
    }

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth;
    }

    void Update()
    {
        int layerMask = (1 << 6) | (1 << 8) | (1 <<9); // only check for collisions with "walkable" or "dither"
        var dir = (transform.parent.position + new Vector3(-ditherCatchRadius, 5.0f, -ditherCatchRadius)) - transform.position;
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, ditherCatchRadius, dir, dir.magnitude, layerMask);
        Dictionary<Transform, Occluder> newOccluders = new Dictionary<Transform, Occluder>();

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
                    // update material shader
                    var mat = rend.material;
                    mat.shader = Shader.Find("Custom/Dither");
                    mat.SetTexture("_DitherPattern", ditherTex);

                    // add occluder
                    var value = new Occluder();
                    value.shader = mat.shader;
                    value.fader = Fade(mat);
                    newOccluders.Add(hit.transform, value);
                    StartCoroutine(value.fader);
                }
            }
        }

        // un-fade old occluders
        foreach (KeyValuePair<Transform, Occluder> occluder in occluders)
        {
            StopCoroutine(occluder.Value.fader);
            StartCoroutine(Unfade(occluder));
        }
        occluders = newOccluders;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // temp texts
        var healthEffect = RenderTexture.GetTemporary(source.width, source.height);
        var coc = RenderTexture.GetTemporary(
            source.width, source.height, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear
        );
        depthOfField.SetTexture("_CoCTex", coc);

        // blit low health shader
        Graphics.Blit(source, healthEffect, lowHealth, 0);

        // blit dof
        Graphics.Blit(healthEffect, coc, depthOfField, 0);
        Graphics.Blit(healthEffect, source, depthOfField, 1);
        Graphics.Blit(source, destination, depthOfField, 2);

        RenderTexture.ReleaseTemporary(healthEffect);
        RenderTexture.ReleaseTemporary(coc);
    }

    IEnumerator Fade(Material mat) 
    {
        var radius = 0.0f;
        var feather = 0.0f;
        while (radius < ditherRadius || feather < ditherFeather)
        {
            if (radius > ditherRadius) radius = ditherRadius;
            if (feather > ditherFeather) feather = ditherFeather;
            mat.SetFloat("_Radius", radius);
            mat.SetFloat("_Feather", feather);
            radius += Time.deltaTime;
            feather += 2 * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator Unfade(KeyValuePair<Transform, Occluder> occluder)
    {
        var rend = occluder.Key.GetComponent<Renderer>();
        if (rend)
        {
            var mat = rend.material;
            var radius = mat.GetFloat("_Radius");
            var feather = mat.GetFloat("_Feather");
            while (radius > 0 || feather > 0)
            {
                if (radius < 0) radius = 0;
                if (feather < 0) feather = 0;
                mat.SetFloat("_Radius", radius);
                mat.SetFloat("_Feather", feather);
                radius -= Time.deltaTime;
                feather -= 2 * Time.deltaTime;
                yield return null;
            }
            mat.shader = occluder.Value.shader;
        }
    }
}
