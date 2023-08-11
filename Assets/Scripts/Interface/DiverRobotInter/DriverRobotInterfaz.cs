using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverRobotInterfaz : MonoBehaviour
{

    public GameObject obj_com_udp; 
    public GameObject panel_graf;

    private COM_upd com_udp;
    private ControlCTC controller = new ControlCTC();


    private int TIEMPO_MUESTREO = 0;
    private Trayectoria tray = new Trayectoria();
    private interfaz_grafica graf_script;
    // Start is called before the first frame update
    void Start()
    {
        
        com_udp = obj_com_udp.GetComponent<COM_upd>();
        graf_script = panel_graf.GetComponent<interfaz_grafica>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator mover_robot(List<List<float>> tray, List<List<float>> tc){
        Posiciones_robot.error.Clear();
        com_udp.iniciar_cliente();
        for (int i=0; i<tray[0].Count; i++ ){
            List<float> tray_send = new List<float>();
            float [] vel = new float[]{0,0,0,0,0,0};

            for (int j=0; j<tray.Count; j++){

                // Actualización de las posiciones del gemelo
                Posiciones_robot.POS_ART[j]= tray[j][i];
                Posiciones_robot.POS_CAR[j] = tc[j][i];
                
                // Calulo de velocidad
                vel[j] = (Posiciones_robot.POS_ART_REAL[j]- Posiciones_robot.POS_ART_PAS_REAL[j])/0.01f;
                
                // Se llena el tray_send
                tray_send.Add(tray[j][i]);
            }

            // Algoritmo de control
            var values=  (controller.retunrTorques(Posiciones_robot.POS_ART, Posiciones_robot.POS_ART_REAL, vel));
            tray_send.AddRange(values.Item1);
            Posiciones_robot.error.Add(values.Item2);

            StartCoroutine(com_udp.trasnmitir(tray_send));

            // Se actualizan las posiciones
            Posiciones_robot.POS_ART_PAS_REAL = Posiciones_robot.POS_ART_REAL;
            Posiciones_robot.POS_ART_REAL = com_udp.get_values();


            yield return TIEMPO_MUESTREO;   
        }
        com_udp.cerrar_cliente();

        // Se cargan a la interfaz de las gráficas
        graf_script.asignar_trays(this.tray.Transpuesta(Posiciones_robot.error, true));
    }

    public IEnumerator mover_robot_tray (List<List<List<float>>> tray,List<List<List<float>>> tc ){
        for (int k=0; k<tray.Count; k++){ // el de las trayectorias
            Posiciones_robot.error.Clear();
            com_udp.iniciar_cliente();
            for (int i=0; i<tray[0].Count; i++ ){
                List<float> tray_send = new List<float>();
                float [] vel = new float[]{0,0,0,0,0,0};

                for (int j=0; j<tray.Count; j++){

                    // Actualización de las posiciones del gemelo
                    Posiciones_robot.POS_ART[j]= tray[k][j][i];
                    Posiciones_robot.POS_CAR[j] = tc[k][j][i];
                    
                    // Calulo de velocidad
                    vel[j] = (Posiciones_robot.POS_ART_REAL[j]- Posiciones_robot.POS_ART_PAS_REAL[j])/0.01f;
                    
                    // Se llena el tray_send
                    tray_send.Add(tray[k][j][i]);
                }

                // Algoritmo de control
                var values=  (controller.retunrTorques(Posiciones_robot.POS_ART, Posiciones_robot.POS_ART_REAL, vel));
                tray_send.AddRange(values.Item1);
                Posiciones_robot.error.Add(values.Item2);

                StartCoroutine(com_udp.trasnmitir(tray_send));

                // Se actualizan las posiciones
                Posiciones_robot.POS_ART_PAS_REAL = Posiciones_robot.POS_ART_REAL;
                Posiciones_robot.POS_ART_REAL = com_udp.get_values();


                yield return TIEMPO_MUESTREO;   
            }
            com_udp.cerrar_cliente();

            // Se cargan a la interfaz de las gráficas
            graf_script.asignar_trays(this.tray.Transpuesta(Posiciones_robot.error, true));
        }

    }
}
