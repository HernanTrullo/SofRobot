using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class interface_predef : MonoBehaviour
{
    public GameObject panel_cir;
    public GameObject panel_senoidal;
    public TMP_Dropdown dp_ventana;
    

    // Llamada a la clase trayectoria
    private Trayectoria tray = new Trayectoria();

    // Start is called before the first frame update
    void Start()
    {
        dp_ventana.onValueChanged.AddListener(CambiarVentana);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CambiarVentana(int opcion)
    {
        panel_cir.SetActive(opcion == 0);
        panel_senoidal.SetActive(opcion == 1);
    }
}
