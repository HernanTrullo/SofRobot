using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;
using RobSof.Assets.Scripts.Interface.Articular;

public class Adaptador_car_puma : MonoBehaviour
{
    public RectTransform content_tray;      // Contiene las trayectorias agregadas
    public RectTransform input_tray;        // Contiene los input de cas trayectorias
    public GameObject val_cart;         // Contiene cada valor asignado de las trayectorias

    public Button btn_agregar;          // Boton agregar
    public Button btn_eliminar;         // Boton para eliminar una trayectoria
    public Button btn_probar;           // Boton probar trayectoria
    public Button btn_P0;               // Boton para enviar a la posicion (cartesiana) inicial
    public Button btn_cargar;           // Boton para cargar varias trayectorias

    public GameObject PUMA_gemelo;       // gemelo digital

    private TMP_InputField [] input_tray_cart = new TMP_InputField[6];

    private List<GameObject> array_val_cart = new List<GameObject>();  // Lista de las trayectorias agregadas
    private int num_max_val_cart = 100;       // Número maximo de trayectorias
    private int num_val_cart = 1;             // Cantidad inicial 


    // Posicion del prefab y aplicación del content
    private Vector3 pos_prefab;
    private Vector2 dim_content;

    // Clase trayectoria
    Trayectoria tray = new Trayectoria();
    private int TIEMPO_MUESTREO = Mathf.RoundToInt(Trayectoria.TIEMPO_MUESTREO*1000);


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
            this.input_tray_cart[i] = input_tray.Find("cart_"+(i+1)).GetComponent<TMP_InputField>();
            this.input_tray_cart[i].text = "" + rangos_arts.posiciones_iniciales_cartesianas[i];
        }

        // Agregar primer elemento tray a la lista array_val _cart
        array_val_cart.Add(val_cart);
        for (int j=0; j<6; j++){
            Acceso_Datos.agregar_valores_tray(array_val_cart[0],"val_art"+(j+1), rangos_arts.posiciones_iniciales_cartesianas[j]);
        }

        // Desactivar el toggle del prefab
        Acceso_Datos.activar_desactivar_toggle(array_val_cart[0], false);

        // Botones a la escucha
        btn_agregar.onClick.AddListener(agregar);
        btn_eliminar.onClick.AddListener(eliminar);
        btn_probar.onClick.AddListener(probar);
        btn_P0.onClick.AddListener(v_Po);
        btn_cargar.onClick.AddListener(cargar);

        // Inicialización de la posición inicia del prefab
        pos_prefab = val_cart.transform.localPosition;
        dim_content = content_tray.sizeDelta;

    }

    void funciones_de_prueba(List<float> array){
        for(int j=0; j<array.Count; j++){
            Debug.Log(""+array[j]);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void agregar(){
        if (num_val_cart < num_max_val_cart){
            array_val_cart.Add(Instantiate(val_cart, content_tray, false));
            array_val_cart[num_val_cart].transform.name = "val_arts"+num_val_cart;

            // Ampliación del content
            dim_content.y +=30;
            content_tray.sizeDelta  = dim_content;

            pos_prefab.y = -30*(num_val_cart+1);
            array_val_cart[num_val_cart].transform.localPosition = pos_prefab;

            // Activar el toggle o checkbox
            Acceso_Datos.activar_desactivar_toggle(array_val_cart[num_val_cart], true);
            
            
            // Agregación del los valores a las trayectorias
            for (int i=0; i<6; i++){
                Acceso_Datos.agregar_valores_tray(array_val_cart[num_val_cart],"val_art"+(i+1), 
                float.Parse(input_tray_cart[i].text)); 
            }
            num_val_cart ++;
        }
    }
    void eliminar(){
        if (num_val_cart > 1){
            List<int> index_a_elim = new List<int>();
            int index = 0;
            foreach (GameObject val_art in array_val_cart){
                Toggle status = val_art.transform.Find("Toggle").GetComponent<Toggle>();
                if (status.isOn){
                    index_a_elim.Add(index);
                }
                index ++;
            }

            index_a_elim.Sort();
            index_a_elim.Reverse();

            foreach(int index_ in index_a_elim){
                Destroy(array_val_cart[index_]);
                array_val_cart.RemoveAt(index_);

                dim_content.y -=30;
                content_tray.sizeDelta  = dim_content;

                num_val_cart --;
            }

            // Actualizar los prefab a las posiciones normales y sus respectivos nombres
            for (int i=1; i<num_val_cart;i++ ){
                array_val_cart[i].transform.name = "val_arts"+i;
                pos_prefab.y = -30*(i+1);
                array_val_cart[i].transform.localPosition = pos_prefab;
            }
        }
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
        for(int i=0; i<num_val_cart; i++){
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
}
