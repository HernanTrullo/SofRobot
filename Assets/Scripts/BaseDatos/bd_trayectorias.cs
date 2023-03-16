using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


[System.Serializable]
public class Tray_BD{
    public List<List<float>> tray = new List<List<float>>();
    public string nombre_tray = "";
    public string descripcion = "";
}

[System.Serializable]
public class TRAY_BD{
    public List<Tray_BD> tray_bd = new List<Tray_BD>();
}


public static class bd_trayectorias
{
    // Atributo global que se encarga de guardar temporalmente las trayectorias
    public static List<List<float>> TRAY_SCROLL_VIEW;

    public const string BD_PUMA_ART = "tray_articular.json";
    public const string BD_PUMA_CART = "tray_cartesiana.json";

    public static void guardar_tray(TRAY_BD tray_bd, string nombre_archivo){
        string json_data = JsonConvert.SerializeObject(tray_bd, Formatting.Indented);
        File.WriteAllText(nombre_archivo, json_data);
        
    }
    public static TRAY_BD cargar_tray(string nombre_archivo){
        if (!File.Exists(nombre_archivo)){
            return null;
        }
        string json_data = File.ReadAllText(nombre_archivo);
        return JsonConvert.DeserializeObject<TRAY_BD>(json_data);
    }
}
