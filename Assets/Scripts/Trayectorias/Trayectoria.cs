using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RobSof.Assets.Scripts.Interface.Articular;

public class Trayectoria
{
    public static float TIEMPO_MUESTREO = 0.02f;
    private PUMA_modelo puma_modelo = new PUMA_modelo();
    Rangos_arts rangos_arts = new Rangos_arts();

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

    public (List<List<float>>, List<List<float>>) tray_articular(float [] pos_inicial, float [] pos_final, int T_FINAL, int num_arts){
        List<List<float>> tray = new List<List<float>>();
        List<List<float>> tray_car = new List<List<float>>();
        for (int j=0; j<num_arts; j++){
            tray.Add(grado_5(pos_inicial[j], pos_final[j], T_FINAL));
        }
        for (int i=0; i<tray[0].Count; i++){
            float [] tart = new float[num_arts];
            for (int j=0; j<num_arts; j++){
                tart[j] =  tray[j][i]*Mathf.Deg2Rad;
            }
            tray_car.Add(puma_modelo.mgd_puma(tart)); // Angulos en radianes
        }
        return  (tray, Transpuesta(tray_car, true));
    }
    public (List<List<float>>, List<List<float>>)tray_cartesiana(float [] pos_inicial, float [] pos_final, int T_FINAL, int num_arts){
        List<List<float>> tray;
        List<List<float>> tc = new List<List<float>>(); // tray cartesiana
        List<List<float>> tt = new List<List<float>>(); // tray articular transpuesta

        for (int j=0; j<6; j++){ // va hasta 6 pues son {x,y,z rotx, roty, rotz}
            tc.Add(grado_5(pos_inicial[j], pos_final[j], T_FINAL));
        }
       
        // Llamada del MGI
        for (int i=0; i<tc[0].Count; i++){
            tt.Add(puma_modelo.mgi_puma(tc[0][i], tc[1][i], tc[2][i], tc[3][i], tc[4][i], tc[5][i]));
        }

        // Transponer Matriz tt
        tray = Transpuesta(tt);

        return (tray, tc);
    }

    public List<List<float>> Transpuesta(List<List<float>> tt){
        List<List<float>> tray = new List<List<float>>();
        for (int i =0; i<tt[0].Count;i++){
            List<float> _tray = new List<float>();
            for (int j=0; j<tt.Count; j++){
                _tray.Add(tt[j][i]*Mathf.Rad2Deg);
            }
            tray.Add(_tray);
        }
        return tray;
    }
    public List<List<float>> Transpuesta(List<List<float>> tt, bool bol){
        List<List<float>> tray = new List<List<float>>();
        for (int i =0; i<tt[0].Count;i++){
            List<float> _tray = new List<float>();
            for (int j=0; j<tt.Count; j++){
                _tray.Add(tt[j][i]);
            }
            tray.Add(_tray);
        }
        return tray;
    }

    public (List<List<float>>, List<List<float>>) tray_circular(float x, float y, float z, float radio, float t_final){
        List<List<float>> tc = new List<List<float>>(); // tray cartesiana
        List<List<float>> tt = new List<List<float>>(); // tray articular transpuesta

        float rot_x = rangos_arts.posiciones_iniciales_cartesianas[3];
        float rot_y = rangos_arts.posiciones_iniciales_cartesianas[4];
        float rot_z = rangos_arts.posiciones_iniciales_cartesianas[5];

        int num_steps = Mathf.RoundToInt(t_final/TIEMPO_MUESTREO);

        // Trayectoria cartesiana
        for (int j=0; j<num_steps; j++){
            float x_t = x ; 
            float y_t = y + radio*Mathf.Sin(2 * Mathf.PI * j / num_steps);
            float z_t = z + radio*Mathf.Cos(2 * Mathf.PI * j / num_steps);
            tc.Add(new List<float>(){x_t,y_t,z_t,rot_x, rot_y, rot_z});
        }
        // Trayectoria articular 
        for (int j=0; j<tc.Count;j++){
            tt.Add(puma_modelo.mgi_puma(tc[j][0], tc[j][1], tc[j][2], tc[j][3], tc[j][4], tc[j][5]));
        }

        return (Transpuesta(tt), Transpuesta(tc, true));
    }
    string probarstr(float[] values){
        string st = "";
        foreach (var item in values){
            st = st +" % % " + item.ToString();
        }
        return st;
    }
    
}
