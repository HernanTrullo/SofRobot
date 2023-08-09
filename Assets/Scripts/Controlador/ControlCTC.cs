using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ControlCTC
{
    private int[] KP = new int[]{10000,10000,10000,10000,10000,10000}; 
    private int[] KD = new int[]{10,10,10,10,10,10};
    // Start is called before the first frame update
    
    private PUMA_modelo puma_mod = new PUMA_modelo();  

    public ControlCTC(){
        
    }

    public (float[], List<float>) retunrTorques(List<float> pos_des, List<float> pos_rob, float[] vel_rob){
        // Se obtiene el error
        List<float> error = new List<float>(){0,0,0,0,0,0};

        // Cálculo de las aceleraciones en radianes todas las unidades
        pos_des = deg2rad(pos_des.ToArray());
        pos_rob = deg2rad(pos_rob.ToArray());
        vel_rob = deg2rad(vel_rob).ToArray();

        float[] acel = new float[]{0,0,0,0,0,0};
        for (int j=0; j<pos_rob.Count; j++){
            error[j] = pos_des[j]-pos_rob[j];
            acel[j] = KP[j]*(error[j]) - KD[j]*vel_rob[j];
        }

        // Cálculo de los torques usando el modelo dinámico inverso
        return (puma_mod.puma_inverso(pos_rob.ToArray(), vel_rob, acel), error.Select(x=> x*Mathf.Rad2Deg).ToList());

    }
    List<float> deg2rad(float[] values){
        List<float> rad_values = new List<float>(){0,0,0,0,0,0};
        for (int i = 0; i < rad_values.Count; i++)
        {
            rad_values[i] = values[i]*Mathf.Deg2Rad;
        }
        return rad_values;
    }
}
