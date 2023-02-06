using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RobSof.Assets.Scripts.Interface.Articular;
using TMPro;

public class Adaptador_art_puma : MonoBehaviour
{
    
    public RectTransform content_trays;      // El ambiente content del scroll_trays view
    public RectTransform content_arts;       // El ambiente content del scrol_arts view
    public GameObject val_arts;       // El prebaf de las trayectorias articulares
    public Button btn_agregar;          // Boton agregar
    public Button btn_eliminar;         // Boton para eliminar una trayectoria
   
   
    private Transform [] sliders = new Transform[6];
    private Slider_art[] script_slider = new Slider_art[6];

    private Rangos_arts rangos_arts = new Rangos_arts();

    private List<GameObject> array_val_arts;
    private int num_max_val_arts = 100;
    private int num_val_arts = 1;

    // Posicion del prefab y aplicación del content
    private Vector3 pos_prefab;
    private Vector2 dim_content;

    // Start is called before the first frame update
    void Start()
    {
        // Se instancia la lista de los prebab de las trayectorias
        array_val_arts = new List<GameObject>();

        // Desactivar el checkbox del primer objeto
        array_val_arts.Add(val_arts);
        this.activar_desactivar_toggle(array_val_arts[0], false);

        // Se coloca el boton a la escucha
        btn_agregar.onClick.AddListener(agregar);
        btn_eliminar.onClick.AddListener(eliminar);

        // Inicialización de la posición inicia del prefab
        pos_prefab = val_arts.transform.localPosition;
        dim_content = content_trays.sizeDelta;
        
        // Inicialización de los sliders
        for (int i = 0; i<6; i++){
            sliders[i]= content_arts.transform.Find("slider"+(i+1));
            script_slider[i] = sliders[i].GetComponent<Slider_art>();
            
            // Asignacion de los rangos de cada articulacion
            script_slider[i].set_max(rangos_arts.rango_arts[i, 1]);
            script_slider[i].set_min(rangos_arts.rango_arts[i, 0]);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void agregar(){
        if (num_val_arts < num_max_val_arts){
            array_val_arts.Add(Instantiate(val_arts, content_trays, false));
            array_val_arts[num_val_arts].transform.name = "val_arts"+num_val_arts;

            // Ampliación del content
            dim_content.y +=20;
            content_trays.sizeDelta  = dim_content;

            pos_prefab.y = -20*(num_val_arts+1);
            array_val_arts[num_val_arts].transform.localPosition = pos_prefab;

            // Activar el toggle o checkbox
            this.activar_desactivar_toggle(array_val_arts[num_val_arts], true);
            
            
            // Agregación del los valores a las trayectorias
            for (int i=0; i<6; i++){
                agregar_valores_tray(array_val_arts[num_val_arts],"val_art"+(i+1), script_slider[i].get_value()); 
            }
            num_val_arts ++;
        }
    }
    void eliminar(){
       if (num_val_arts > 1){
            List<int> index_a_elim = new List<int>();
            int index = 0;
            foreach (GameObject val_art in array_val_arts){
                Toggle status = val_art.transform.Find("Toggle").GetComponent<Toggle>();
                if (status.isOn){
                    index_a_elim.Add(index);
                }
                index ++;
            }

            index_a_elim.Sort();
            index_a_elim.Reverse();

            foreach(int index_ in index_a_elim){
                Destroy(array_val_arts[index_]);
                array_val_arts.RemoveAt(index_);
                num_val_arts --;
            }

            // Actualizar los prefab a las posiciones normales y sus respectivos nombres
            for (int i=1; i<num_val_arts;i++ ){
                array_val_arts[i].transform.name = "val_arts"+i;
                pos_prefab.y = -20*(i+1);
                array_val_arts[i].transform.localPosition = pos_prefab;
            }

            dim_content.y = 50+20*(num_val_arts-1);
            content_trays.sizeDelta  = dim_content;
        }
    }

    void activar_desactivar_toggle(GameObject art_vals, bool interactable){
        Toggle status = art_vals.transform.Find("Toggle").GetComponent<Toggle>();
        status.interactable = interactable;
    }
    void agregar_valores_tray(GameObject arts_vals, string name, float f_value){
        TextMeshProUGUI value = arts_vals.transform.Find(name).GetComponent<TextMeshProUGUI>();
        value.text = f_value.ToString("0.##");
    }
}
