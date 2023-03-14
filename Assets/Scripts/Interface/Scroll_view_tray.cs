using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RobSof.Assets.Scripts.Interface.Articular;

public class Scroll_view_tray : MonoBehaviour
{
    public RectTransform content_trays;
    public GameObject val_arts; 

    private List<GameObject> array_val_arts = new List<GameObject>();
    private int num_max_val_arts = 100;
    private int num_val_arts = 1;

    private Vector3 pos_prefab;
    private Vector2 dim_content;
    private float dim_content_y; 

    private Rangos_arts rangos_arts = new Rangos_arts();

    private int NUMERO_ARTICULACIONES = 6;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void inicializar_posiciones(float [] posiciones_iniciales){
        array_val_arts.Add(val_arts);
        // Inicialización de la posición inicia del prefab
        pos_prefab = val_arts.transform.localPosition;
        dim_content = content_trays.sizeDelta;
        dim_content_y = content_trays.sizeDelta.y;
        
        Acceso_Datos.activar_desactivar_toggle(array_val_arts[0], false);
        for (int i=0; i<NUMERO_ARTICULACIONES; i++){
            Acceso_Datos.agregar_valores_tray(array_val_arts[0],"val_art"+(i+1), posiciones_iniciales[i]); 
        }
    }
    public void agregar(List<float> values){
        if (num_val_arts < num_max_val_arts){
            array_val_arts.Add(Instantiate(val_arts, content_trays, false));
            array_val_arts[num_val_arts].transform.name = "val_arts"+num_val_arts;

            // Ampliación del content
            dim_content.y +=30;
            content_trays.sizeDelta  = dim_content;

            pos_prefab.y = -30*(num_val_arts+1);
            array_val_arts[num_val_arts].transform.localPosition = pos_prefab;

            // Activar el toggle o checkbox
            Acceso_Datos.activar_desactivar_toggle(array_val_arts[num_val_arts], true);
            
            
            // Agregación del los valores a las trayectorias
            for (int i=0; i<6; i++){
                Acceso_Datos.agregar_valores_tray(array_val_arts[num_val_arts],"val_art"+(i+1),values[i]); 
            }
            num_val_arts ++;
        }
    }
    public void agregar(List<List<float>> tray){
        this.num_val_arts = 1;
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
            Acceso_Datos.activar_desactivar_toggle(array_val_arts[num_val_arts], true);
            this.num_val_arts ++;
        }
        // Agregación del los valores a las trayectorias
        for (int num_val_arts = 1; num_val_arts<tray.Count; num_val_arts++){
            for (int i=0; i<6; i++){
                Acceso_Datos.agregar_valores_tray(array_val_arts[num_val_arts],"val_art"+(i+1), tray[num_val_arts][i]); 
            }
        }

        // Ampliación del content
        dim_content.y = dim_content_y + 30*(tray.Count-1);
        content_trays.sizeDelta  = dim_content;
    }

    public void eliminar(){
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

            // Se reorganizan los indices para eliminarlos
            index_a_elim.Sort();
            index_a_elim.Reverse();
            foreach(int index_ in index_a_elim){
                Destroy(array_val_arts[index_]);
                array_val_arts.RemoveAt(index_);
                dim_content.y -=30;
                content_trays.sizeDelta  = dim_content;
                num_val_arts --;
            }

            // Actualizar los prefab a las posiciones normales y sus respectivos nombres
            for (int i=1; i<num_val_arts;i++ ){
                array_val_arts[i].transform.name = "val_arts"+i;
                pos_prefab.y = -30*(i+1);
                array_val_arts[i].transform.localPosition = pos_prefab;
            }
            
        }
    }

    public List<GameObject> get_array_val_arts(){
        return array_val_arts;
    }
}
