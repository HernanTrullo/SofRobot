using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public static class Acceso_Datos
{
    public static void activar_desactivar_toggle(GameObject art_vals, bool interactable){
        Toggle status = art_vals.transform.Find("Toggle").GetComponent<Toggle>();
        status.interactable = interactable;
    }
    public static void agregar_valores_tray(GameObject arts_vals, string name, float f_value){
        TextMeshProUGUI value = arts_vals.transform.Find(name).GetComponent<TextMeshProUGUI>();
        value.text = f_value.ToString("0.##");
    }
    public static float obtener_valores_tray(GameObject arts_vals, string name){
        TextMeshProUGUI value = arts_vals.transform.Find(name).GetComponent<TextMeshProUGUI>();
        return float.Parse(value.text);
    }
    public static List<List<float>> return_values_tray(List<GameObject> array_val_arts){
        List<List<float>> values = new List<List<float>>();
        // Se obtienen los valores de cada movimiento
        for(int i=0; i<array_val_arts.Count; i++){
            List<float> _value = new List<float>();
            for (int j=0; j<6; j++){
                // Se coloca i+1 debido a que la trayectoria inicial siempre se manetiene
                _value.Add(obtener_valores_tray(array_val_arts[i], "val_art"+(j+1)));
            }
            values.Add(_value);
        }
        return values;
    }
}
