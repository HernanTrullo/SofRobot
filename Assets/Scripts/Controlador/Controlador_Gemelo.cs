using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public static class Controlador_Gemelos 
{
    public static IEnumerator mover_robot(List<List<float>> tray, List<List<float>> tc, Gemelo_digital PUMA_script, int TIEMPO_MUESTREO){
        for (int i=0; i<tray[0].Count; i++ ){
            for (int j=0; j<tray.Count; j++){
                
                // Actualización Posiciones Articulares
                Posiciones_robot.POS_ART[j]= tray[j][i];
                Posiciones_robot.pos_art[j].text = tray[j][i].ToString("0.##");

                // Actualización Posiciones Cartesianas
                Posiciones_robot.POS_CAR[j] = tc[j][i];
                Posiciones_robot.pos_car[j].text = tc[j][i].ToString("0.##");
            } 
            Thread.Sleep(TIEMPO_MUESTREO);
            yield return 0;   
        }
    }
    public static IEnumerator mover_robot_tray (List<List<List<float>>> tray,List<List<List<float>>> tc, Gemelo_digital PUMA_script, int TIEMPO_MUESTREO ){
        for (int k=0; k<tray.Count; k++){ // el de las trayectorias
            for (int i=0; i<tray[0][0].Count; i++ ){
                for (int j=0; j<tray[0].Count; j++){
                    // Actualización Posiciones Articulares
                    Posiciones_robot.POS_ART[j]= tray[k][j][i];
                    Posiciones_robot.pos_art[j].text = tray[k][j][i].ToString("0.##");

                    // Actualización Posiciones Cartesianas
                    Posiciones_robot.POS_CAR[j] = tc[k][j][i];
                    Posiciones_robot.pos_car[j].text = tc[k][j][i].ToString("0.##");
                } 
                Thread.Sleep(TIEMPO_MUESTREO);
            yield return 0;   
        }
        }

    }
}
