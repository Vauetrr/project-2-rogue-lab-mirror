using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    [SerializeField] private TMP_Text text;
    
    
    public void SetHealthBar(float percentile) 
    {
        slider.value = percentile;
    }

    public void SetHealthBar(float current, float max)
    {
        slider.value = (current / max);
        text.SetText(current.ToString() + "/" + max.ToString());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
