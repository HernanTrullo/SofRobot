using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RobSof.Assets.Scripts.Interface.Articular;
using TMPro;
using System.Threading;
using System.Linq;

public class Adaptador_art_puma : MonoBehaviour
{
    public RectTransform scroll_view;
    public RectTransform content_arts;       // El ambiente content del scrol_arts view
    public GameObject val_arts;               // El prebaf de las trayectorias articulares
    

    public Button btn_agregar;          // Boton agregar
    public Button btn_eliminar;         // Boton para eliminar una trayectoria
    public Button btn_probar;           // Boton para probar una taryectoria en los sliders
    public Button btn_Q0;               // Boton para llevar el robot a la posicion inicial
    public Button btn_cargar;           // Btoton para cargar trayectorias del scroll view


    // Cuadros de dialogo
    public RectTransform cuadro_dialogo_subir;
    public RectTransform cuadro_dialogo_bajar;

    // Base de datos
    public RectTransform BD_panel;

    public GameObject gemelo_digital;
    // driver _robot_ graficas y demas (clase estatica)
    public GameObject obj_driver_rob_inter;

    // Script que maneja el scroll view de las trayectorias
    private Scroll_view_tray scrol_view_tray;

    // Numero de articulaciones 
    private int NUMERO_ARTICULACIONES = 6;

    // Atributos para manejar  los sliders
    private Transform [] sliders;
    private Slider_art[] script_slider;

    // Llamada a la clase trayectoria
    private Trayectoria tray = new Trayectoria();
    private Rangos_arts rangos_arts = new Rangos_arts();
    // Tiempo de la trayectoria
    private int TIEMPO_TRAYECTORIA = 2;


    private DriverRobotInterfaz script_driver_rob_int; 

    // Start is called before the first frame update
    void Start()
    {
        sliders = new Transform[NUMERO_ARTICULACIONES];
        script_slider = new Slider_art[NUMERO_ARTICULACIONES];
        

        // script que maneja el driver rob_inter
        script_driver_rob_int = obj_driver_rob_inter.GetComponent<DriverRobotInterfaz>();

        // Se coloca el boton a la escucha
        btn_agregar.onClick.AddListener(agregar);
        btn_eliminar.onClick.AddListener(eliminar);
        btn_probar.onClick.AddListener(probar);
        btn_Q0.onClick.AddListener(v_Qo);
        btn_cargar.onClick.AddListener(cargar);

        // Se agregan los eventos del cuadro de dialogo
        cuadro_dialogo_subir.transform.GetComponent<Cuadro_dialogo>().btn_si_click_event.AddListener(
            subir
        );
        cuadro_dialogo_bajar.transform.GetComponent<Cuadro_dialogo>().btn_si_click_event.AddListener(
            bajar
        );
        
        // Inicialización de los sliders, gemelo, taryectoias
        for (int i = 0; i<NUMERO_ARTICULACIONES; i++){
            // Sliders
            sliders[i]= content_arts.transform.Find("slider"+(i+1));
            script_slider[i] = sliders[i].GetComponent<Slider_art>();

            // Asignacion de los rangos de cada articulacion
            script_slider[i].set_max(rangos_arts.rango_arts[i, 1]);
            script_slider[i].set_min(rangos_arts.rango_arts[i, 0]);

            // Posiciones iniciales sliders
            script_slider[i].set_value(""+rangos_arts.posiciones_iniciales[i].ToString("0.##"));

            // Gemelo digital
            gemelo_digital.GetComponent<Gemelo_digital>().rotar_articulación(i, rangos_arts.posiciones_iniciales[i]);
        }
        
        // Se obtiene el script que maneja el scroll view internamente
        scrol_view_tray = scroll_view.transform.GetComponent<Scroll_view_tray>();
        scrol_view_tray.inicializar_posiciones(rangos_arts.posiciones_iniciales);

        // Se ininializa el nombre del archivo 
        BD_panel.transform.GetComponent<BaseDatos>().set_NOMBRE_ARCHIVO_BD(bd_trayectorias.BD_PUMA_ART);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void agregar(){
       // Retornar los valored de los slider
       List<float> values = new List<float>();
       foreach(Slider_art slider in script_slider){
            values.Add(slider.get_value());
        }
        // Se llama al método agregar del scrollview
        scrol_view_tray.agregar(values);

    }
    void eliminar(){
       scrol_view_tray.eliminar();
    }

    void probar (){
        // Retornar los valores de las sliders
        float [] pos_inicial = new float [NUMERO_ARTICULACIONES];
        float [] pos_final = new float [NUMERO_ARTICULACIONES];

        // Asignan los valores finales e iniciales
        for (int j=0; j<NUMERO_ARTICULACIONES; j++){
            pos_inicial[j] = Posiciones_robot.POS_ART[j];
            pos_final[j] = script_slider[j].get_value();
        }

        // Se retornan "NUMERO DE ARICULACIONES" trayectorias
        var trayectoria = tray.tray_articular(pos_inicial, pos_final, TIEMPO_TRAYECTORIA, NUMERO_ARTICULACIONES);

        // Se inicializa la corrutina
        StartCoroutine(script_driver_rob_int.mover_robot(trayectoria.Item1, trayectoria.Item2));
    }

    void v_Qo(){
        for (int j=0; j<NUMERO_ARTICULACIONES; j++){
            script_slider[j].set_value(""+rangos_arts.posiciones_iniciales[j].ToString("0.##"));
        }
    }

    void cargar(){
        List<List<float>> values = new List<List<float>>();

        // Se agregan los valores en donde esta el robot actualmente
        values.Add(Posiciones_robot.POS_ART);

        // Se obtienen los valores de cada movimiento
        List<GameObject> array_val_arts = scrol_view_tray.get_array_val_arts();
        for(int i=0; i<array_val_arts.Count; i++){
            List<float> _value = new List<float>();
            for (int j=0; j<NUMERO_ARTICULACIONES; j++){
                _value.Add(Acceso_Datos.obtener_valores_tray(array_val_arts[i], "val_art"+(j+1)));
            }
            values.Add(_value);
        }
        // Se obtienen las trayectorias
        List<List<List<float>>> tray_gen = new List<List<List<float>>>();
        List<List<List<float>>> tray_gen_car = new List<List<List<float>>>();

        for (int i=0; i<values.Count-1; i++){
            var TRAY = tray.tray_articular(values[i].ToArray(), values[i+1].ToArray(), TIEMPO_TRAYECTORIA, NUMERO_ARTICULACIONES);
            tray_gen.Add(TRAY.Item1);
            tray_gen_car.Add(TRAY.Item2);
        }

        // Se inicializa la corrutina
        StartCoroutine(script_driver_rob_int.mover_robot_tray(tray_gen, tray_gen_car));
    }

    void subir(){
        bd_trayectorias.TRAY_SCROLL_VIEW = Acceso_Datos.return_values_tray(scrol_view_tray.get_array_val_arts());
    }

    void bajar(){
        scrol_view_tray.agregar(bd_trayectorias.TRAY_SCROLL_VIEW);
    }
}
