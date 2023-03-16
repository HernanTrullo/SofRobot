using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using RobSof.Assets.Scripts.Interface.Articular;

public class Adaptador_car_puma : MonoBehaviour
{
    public RectTransform scroll_view;      // Contiene las trayectorias agregadas
    public RectTransform input_tray;        // Contiene los input de cas trayectorias

    public Button btn_agregar;          // Boton agregar
    public Button btn_eliminar;         // Boton para eliminar una trayectoria
    public Button btn_probar;           // Boton probar trayectoria
    public Button btn_P0;               // Boton para enviar a la posicion (cartesiana) inicial
    public Button btn_cargar;           // Boton para cargar varias trayectorias

    // Cuadros de dialogo
    public RectTransform cuadro_dialogo_subir;
    public RectTransform cuadro_dialogo_bajar;


    public GameObject PUMA_gemelo;       // gemelo digital
    public RectTransform BD_panel;

    private TMP_InputField [] input_tray_cart = new TMP_InputField[6];

    // Clase trayectoria
    Trayectoria tray = new Trayectoria();
    private int TIEMPO_MUESTREO = Mathf.RoundToInt(Trayectoria.TIEMPO_MUESTREO*1000);

    // Script que maneja el scroll view de las trayectorias
    private Scroll_view_tray scrol_view_tray;

    // Script que maneja el gemelo digital
    private Gemelo_digital PUMA_script;

    // Modelo del puma
    PUMA_modelo pm_mod = new PUMA_modelo();

    // rangos de las articulaciones
    Rangos_arts rangos_arts = new Rangos_arts();

    // Tiempo TRAYECTORIA
    int TIEMPO_TRAYECTORIA = 3;
    // Start is called before the first frame update
    void Start()
    {
        // Se instancia el script que maneja el gameObject (PUMA) 
        PUMA_script = PUMA_gemelo.GetComponent<Gemelo_digital>();
        for(int i=0; i<6; i++){
            input_tray_cart[i] = input_tray.Find("cart_"+(i+1)).GetComponent<TMP_InputField>();
            input_tray_cart[i].text = "" + rangos_arts.posiciones_iniciales_cartesianas[i];
        }
        // Botones a la escucha
        btn_agregar.onClick.AddListener(agregar);
        btn_eliminar.onClick.AddListener(eliminar);
        btn_probar.onClick.AddListener(probar);
        btn_P0.onClick.AddListener(v_Po);
        btn_cargar.onClick.AddListener(cargar);

        // Se agregan los eventos del cuadro de dialogo
        cuadro_dialogo_subir.transform.GetComponent<Cuadro_dialogo>().btn_si_click_event.AddListener(
            subir
        );
        cuadro_dialogo_bajar.transform.GetComponent<Cuadro_dialogo>().btn_si_click_event.AddListener(
            bajar
        );
        
        // Se obtiene el script que maneja el scroll view internamente
        scrol_view_tray = scroll_view.transform.GetComponent<Scroll_view_tray>();
        scrol_view_tray.inicializar_posiciones(rangos_arts.posiciones_iniciales_cartesianas);

        // Se inicializa el nombre de la base de datos
        BD_panel.transform.GetComponent<BaseDatos>().set_NOMBRE_ARCHIVO_BD(bd_trayectorias.BD_PUMA_CART);
    }
    
    // Update is called once per frame
    void Update(){

    }
    void agregar(){
       // Retornar valores de los input text cartesianos
       List<float> values = new List<float>();
       foreach(TMP_InputField input in input_tray_cart){
            values.Add(float.Parse(input.text));
       }
       // Se envían al scrol view para ser agregados
       scrol_view_tray.agregar(values);
    }

    void eliminar(){
       scrol_view_tray.eliminar();
    }

    void probar(){
        // Posicion final e inicial
        float [] pos_inicial = new float [6];
        float [] pos_final = new float [6];

        // Asignan los valores finales e iniciales
        for (int j=0; j<6; j++){
            pos_inicial[j] = Posiciones_robot.POS_CAR[j];
            pos_final[j] = float.Parse(input_tray_cart[j].text);
        }

        // retornar trayectoria
        var trayectoria = tray.tray_cartesiana(pos_inicial, pos_final, TIEMPO_TRAYECTORIA, 6);
        // Se inicializa la corrutina
        StartCoroutine(mover_robot(trayectoria.Item1, trayectoria.Item2));
    }

