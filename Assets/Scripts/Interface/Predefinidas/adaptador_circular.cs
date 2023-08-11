using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RobSof.Assets.Scripts.Interface.Articular;

public class adaptador_circular : MonoBehaviour
{
    public TMP_InputField inp_posx;
    public TMP_InputField inp_posy;
    public TMP_InputField inp_posz;
    public TMP_InputField inp_radio;
    public Button btn_probar;
    public Button btn_q0;

    public GameObject obj_driver_rob_int; 

    Trayectoria tray = new Trayectoria();
    int TIEMPO_TRAYECTORIA = 3; // sec
    
    private interfaz_grafica graf_script;
    
    Rangos_arts rangos_arts = new Rangos_arts(); 
    private DriverRobotInterfaz scrip_driver_rob_inter;


    // Start is called before the first frame update
    void Start()
    {
        btn_probar.onClick.AddListener(probar);
        btn_q0.onClick.AddListener(v_Po);

        // Se inicializa el driver que manera las gráficas, controlador.
        scrip_driver_rob_inter = obj_driver_rob_int.GetComponent<DriverRobotInterfaz>();

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

        StartCoroutine(scrip_driver_rob_inter.mover_robot(trayectoria.Item1, trayectoria.Item2));
    }
    string probarstr(float[] values){
        string st = "";
        foreach (var item in values){
            st = st +" % % " + item.ToString();
        }
        return st;
    }
    void v_Po(){

        float [] pos_ini = rangos_arts.po_ini_art_cart.ToArray();

        for (int i=0; i<6; i++){
            pos_ini[i] = pos_ini[i]*Mathf.Rad2Deg;
        }
        
        var tray_= tray.tray_articular(Posiciones_robot.POS_ART.ToArray(),pos_ini, TIEMPO_TRAYECTORIA+1, 6);
        StartCoroutine(scrip_driver_rob_inter.mover_robot(tray_.Item1, tray_.Item2));

    }
}
