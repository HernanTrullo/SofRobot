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
            setvalue(slider1.value.ToString("0.##"));});
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void setvalue(string value){
        this.value.text = value;
    }
    public float get_value(){
        return slider1.value;
    }
    public void set_max(string value){
        this.max.text = value;  
    }
    public void set_min(string value){
        this.min.text = value;  
    }
}
