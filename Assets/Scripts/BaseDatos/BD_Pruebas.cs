using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

[System.Serializable]
public class Trayectorias_PRU_BD{
    // El primer argumento de la tupla es la trayecoria deseada y el segundo es la trayectoria obtenida
    public (List<List<float>>, List<List<float>>) tray_art = (new List<List<float>>(), new List<List<float>>());
    public (List<List<float>>, List<List<float>>) tray_cart = (new List<List<float>>(), new List<List<float>>());
    public List<List<float>> tray_error = new List<List<float>>();
    public List<float> tray_ECM = new List<float>();
}

public static class BD_Pruebas
{

    public const string nombre_archivo = "pruebas.json";

    public static void guardar_trayectorias_prueba(List<List<float>> tc_des, List<List<float>> tart_des, List<List<float>> tc, List<List<float>> tart, List<List<float>> error, List<float> error_cart){

        Trayectorias_PRU_BD tray_return = cargar_trayectorias();
        // Las trayectorias articulares deseadas y obtenidas
        tray_return.tray_art.Item1 = tart_des;
        tray_return.tray_art.Item2 = tart;

        // Las trayectorias cartesinas deseadas y obbtenidas
        tray_return.tray_cart.Item1 = tc_des;
        tray_return.tray_cart.Item2 = tc;

        // El error de cada una de las variables
        tray_return.tray_error = error; 

        // EL error cartesiano
        tray_return.tray_ECM = error_cart;

        guardar_trayectorias(tray_return);
    }

    public static void guardar_trayectorias(Trayectorias_PRU_BD tray){
        string json_data = JsonConvert.SerializeObject(tray,Formatting.Indented);
        File.WriteAllText(nombre_archivo, json_data);
    }

    public static Trayectorias_PRU_BD cargar_trayectorias(){
        string json_data = File.ReadAllText(nombre_archivo);
        return JsonConvert.DeserializeObject<Trayectorias_PRU_BD>(json_data);
    }
}
