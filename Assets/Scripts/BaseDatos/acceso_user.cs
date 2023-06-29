using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;


[System.Serializable]
public class Usuario{
    public string user = "";
    public string pass = "";
}

[System.Serializable]
public class Usuarios_BD{
    public List<Usuario> users = new List<Usuario>();
}


public static class acceso_user
{
    public const string nombre_archivo = "user.json";

    public static bool hay_usuario(string user, string pass){
        bool respuesta = false;

        Usuarios_BD us= cargar_usuarios();

        foreach (Usuario users in us.users){
            if (user == users.user && pass == users.pass){
                respuesta = true;
            }
        }
        return respuesta;
    }

    public static Usuarios_BD cargar_usuarios(){
        string json_data = File.ReadAllText(nombre_archivo);
        return JsonConvert.DeserializeObject<Usuarios_BD>(json_data);
    }

    public static void guardar_usuario(Usuarios_BD user){
        string json_data = JsonConvert.SerializeObject(user, Formatting.Indented);
        File.WriteAllText(nombre_archivo, json_data);
    }

}
