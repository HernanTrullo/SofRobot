using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Globalization;



public class COM_upd : MonoBehaviour
{
    // Start is called before the first frame update
    string ip = "192.168.210.40";
    int puerto = 5000;
    UdpClient cliente = new UdpClient();
    IPEndPoint servidorEndPoint;
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
        byte[] dtaBytes = Encoding.UTF8.GetBytes(send);
        cliente.Send(dtaBytes, dtaBytes.Length);

        byte [] respuesta = cliente.Receive(ref servidorEndPoint);
        string respuestaS = Encoding.UTF8.GetString(respuesta);
        yield return 0;
    }

    public void iniciar_cliente(){
        cliente = new UdpClient();
        cliente.Connect(servidorEndPoint);
    }
    public void cerrar_cliente(){
        cliente.Close();
    }
}
