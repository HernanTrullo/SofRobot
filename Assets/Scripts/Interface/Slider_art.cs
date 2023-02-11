using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slider_art : MonoBehaviour
{
    public Slider slider1;
    public TextMeshProUGUI value;
    public TextMeshProUGUI min;
    public TextMeshProUGUI max;
    // Start is called before the first frame update
    void Start()
    {
        slider1.onValueChanged.AddListener(delegate{
            slider1.value = Mathf.Round(slider1.value*100f)/100f;
            set_value(slider1.value.ToString("0.##"));});

                
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void set_value(string value){
        this.value.text = value;
        slider1.value = float.Parse(value);
    }
    public float get_value(){
        return slider1.value;
    }
    public void set_max(float value){
        this.max.text = ""+value; 
        this.slider1.maxValue= value; 
    }
    public void set_min(float value){
        this.min.text = ""+value;
        this.slider1.minValue = value;  
    }
}
