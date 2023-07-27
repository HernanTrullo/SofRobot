using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Input_text : MonoBehaviour
{
    public TMP_InputField input_field;
    public string value_default;

    // Start is called before the first frame update
    void Start()
    {
        input_field.onEndEdit.AddListener(input_validate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void input_validate(string input){
        if (string.IsNullOrEmpty(input_field.text)){
            input_field.text = value_default;
        }
    }
}