    void v_Po(){
        float [] pos_ini = rangos_arts.po_ini_art_cart.ToArray();

        for (int i=0; i<6; i++){
            pos_ini[i] = pos_ini[i]*Mathf.Rad2Deg;
        }
        
        var tray_= tray.tray_articular(Posiciones_robot.POS_ART.ToArray(),pos_ini, TIEMPO_TRAYECTORIA-1, 6);
        StartCoroutine(mover_robot(tray_.Item1, tray_.Item2));
    }

    IEnumerator mover_robot(List<List<float>> tray,List<List<float>> tc){
        for (int i=0; i<tray[0].Count; i++ ){
            for (int j=0; j<6; j++){
                PUMA_script.rotar_articulación(j, tray[j][i]);
                // Se asignan a las posiciones cartesianas
                Posiciones_robot.POS_CAR[j] = tc[j][i];
                Posiciones_robot.pos_car[j].text = tc[j][i].ToString("0.##");

                // Se asignas a las posiciones articulares
                Posiciones_robot.POS_ART[j] = tray[j][i];
                Posiciones_robot.pos_art[j].text = tray[j][i].ToString("0.##");
            } 
            Thread.Sleep(TIEMPO_MUESTREO);
            yield return 0;   
        }
    }

    IEnumerator mover_robot_tray (List<List<List<float>>> tray,List<List<List<float>>> tc ){
        for (int k=0; k<tray.Count; k++){ // el de las trayectorias
            for (int i=0; i<tray[0][0].Count; i++ ){
                for (int j=0; j<tray[0].Count; j++){
                    PUMA_script.rotar_articulación(j, tray[k][j][i]);
                    // Actualización Posiciones Articulares
                    Posiciones_robot.POS_ART[j]= tray[k][j][i];
                    Posiciones_robot.pos_art[j].text = tray[k][j][i].ToString("0.##");

                    // Actualización Posiciones Cartesianas
                    Posiciones_robot.POS_CAR[j] = tc[k][j][i];
                    Posiciones_robot.pos_car[j].text = tc[k][j][i].ToString("0.##");
                } 
                Thread.Sleep(TIEMPO_MUESTREO);
                yield return 0;   
            }
        }
    }

    void cargar(){
        List<List<float>> values = new List<List<float>>();
        // Se agregan los valores en donde esta el robot actualmente
        values.Add(Posiciones_robot.POS_CAR);

        // Se obtienen los valores de cada movimiento
        List<GameObject> array_val_cart = scrol_view_tray.get_array_val_arts();
        for(int i=0; i<array_val_cart.Count; i++){
            List<float> _value = new List<float>();
            for (int j=0; j<6; j++){
                _value.Add(Acceso_Datos.obtener_valores_tray(array_val_cart[i], "val_art"+(j+1)));
            }
            values.Add(_value);
        }
        // Se obtienen las trayectorias
        List<List<List<float>>> tray_gen = new List<List<List<float>>>();
        List<List<List<float>>> tray_gen_car = new List<List<List<float>>>();

        for (int i=0; i<values.Count-1; i++){
            var TRAY = tray.tray_cartesiana(values[i].ToArray(), values[i+1].ToArray(), TIEMPO_TRAYECTORIA, 6);
            tray_gen.Add(TRAY.Item1);
            tray_gen_car.Add(TRAY.Item2);
        }

        // Se inicializa la corrutina
        StartCoroutine(mover_robot_tray(tray_gen, tray_gen_car));
    }
    void subir(){
        bd_trayectorias.TRAY_SCROLL_VIEW = Acceso_Datos.return_values_tray(scrol_view_tray.get_array_val_arts());
    }

    void bajar(){
        scrol_view_tray.agregar(bd_trayectorias.TRAY_SCROLL_VIEW);
    }
}
