using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Globalization;
using System.Linq;


public class COM_upd : MonoBehaviour
{
    // Start is called before the first frame update
    string ip = "169.254.75.5";
    int puerto = 5000;
    UdpClient cliente = new UdpClient();
    IPEndPoint servidorEndPoint;

    List<float> values_sensor = new List<float>();

    void Start()
    {
        servidorEndPoint = new IPEndPoint(IPAddress.Parse(ip), puerto);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator trasnmitir (List<float> values){
        string send = "";
        foreach (float value in values){
            send = send + value.ToString("F2").Replace(",", ".") + "%"; 
        }
        send = send.Remove(send.Length -1); // Se elimina el ultimo caracter 
        byte[] dtaBytes = Encoding.UTF8.GetBytes(send);
        cliente.Send(dtaBytes, dtaBytes.Length);

        actualizar_valores_sensor();
        yield return 0;
    }

    public void iniciar_cliente(){
        cliente = new UdpClient();
        cliente.Connect(servidorEndPoint);
    }
    public void cerrar_cliente(){
        cliente.Close();
    }

    void actualizar_valores_sensor(){
        values_sensor.Clear();
        byte [] respuesta = cliente.Receive(ref servidorEndPoint);
        string respuestaS = Encoding.UTF8.GetString(respuesta);
        respuestaS = respuestaS.Remove(respuestaS.Length -1);

        string[] valueStr = respuestaS.Split("%");

        foreach (var valuestr in valueStr){
            values_sensor.Add(float.Parse(valuestr,CultureInfo.InvariantCulture));
        }
    }

    public List<float> get_values(){

        return values_sensor;
    }
}
