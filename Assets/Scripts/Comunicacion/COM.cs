using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;
using System.Threading;


public class COM : MonoBehaviour
{
    public GameObject Uduino;

    private UduinoManager uduinoManager;
    // Start is called before the first frame update
    void Start()
    {
        uduinoManager = Uduino.transform.GetComponent<UduinoManager>();
        uduinoManager.pinMode(11, PinMode.PWM);
        StartCoroutine(funcion());
    }

    IEnumerator funcion(){
        

        for (int i=0; i<255; i++){

            uduinoManager.analogWrite(11, i);
            yield return new WaitForSeconds(0.1f); 
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
