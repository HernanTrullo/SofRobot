using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class adaptador_circular : MonoBehaviour
{
    public TMP_InputField inp_posx;
    public TMP_InputField inp_posy;
    public TMP_InputField inp_posz;
    public TMP_InputField inp_radio;
    public Button btn_probar;

    Trayectoria tray = new Trayectoria();
    int TIEMPO_TRAYECTORIA = 3; // sec

    // Start is called before the first frame update
    void Start()
    {
        btn_probar.onClick.AddListener(probar);
        inp_posx.text = 0.5f.ToString();
        inp_posy.text = 0.ToString();
        inp_posz.text = 0.1f.ToString();
        inp_radio.text = 0.2f.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void probar(){
        float posx = float.Parse(inp_posx.text);
        float posy = float.Parse(inp_posy.text);
        float posz = float.Parse(inp_posz.text);
        float radio = float.Parse(inp_radio.text);

        // Se calcula la trayectoria circular
        var TRAY = tray.tray_circular(posx, posy, posz, radio, 6);

        // Se obtinen las trayectorias del primer movimiento
        float [] pos_inicial = new float [6];
        float [] pos_final = new float [6];
        for (int j=0; j<6; j++){
            pos_inicial[j] = Posiciones_robot.POS_CAR[j];
            pos_final[j] = TRAY.Item2[j][0]; // Corresponde al punto cartesiano al que debe llegar el robot primero
        }

        // retornar trayectoria hasta el primer punto del circulo
        var trayectoria = tray.tray_cartesiana(pos_inicial, pos_final, TIEMPO_TRAYECTORIA, 6);
        
        for (int j=0; j<trayectoria.Item1.Count; j++){
            trayectoria.Item1[j].AddRange(TRAY.Item1[j]);
            trayectoria.Item2[j].AddRange(TRAY.Item2[j]);
        }

        StartCoroutine(mover_robot(trayectoria.Item1, trayectoria.Item2));
    }
    string probarstr(float[] values){
        string st = "";
        foreach (var item in values){
            st = st +" % % " + item.ToString();
        }
        return st;
    }

    IEnumerator mover_robot(List<List<float>> tray,List<List<float>> tc){
        //com_udp.iniciar_cliente();
        for (int i=0; i<tray[0].Count; i++ ){
            List<float> tray_send = new List<float>();
            for (int j=0; j<6; j++){
                // Se asignan a las posiciones cartesianas
                Posiciones_robot.POS_CAR[j] = tc[j][i];

                // Se asignas a las posiciones articulares
                Posiciones_robot.POS_ART[j] = tray[j][i];

                // Se llena el tray_send
                tray_send.Add(tray[j][i]);

            }
            //StartCoroutine(com_udp.trasnmitir(tray_send));
            yield return 0;
        }
        //com_udp.cerrar_cliente(); 
    }
}
