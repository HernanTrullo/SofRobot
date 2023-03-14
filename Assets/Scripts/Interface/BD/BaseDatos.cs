using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseDatos : MonoBehaviour
{
    public TMP_Dropdown dropdown_nombre_trays;
    public TextMeshProUGUI descripcion;
    public RectTransform content_trays;
    public GameObject val_arts;
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

    // El arreglo que controla el scrollview
    private List<GameObject> array_val_arts = new List<GameObject>();


    // Posicion del prefab y aplicación del content
    private Vector3 pos_prefab;
    private Vector2 dim_content;
    private float dim_content_y; 
    

    // Start is called before the first frame update
    void Start()
    {
        btn_refrescar.onClick.AddListener(refrescar);

        dropdown_nombre_trays.onValueChanged.AddListener(delegate{
            mostrar_tray(dropdown_nombre_trays);
        });
        // Se agrega al primer objeto
        array_val_arts.Add(val_arts);
        Acceso_Datos.activar_desactivar_toggle(array_val_arts[0], false);

        // Inicializacion del dimcontent
        pos_prefab = val_arts.transform.localPosition;
        dim_content = content_trays.sizeDelta;
        dim_content_y = content_trays.sizeDelta.y;

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

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void agregar(List<List<float>> tray){
        // Se eliminan los gameObject para volver a actualizar la lista
        for(int index_= array_val_arts.Count-1; index_>0; index_--){
            Destroy(array_val_arts[index_]);
            array_val_arts.RemoveAt(index_);
        }

        for (int num_val_arts = 1; num_val_arts<tray.Count; num_val_arts++){
            array_val_arts.Add(Instantiate(val_arts, content_trays, false));
            array_val_arts[num_val_arts].transform.name = "val_arts"+num_val_arts;

            pos_prefab.y = -30*(num_val_arts+1);
            array_val_arts[num_val_arts].transform.localPosition = pos_prefab;

            // Activar el toggle o checkbox
            Acceso_Datos.activar_desactivar_toggle(array_val_arts[num_val_arts], false);

        }
        // Agregación del los valores a las trayectorias
        for (int num_val_arts = 0; num_val_arts<tray.Count; num_val_arts++){
            for (int i=0; i<6; i++){
                Acceso_Datos.agregar_valores_tray(array_val_arts[num_val_arts],"val_art"+(i+1), tray[num_val_arts][i]); 
            }
        }

        // Ampliación del content
        dim_content.y = dim_content_y + 30*(tray.Count-1);
        content_trays.sizeDelta  = dim_content;
    }
    void subir(){
        bd_trayectorias.TRAY_SCROLL_VIEW = Acceso_Datos.return_values_tray(array_val_arts);
    }
    void bajar(){
        agregar(bd_trayectorias.TRAY_SCROLL_VIEW);
    }
    void mostrar_tray(TMP_Dropdown dropdown){
        TRAY_BD tray_bd = bd_trayectorias.cargar_tray();  // Se cargan las trayectorias que existen en el archivo .json
        descripcion.text = tray_bd.tray_bd[dropdown.value].descripcion;

        // Se muestra la lista_tray de la primera trayectoria por defecto
        agregar(tray_bd.tray_bd[dropdown.value].tray);

        // Se agregan el nombre y la descripcion del objeto por defecto
        inputf_nombre.text = tray_bd.tray_bd[dropdown_nombre_trays.value].nombre_tray;
        inputf_descripción.text = tray_bd.tray_bd[dropdown_nombre_trays.value].descripcion;
    }
    void refrescar(){
        dropdown_nombre_trays.ClearOptions(); // Se limpian todos los item que haya
        
        TRAY_BD tray_bd = bd_trayectorias.cargar_tray();  // Se cargan las trayectorias que existen en el archivo .json

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
        agregar(tray_bd.tray_bd[dropdown_nombre_trays.value].tray);

        // Se agregan el nombre y la descripcion del objeto por defecto
        inputf_nombre.text = tray_bd.tray_bd[dropdown_nombre_trays.value].nombre_tray;
        inputf_descripción.text = tray_bd.tray_bd[dropdown_nombre_trays.value].descripcion;
    }
    void guardar(){
        
    }
    void eliminar(){
        
    }
    void actualizar(){
        TRAY_BD tray_bd = bd_trayectorias.cargar_tray();  // Se cargan las trayectorias que existen en el archivo .json

        // Devolver la lista de los valores de la trayectoria a actualizar
        List<List<float>> values = new List<List<float>>();
        // Se obtienen los valores de cada movimiento
        foreach(GameObject val_arts in array_val_arts){
            List<float> _value = new List<float>();
            for (int j=0; j<6; j++){
                // Se coloca i+1 debido a que la trayectoria inicial siempre se manetiene
                _value.Add(Acceso_Datos.obtener_valores_tray(val_arts, "val_art"+(j+1)));
            }
            values.Add(_value);
        }

        // Actualizacion de la trayectoria
        tray_bd.tray_bd[dropdown_nombre_trays.value].tray = values;
        // Actualizacion de la descripcion
        tray_bd.tray_bd[dropdown_nombre_trays.value].descripcion = inputf_descripción.text;
        // Actualizacion del nombre
        tray_bd.tray_bd[dropdown_nombre_trays.value].nombre_tray = inputf_nombre.text;

        // Se guardan los cambios
        bd_trayectorias.guardar_tray(tray_bd);

        refrescar();
    }
}
