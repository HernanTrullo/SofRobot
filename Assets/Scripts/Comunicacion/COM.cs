using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;

public class COM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UduinoManager.Instance.BaudRate = 115200;
        UduinoManager.Instance.pinMode(13, PinMode.Output);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            UduinoManager.Instance.digitalWrite(13, State.HIGH);
        }
        else if (Input.GetKeyUp(KeyCode.Space)){
            UduinoManager.Instance.digitalWrite(13, State.LOW);
        }
    }
}
