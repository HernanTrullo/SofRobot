using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUMA_modelo
{
    private float d3 = 0.63198f;
    private float r4 = 0.5182f;

    public float [] mgd_puma(float t1,float t2,float t3, float t4,float t5,float t6){

        float C1 = Coseno(t1);
        float C2 = Coseno(t2);
        float C3 = Coseno(t3);
        float C4 = Coseno(t4);
        float C5 = Coseno(t5);
        float C6 = Coseno(t6);
        float S1 = Seno(t1);
        float S2 = Seno(t2);
        float S3 = Seno(t3);
        float S4 = Seno(t4);
        float S5 = Seno(t5);
        float S6 = Seno(t6);
        float C23 = Coseno(t2 + t3);
        float S23 = Coseno(t2 + t3);


        float xa = Coseno(t1)*((-Seno(t2 + t3)*r4 + Coseno(t2)*d3));
        float ya = Seno(t1)*((-Seno(t2 + t3)*r4 + Coseno(t2)*d3));
        float za = Coseno(t2 + t3)*r4 + Seno(t2)*d3;

        float sx = - S6*(S4*(C1*C2*C3 - C1*S2*S3) + C4*S1) - C6*(C5*(S1*S4 - C4*(C1*C2*C3 - C1*S2*S3)) + S5*(C1*C2*S3 + C1*C3*S2));
        float sy = C6*(C5*(C4*(C2*C3*S1 - S1*S2*S3) + C1*S4) - S5*(C2*S1*S3 + C3*S1*S2)) - S6*(S4*(C2*C3*S1 - S1*S2*S3) - C1*C4);
        float sz = - C6*(S5*(S2*S3 - C2*C3) - C4*C5*(C2*S3 + C3*S2)) - S4*S6*(C2*S3 + C3*S2);
        float nx = S6*(C5*(S1*S4 - C4*(C1*C2*C3 - C1*S2*S3)) + S5*(C1*C2*S3 + C1*C3*S2)) - C6*(S4*(C1*C2*C3 - C1*S2*S3) + C4*S1);
        float ny = - S6*(C5*(C4*(C2*C3*S1 - S1*S2*S3) + C1*S4) - S5*(C2*S1*S3 + C3*S1*S2)) - C6*(S4*(C2*C3*S1 - S1*S2*S3) - C1*C4);
        float nz = S6*(S5*(S2*S3 - C2*C3) - C4*C5*(C2*S3 + C3*S2)) - C6*S4*(C2*S3 + C3*S2);
        float ax = S5*(S1*S4 - C4*(C1*C2*C3 - C1*S2*S3)) - C5*(C1*C2*S3 + C1*C3*S2);
        float ay = - C5*(C2*S1*S3 + C3*S1*S2) - S5*(C4*(C2*C3*S1 - S1*S2*S3) + C1*S4);
        float az = - C5*(S2*S3 - C2*C3) - C4*S5*(C2*S3 + C3*S2);

        Matrix4x4 rotm = Matrix4x4.identity;
        rotm.m00 = sx; rotm.m01 = nx; rotm.m02 = ax;
        rotm.m10 = sy; rotm.m11 = ny; rotm.m12 = ay;
        rotm.m20 = sz; rotm.m21 = nz; rotm.m22 = az;


        float [] angles = rotm2euler(rotm);
        float [] coordenate = new float[] {xa, ya, za, angles[0], angles[1], angles[2]};
        return coordenate;
    }
    public float [] rotm2euler(Matrix4x4 rotm, bool f){

        Quaternion rotation = Quaternion.LookRotation(rotm.GetColumn(1), rotm.GetColumn(2));
        Vector3 euler_angles = rotation.eulerAngles;

        if (euler_angles.x >= 90.0f && euler_angles.x <= 270.0f){
            euler_angles.y = 180.0f - euler_angles.y;
            euler_angles.z = 180.0f - euler_angles.z;
        }
    
        return new float [] {euler_angles.x, euler_angles.y, euler_angles.z};
    }
    public float [] rotm2euler(Matrix4x4 rotm){
        Vector3 angles = new Vector3();
        float threshold = 0.9999f;

        angles.y = Mathf.Asin(rotm.m20);
        // Checheo del gimbal lock
        if (Mathf.Abs(rotm.m20) > threshold){
            angles.x = (float)Mathf.Atan2(rotm.m01, rotm.m02);
            angles.z = 0.0f;
        }
        else{
            angles.x = (float)Mathf.Atan2(-rotm.m21, rotm.m22);
            angles.z = (float)Mathf.Atan2(-rotm.m10, rotm.m00);
        }
        Vector3 angles_grados = angles*Mathf.Rad2Deg;

        return new float[] {angles.x, angles.y, angles.z};
    }

    // Función que calcula el angulo en radianes
    private float Coseno(float angle){
        if (Mathf.Abs(Mathf.Cos(angle)) < double.Epsilon){
            return 0;
        }
        else{
            return Mathf.Cos(angle);
        }
    }
    private float Seno(float angle){
        if (Mathf.Abs(Mathf.Sin(angle)) < double.Epsilon){
            return 0;
        }
        else{
            return Mathf.Sin(angle);
        }
    }
}
