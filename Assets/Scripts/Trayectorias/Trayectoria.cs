using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Trayectoria
{
    public static float TIEMPO_MUESTREO = 0.005f;

    public List<float> grado_5(float pos_inicial, float pos_final, int T_FINAL){
        List<float> tray = new List<float>();
        int num_steps = Mathf.RoundToInt(T_FINAL/TIEMPO_MUESTREO);
        float delta_pos = pos_final-pos_inicial;
        float temps = 0;
        for (int j= 0; j< num_steps; j++ ){
            tray.Add(pos_inicial + delta_pos*(10*Mathf.Pow(temps/T_FINAL, 3) - 15*Mathf.Pow(temps/T_FINAL,4) 
            + 6*Mathf.Pow(temps/T_FINAL,5)));
            temps += TIEMPO_MUESTREO;
        }
        return tray;
    }

    public List<List<float>> tray_articular(float [] pos_inicial, float [] pos_final, int T_FINAL, int num_arts){
        List<List<float>> tray = new List<List<float>>();
        for (int j=0; j<num_arts; j++){
            tray.Add(grado_5(pos_inicial[j], pos_final[j], T_FINAL));
        }
        return  tray;
    }
    
}
