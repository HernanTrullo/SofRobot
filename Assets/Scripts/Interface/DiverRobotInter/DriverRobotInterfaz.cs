using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class DriverRobotInterfaz : MonoBehaviour
{

    public GameObject obj_com_udp; 
    public GameObject panel_graf;

    private COM_upd com_udp;
    private ControlCTC controller = new ControlCTC();


    private int TIEMPO_MUESTREO = 0;
    private Trayectoria tray = new Trayectoria();
    private interfaz_grafica graf_script;

    private PUMA_modelo puma_modelo = new PUMA_modelo();
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
        Posiciones_robot.error_cart.Clear();
        Posiciones_robot.POS_CART_REAL.Clear();
        Posiciones_robot.POS_ART_REAL_ARRAY.Clear();

        com_udp.iniciar_cliente();
        for (int i=0; i<tray[0].Count; i++ ){

            List<float> tray_send = new List<float>();
            float [] vel = new float[]{0,0,0,0,0,0};

            for (int j=0; j<tray.Count; j++){

                // Actualización de las posiciones del gemelo
                Posiciones_robot.POS_ART[j]= tray[j][i];
                Posiciones_robot.POS_CAR[j] = tc[j][i];
                
                // Calulo de velocidad
                vel[j] = (Posiciones_robot.POS_ART_REAL[j]- Posiciones_robot.POS_ART_PAS_REAL[j])/0.001f;
                
                // Se llena el tray_send
                tray_send.Add(tray[j][i]);
            }

            // Algoritmo de control
            var values=  (controller.retunrTorques(Posiciones_robot.POS_ART, Posiciones_robot.POS_ART_REAL, vel));
            tray_send.AddRange(values.Item1);
            Posiciones_robot.error.Add(values.Item2);


            var pos_cart_deseadas = Posiciones_robot.POS_CAR.GetRange(0,3);
            var pos_cart_obte = puma_modelo.mgd_puma(Posiciones_robot.POS_ART_REAL.Select(x=>x*Mathf.Deg2Rad).ToArray());
            Posiciones_robot.POS_CART_REAL.Add(pos_cart_obte);
            Posiciones_robot.POS_ART_REAL_ARRAY.Add(new List<float>(Posiciones_robot.POS_ART_REAL));
            Posiciones_robot.error_cart.Add(Mathf.Sqrt (pos_cart_deseadas.Zip(pos_cart_obte.GetRange(0,3), (a,b) => Mathf.Pow(a-b,2)).Sum()));
            
            // Se envia las posiciones al controlador del robot PUMA (Raspberry)
            StartCoroutine(com_udp.trasnmitir(tray_send));

            // Se actualizan las posiciones
            Posiciones_robot.POS_ART_PAS_REAL = Posiciones_robot.POS_ART_REAL;
            Posiciones_robot.POS_ART_REAL = com_udp.get_values();

            yield return TIEMPO_MUESTREO;   
        }


        com_udp.cerrar_cliente();

        // Se cargan a la interfaz de las gráficas
        graf_script.asignar_trays(this.tray.Transpuesta(Posiciones_robot.error, true));
        graf_script.asignar_ECM(Posiciones_robot.error_cart);
        
        BD_Pruebas.guardar_trayectorias_prueba(tc, tray, this.tray.Transpuesta(Posiciones_robot.POS_CART_REAL, true), 
                    this.tray.Transpuesta(Posiciones_robot.POS_ART_REAL_ARRAY, true),
                    this.tray.Transpuesta(Posiciones_robot.error, true), Posiciones_robot.error_cart);
    }

    public IEnumerator mover_robot_tray (List<List<List<float>>> tray,List<List<List<float>>> tc ){
        for (int k=0; k<tray.Count; k++){ // el de las trayectorias
            Posiciones_robot.error.Clear();
            Posiciones_robot.error_cart.Clear();
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
                
                // Calulo del error cartesiano
                var pos_cart_deseadas = Posiciones_robot.POS_CAR.GetRange(0,3);
                var pos_cart_obte = puma_modelo.mgd_puma(Posiciones_robot.POS_ART_REAL.Select(x=>x*Mathf.Deg2Rad).ToArray()).GetRange(0,3);
                Posiciones_robot.error_cart.Add(Mathf.Sqrt (pos_cart_deseadas.Zip(pos_cart_obte, (a,b) => Mathf.Pow(a-b,2)).Sum()));
            

                StartCoroutine(com_udp.trasnmitir(tray_send));

                // Se actualizan las posiciones
                Posiciones_robot.POS_ART_PAS_REAL = Posiciones_robot.POS_ART_REAL;
                Posiciones_robot.POS_ART_REAL = com_udp.get_values();


                yield return TIEMPO_MUESTREO;   
            }
            com_udp.cerrar_cliente();

            // Se cargan a la interfaz de las gráficas
            graf_script.asignar_trays(this.tray.Transpuesta(Posiciones_robot.error, true));
            graf_script.asignar_ECM(Posiciones_robot.error_cart);
        }

    }
}
