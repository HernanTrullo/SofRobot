using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Adaptador_car_puma : MonoBehaviour
{
    public RectTransform content_tray;      // Contiene las trayectorias agregadas
    public RectTransform input_tray;        // Contiene los input de cas trayectorias
    public GameObject val_cart;         // Contiene cada valor asignado de las trayectorias

    public Button btn_agregar;          // Boton agregar
    public Button btn_eliminar;         // Boton para eliminar una trayectoria
   
    private TMP_InputField [] input_tray_cart = new TMP_InputField[6];

    private List<GameObject> array_val_cart = new List<GameObject>();  // Lista de las trayectorias agregadas
    private int num_max_val_cart = 100;       // Número maximo de trayectorias
    private int num_val_cart = 1;             // Cantidad inicial 


    // Posicion del prefab y aplicación del content
    private Vector3 pos_prefab;
    private Vector2 dim_content;


    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<6; i++){
            this.input_tray_cart[i] = input_tray.Find("cart_"+(i+1)).GetComponent<TMP_InputField>();
        }

        // Agregar primer elemento tray a la lista array_val _cart
        array_val_cart.Add(val_cart);

        // Desactivar el toggle del prefab
        this.activar_desactivar_toggle(array_val_cart[0], false);

        // Botones a la escucha
        btn_agregar.onClick.AddListener(agregar);
        btn_eliminar.onClick.AddListener(eliminar);

        // Inicialización de la posición inicia del prefab
        pos_prefab = val_cart.transform.localPosition;
        dim_content = content_tray.sizeDelta;

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
            dim_content.y +=20;
            content_tray.sizeDelta  = dim_content;

            pos_prefab.y = -20*(num_val_cart+1);
            array_val_cart[num_val_cart].transform.localPosition = pos_prefab;

            // Activar el toggle o checkbox
            this.activar_desactivar_toggle(array_val_cart[num_val_cart], true);
            
            
            // Agregación del los valores a las trayectorias
            for (int i=0; i<6; i++){
                agregar_valores_tray(array_val_cart[num_val_cart],"val_art"+(i+1), 
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
                num_val_cart --;
            }

            // Actualizar los prefab a las posiciones normales y sus respectivos nombres
            for (int i=1; i<num_val_cart;i++ ){
                array_val_cart[i].transform.name = "val_arts"+i;
                pos_prefab.y = -20*(i+1);
                array_val_cart[i].transform.localPosition = pos_prefab;
            }

            dim_content.y = 50+20*(num_val_cart-1);
            content_tray.sizeDelta  = dim_content;
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
