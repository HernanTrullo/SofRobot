using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseDatos : MonoBehaviour
{
    public TMP_Dropdown dropdown_nombre_trays;
    public TextMeshProUGUI descripcion;
    public RectTransform scroll_view;
    public Button btn_refrescar;

    // Los paneles de dialogo eliminar y guardar
    public RectTransform cuadro_dialogo_guardar;
    public RectTransform cuadro_dialogo_eliminar;
    public RectTransform cuadro_dialogo_actualizar;
    public RectTransform cuadro_dialogo_subir;
    public RectTransform cuadro_dialogo_bajar;

    // Los las entradas de nombre y descrpcion
    public TMP_InputField inputf_nombre;
    public TMP_InputField inputf_descripción;
    
    // Script que maneja el scroll view de las trayectorias
    private Scroll_view_tray scrol_view_tray;
    private string NOMBRE_ARCHIVO_BD = "";
    // Start is called before the first frame update
    void Start()
    {
        btn_refrescar.onClick.AddListener(refrescar);

        dropdown_nombre_trays.onValueChanged.AddListener(delegate{
            mostrar_tray(dropdown_nombre_trays);
        });

        // Asignacion del evento correspondiente
        cuadro_dialogo_guardar.transform.GetComponent<Cuadro_dialogo>().btn_si_click_event.AddListener(
            guardar
        );
        cuadro_dialogo_eliminar.transform.GetComponent<Cuadro_dialogo>().btn_si_click_event.AddListener(
            eliminar
        );
        cuadro_dialogo_actualizar.transform.GetComponent<Cuadro_dialogo>().btn_si_click_event.AddListener(
            actualizar);

        cuadro_dialogo_subir.transform.GetComponent<Cuadro_dialogo>().btn_si_click_event.AddListener(
            subir
        );
        cuadro_dialogo_bajar.transform.GetComponent<Cuadro_dialogo>().btn_si_click_event.AddListener(
            bajar
        );

        scrol_view_tray = scroll_view.transform.GetComponent<Scroll_view_tray>();
        scrol_view_tray.inicializar_posiciones(new float[]{0,0,0,0,0,0});
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void set_NOMBRE_ARCHIVO_BD(string NOMBRE_ARCHIVO_BD){
        this.NOMBRE_ARCHIVO_BD = NOMBRE_ARCHIVO_BD; 
    }
    void subir(){
        bd_trayectorias.TRAY_SCROLL_VIEW = Acceso_Datos.return_values_tray(scrol_view_tray.get_array_val_arts());
    }
    void bajar(){
        scrol_view_tray.agregar(bd_trayectorias.TRAY_SCROLL_VIEW);
    }
    void mostrar_tray(TMP_Dropdown dropdown){
        TRAY_BD tray_bd = bd_trayectorias.cargar_tray(NOMBRE_ARCHIVO_BD);  // Se cargan las trayectorias que existen en el archivo .json
        descripcion.text = tray_bd.tray_bd[dropdown.value].descripcion;

        // Se muestra la lista_tray de la primera trayectoria por defecto
        scrol_view_tray.agregar(tray_bd.tray_bd[dropdown.value].tray);

        // Se agregan el nombre y la descripcion del objeto por defecto
        inputf_nombre.text = tray_bd.tray_bd[dropdown_nombre_trays.value].nombre_tray;
        inputf_descripción.text = tray_bd.tray_bd[dropdown_nombre_trays.value].descripcion;
    }
    void refrescar(){
        dropdown_nombre_trays.ClearOptions(); // Se limpian todos los item que haya
        
        TRAY_BD tray_bd = bd_trayectorias.cargar_tray(NOMBRE_ARCHIVO_BD);  // Se cargan las trayectorias que existen en el archivo .json

        List<string> nombre_tray = new List<string>(); // El arreglo que contiene las opciones del desplegable

        // Se recorre todo el atributo list de la clase TRAY_BD en busca de los nombres de las trayectorias
        foreach (Tray_BD tray in tray_bd.tray_bd){
            nombre_tray.Add(tray.nombre_tray); 
        }

        // Se añaden los nombres al dropdown
        dropdown_nombre_trays.AddOptions(nombre_tray);

        // Se muestra la descrpcion de la primera trayectoria por defecto
        descripcion.text = tray_bd.tray_bd[dropdown_nombre_trays.value].descripcion;

        // Se muestra la lista_tray de la primera trayectoria por defecto
        scrol_view_tray.agregar(tray_bd.tray_bd[dropdown_nombre_trays.value].tray);

        // Se agregan el nombre y la descripcion del objeto por defecto
        inputf_nombre.text = tray_bd.tray_bd[dropdown_nombre_trays.value].nombre_tray;
        inputf_descripción.text = tray_bd.tray_bd[dropdown_nombre_trays.value].descripcion;
    }
    void guardar(){
        TRAY_BD tray_bd = bd_trayectorias.cargar_tray(NOMBRE_ARCHIVO_BD); // Se carga la trayectoria

        // Retornar el objeto de tipo list<tray_bd>
        Tray_BD tray_ = new Tray_BD();
        tray_.tray = Acceso_Datos.return_values_tray(scrol_view_tray.get_array_val_arts());
        tray_.descripcion = inputf_descripción.text;
        tray_.nombre_tray = inputf_nombre.text;
        
        // Se agrega al objeto ya retornado
        tray_bd.tray_bd.Add(tray_);

        // Se carga a la base de datos nuevamente
        bd_trayectorias.guardar_tray(tray_bd,NOMBRE_ARCHIVO_BD);

        // Se refreca 
        refrescar();
    }
    void eliminar(){
        TRAY_BD tray_bd = bd_trayectorias.cargar_tray(NOMBRE_ARCHIVO_BD); // Se carga la trayectoria

        // Se elimina la trayectoria asociada al index del dropdown
        tray_bd.tray_bd.RemoveAt(dropdown_nombre_trays.value);

        // Se carga a la base de datos
        bd_trayectorias.guardar_tray(tray_bd,NOMBRE_ARCHIVO_BD);

        // Se refreca 
        refrescar();
    }
    void actualizar(){
        // Se cargan las trayectorias que existen en el archivo .json
        TRAY_BD tray_bd = bd_trayectorias.cargar_tray(NOMBRE_ARCHIVO_BD);  
        // Actualizacion de la trayectoria
        tray_bd.tray_bd[dropdown_nombre_trays.value].tray = Acceso_Datos.return_values_tray(scrol_view_tray.get_array_val_arts());;
        // Actualizacion de la descripcion
        tray_bd.tray_bd[dropdown_nombre_trays.value].descripcion = inputf_descripción.text;
        // Actualizacion del nombre
        tray_bd.tray_bd[dropdown_nombre_trays.value].nombre_tray = inputf_nombre.text;
        // Se guardan los cambios
        bd_trayectorias.guardar_tray(tray_bd,NOMBRE_ARCHIVO_BD);

        refrescar();
    }
}
