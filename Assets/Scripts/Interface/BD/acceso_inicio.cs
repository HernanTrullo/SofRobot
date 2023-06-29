using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class acceso_inicio : MonoBehaviour
{
    public RectTransform cuadro_dialogo_ingresar;
    public RectTransform cuadro_dialogo_guardar;
    public Button btn_ingresar;
    public Button btn_guardar;

    // Los las entradas de nombre y descrpcion
    public TMP_InputField inputf_user;
    public TMP_InputField inputf_clave;

    // 
    public GameObject ventana_principal;
    public GameObject ventana_incio;

    // Start is called before the first frame update
    void Start()
    {
        btn_ingresar.onClick.AddListener(
            ingresar
        );
        
        cuadro_dialogo_guardar.transform.GetComponent<Cuadro_dialogo>().btn_si_click_event.AddListener(
            guardar
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ingresar(){
        if (acceso_user.hay_usuario(inputf_user.text, inputf_clave.text)){
            mostrar_ventana();
        }
        else{
            cuadro_dialogo_ingresar.gameObject.SetActive(true);
        }
    }
    void guardar(){
        // Se cargan los usuarios 
        Usuarios_BD us = acceso_user.cargar_usuarios();

        // Se agrega el usuario a los ya existentes
        Usuario user = new Usuario();
        user.user = inputf_user.text;
        user.pass = inputf_clave.text;

        // Se agregan a los usuario existentes
        us.users.Add(user);
        acceso_user.guardar_usuario(us);

        // Limpiar inputText
        inputf_clave.text = "";
        inputf_user.text = "";
    }
    void mostrar_ventana(){

        ventana_incio.SetActive(false);
        ventana_principal.SetActive(true);

        // Limpiar inputText
        inputf_clave.text = "";
        inputf_user.text = "";
    }
}
