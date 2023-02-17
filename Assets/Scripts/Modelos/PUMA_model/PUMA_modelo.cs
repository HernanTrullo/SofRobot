using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUMA_modelo
{
    public float d3 = 0.63198f;
    public float  r4 = 0.5182f;

    public List<float> mgd_puma(float [] q){

        float C1 = Coseno(q[0]);
        float C2 = Coseno(q[1]);
        float C3 = Coseno(q[2]);
        float C4 = Coseno(q[3]);
        float C5 = Coseno(q[4]);
        float C6 = Coseno(q[5]);
        float S1 = Seno(q[0]);
        float S2 = Seno(q[1]);
        float S3 = Seno(q[2]);
        float S4 = Seno(q[3]);
        float S5 = Seno(q[4]);
        float S6 = Seno(q[5]);
        float C23 = Coseno(q[1] + q[2]);
        float S23 = Coseno(q[1] + q[2]);


        float xa = Coseno(q[0])*((-Seno(q[1] + q[2])*r4 + Coseno(q[1])*d3));
        float ya = Seno(q[0])*((-Seno(q[1] + q[2])*r4 + Coseno(q[1])*d3));
        float za = Coseno(q[1] + q[2])*r4 + Seno(q[1])*d3;

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
        return new List<float> {xa, ya, za, angles[0], angles[1], angles[2]};
        
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

        return new float[] {angles_grados.x, angles_grados.y, angles_grados.z};
    }

    public List<float> mgi_puma(float posx, float posy, float posz, float rotx, float roty, float rotz){
        Matrix4x4 Mrot = euler2rotm(rotx, roty, rotz);

        float sx= Mrot.m00;
        float sy= Mrot.m10;
        float sz= Mrot.m20;

        float nx= Mrot.m01;
        float ny= Mrot.m11;
        float nz= Mrot.m21;

        float ax= Mrot.m02;
        float ay= Mrot.m12;
        float az= Mrot.m22;

        float Px = posx;
        float Py = posy;
        float Pz = posz;

        float q1 = 0;
        //Calculo de q1:
        if (Px <= Mathf.Epsilon){
            q1 = 0;
        }else{
            q1 = Mathf.Atan2(Py,Px);
        }
        

        //Calculo de q2 y q3:

        float X = -2*Pz*d3;
        float B1 = Px*Coseno(q1) + Py*Seno(q1);
        float Y = -2*B1*d3;
        float Z = Pow(r4,2) - Pow(d3,2) - Pow(Pz,2) - Pow(B1,2);

        float C2 = (Y*Z + X*Sqrt(Pow(X,2) + Pow(Y,2) - Pow(Z,2)))/( Pow(X,2) + Pow(Y,2));
        float S2 = (X*Z - Y*Mathf.Sqrt(Pow(X,2) + Pow(Y,2) - Pow(Z,2)))/(Pow(X,2) + Pow(Y,2));

        float q2 = Mathf.Atan2(S2,C2);

        float S3 = (-Pz*Seno(q2) - B1*Coseno(q2) + d3)/(r4);
        float C3 = (-B1*Seno(q2) + Pz*Coseno(q2))/(r4);
        
        //%S3 = (-Pz*S2 - B1*C2 + d3)/(r4);
        //%C3 = (-B1*S2 + Pz*C2)/(r4);

        float q3 = Mathf.Atan2(S3,C3);

        //Calculo de q4:

        float Hx = Coseno(q2 + q3)*(Coseno(q1)*ax + Seno(q1)*ay) + Seno(q2 + q3)*az;
        float Hy =-Seno(q2 + q3)*(Coseno(q1)*ax + Seno(q1)*ay) + Coseno(q2 + q3)*az;
        float Hz = Seno(q1)*ax - Coseno(q1)*ay;

        float q4 = Mathf.Atan2(Hz,-Hx);
        
        //%q4 = Mathf.Atan2(Hz,-Hx) + pi;

        //Calculo de q5:

        float S5 = -Coseno(q4)*Hx + Seno(q4)*Hz;
        float C5 = Hy;

        float q5 = Mathf.Atan2(S5,C5);
        //Calculo de q6:

        float Fx = Coseno(q2 + q3)*(Coseno(q1)*sx + Seno(q1)*sy) + Seno(q2 + q3)*sz;
        float Fz = Seno(q1)*sx - Coseno(q1)*sy;
        float Gx = Coseno(q2 + q3)*(Coseno(q1)*nx + Seno(q1)*ny) + Seno(q2 + q3)*nz;
        float Gz = Seno(q1)*nx - Coseno(q1)*ny;

        float S6 = -Coseno(q4)*Fz - Seno(q4)*Fx;
        float C6 = -Coseno(q4)*Gz - Seno(q4)*Gx;

        float q6 = Mathf.Atan2(S6,C6);
        if (q4<0){
            q4 = 2*Mathf.PI+q4;
        }
        if (q5<0){
            q5 = 2*Mathf.PI+q5;
        }
        if (q6<0){
            q6 = 2*Mathf.PI+q6;
        }

        return new List<float> {q1, q2, q3, q4, q5, q6};
    }

    

    public Matrix4x4 euler2rotm(float rotx, float roty, float rotz){
        Quaternion rotation = Quaternion.Euler(rotx, roty, rotz);
        Matrix4x4 rotm = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        return rotm;
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

    private float Pow(float value, float exp){
        return Mathf.Pow(value, exp);
    }
    private float Sqrt(float value){
        return Mathf.Sqrt(value);
    }
}
