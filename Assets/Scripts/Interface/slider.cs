using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class slider : MonoBehaviour
{
    public Slider slider1;
    public TextMeshProUGUI value;
    public TextMeshProUGUI min;
    public TextMeshProUGUI max;
    // Start is called before the first frame update
    void Start()
    {
        slider1.onValueChanged.AddListener(delegate{setvalue(slider1.value.ToString("0.##"));});
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void setvalue(string value){
        this.value.text = value;
    }
}
