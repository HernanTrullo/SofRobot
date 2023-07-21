using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class interface_predef : MonoBehaviour
{
    public GameObject panel_cir;
    public GameObject panel_senoidal;

    public void CambiarVentana(int opcion)
    {
        panel_cir.SetActive(opcion == 0);
        panel_senoidal.SetActive(opcion == 1);
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
