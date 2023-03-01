using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class Tray_bd{
    public List<string> nombre_tray;
    public List<List<List<float>>> tray_point; 
}

public static class bd_trayectorias
{
    private const string nombre_archivo = "nombre_tray.json";

    public static void guardar_nombre_tray(){
        
    }
    public static void guardar_tray(Tray_bd tray_bd){
        string json_data = JsonUtility.ToJson(tray_bd);
        File.WriteAllText(nombre_archivo, json_data);
    }
    public static Tray_bd cargar_tray(){
        if (!File.Exists(nombre_archivo)){
            return null;
        }
        string json_data = File.ReadAllText(nombre_archivo);
        return JsonUtility.FromJson<Tray_bd>(json_data);
    }
}
