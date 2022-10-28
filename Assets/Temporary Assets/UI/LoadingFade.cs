// how to fade an image
// https://forum.unity.com/threads/simple-ui-animation-fade-in-fade-out-c.439825/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingFade : MonoBehaviour {

    [SerializeField] private Image img;
    private float fIn = 1.0f;
    private float fOut = 2.5f;

    private float col = 34 / 255.0f;
    public IEnumerator StartFade(bool start, float delay)
    {
        yield return new WaitForSeconds(delay);
        yield return (StartCoroutine(FadeScreen(start)));
    }
 
    IEnumerator FadeScreen(bool start)
    {
        for (float i = (start?0:1); (start?i<=1:i>=0); i += Time.deltaTime/(start?fIn:-fOut))
        {
            img.color = new Color(col, col, col, i);
            yield return null;
        }
        img.color = new Color(col, col, col, (start?1:0));   
    }
}